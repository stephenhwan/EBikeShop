using MediatR;
using Microsoft.EntityFrameworkCore;
using PollBuilder.Application.Interfaces;
using PollBuilder.Common.Contants;
using PollBuilder.Domain.Entities.PollBuilder;


namespace PollBuilder.Application.Features.Polls.Commands.CreatePoll
{
	// 1. Khai báo Handler (Logic xử lý)
	public class CreatePollHandler : IRequestHandler<CreatePollCommand, string>
	{
		private readonly ICurrentUserService _currentUserService;
		private readonly IPollBuilderDbContext _context;

		// Tiêm (Inject) IPollBuilderDbContext vào để tương tác với Database
		public CreatePollHandler(IPollBuilderDbContext context,
		ICurrentUserService currentUserService)
		{
			_currentUserService = currentUserService;
			_context = context;
		}


		// Hàm Handle sẽ được MediatR tự động gọi khi có request
		public async Task<string> Handle(CreatePollCommand request, CancellationToken cancellationToken)
		{
			var code = await GenerateUniqueShortCodeAsync(cancellationToken);
			// Khởi tạo Poll từ dữ liệu của Command
			var poll = new Poll
			{
				Url = code,
				Title = request.Title,
				Status = true,
				StartAt = request.StartAt,
				EndAt = request.EndAt,
				UserId = _currentUserService.UserId,
				CreatedAt = DateTime.UtcNow,

				Questions = new List<Question>()
			};
			foreach (var questionRequest in request.Questions)
			{
				var question = new Question
				{
					QuestionText = questionRequest.QuestionText,
					Position = questionRequest.Position,
					Options = new List<Option>()
				};
				foreach (var optionRequest in questionRequest.Options)
				{
					question.Options.Add(
						new Option
						{
							OptionText = optionRequest.OptionText,
							Position = optionRequest.Position
						});
				}

				poll.Questions.Add(question);
			}

			_context.Polls.Add(poll);
			await _context.SaveChangesAsync(cancellationToken);

			return poll.Url;
		}


		// Sinh mã ngắn 5-6 ký tự alphanumeric, không cần đụng DB để check trùng
		// (xác suất trùng cực thấp với 62^6 tổ hợp; nếu muốn chắc chắn có thể loop check tồn tại)
		private async Task<string> GenerateUniqueShortCodeAsync(CancellationToken cancellationToken)
		{
			const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
			var random = new Random();
			string code;
			bool exists;
			do
			{
				code = new string(Enumerable.Range(0, 6).Select(_ => chars[random.Next(chars.Length)]).ToArray());
				// Cast to IQueryable to ensure EF Core's IQueryable AnyAsync overload is chosen (avoid ambiguity with IAsyncEnumerable extensions)
				exists = await _context.Polls.AsQueryable().AnyAsync(p => p.Url == code, cancellationToken);
			} while (exists);
			return code;
		}
	}
}
