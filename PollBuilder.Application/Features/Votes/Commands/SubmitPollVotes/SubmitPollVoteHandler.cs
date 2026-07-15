// SubmitPollVotesHandler.cs
using MediatR;
using Microsoft.EntityFrameworkCore;
using PollBuilder.Application.Exceptions;
using PollBuilder.Application.Features.Votes.Commands.SubmitPollVotes;
using PollBuilder.Application.Interfaces;
using PollBuilder.Common.Contants;
using PollBuilder.Domain.Entities.PollBuilder;


public class SubmitPollVoteHandler : IRequestHandler<SubmitPollVoteCommand, bool>
{
	private readonly IPollBuilderDbContext _context;
	//private readonly IPollHubService _hubService;
	private readonly IMediator _mediator; // dùng lại GetPollResultsQuery để khỏi lặp code

	public SubmitPollVoteHandler(
		IPollBuilderDbContext context,
		//IPollHubService hubService,
		IMediator mediator)
	{
		_context = context;
		//_hubService = hubService;
		_mediator = mediator;
	}

	public async Task<bool> Handle(SubmitPollVoteCommand request, CancellationToken cancellationToken)
	{
		var poll = await _context.Polls
			.Include(p => p.Questions)
			.ThenInclude(q => q.Options)
			.FirstOrDefaultAsync(p => p.Url == request.Url, cancellationToken);

		if (poll == null)
		{
			throw new NotFoundException(nameof(Poll), request.Url); 
		}

		if (poll.Status == false || poll.ClosedAt < DateTime.UtcNow)
		{
			throw new ValidationException("Cuộc bình chọn này đã kết thúc hoặc bị đóng."); 
		}
		var validOptionIds = poll.Questions
			.SelectMany(q => q.Options, (question, option) => new
			{
				Question = question,
				Option = option
			})
			.ToDictionary(x => x.Option.Id);

		var newVotes = new List<Vote>();
		//xem option thuoc question nao 
		var selectedQuestions = new HashSet<Guid>();
		// 3. Validate dữ liệu đầu vào & Chuẩn bị list Vote mới
		foreach (var optionId in request.SelectedOptionIds)
		{
			// Bảo mật: Kiểm tra OptionId có tồn tại và thuộc về Poll này không (Tránh KeyNotFoundException)
			if (!validOptionIds.TryGetValue(optionId, out var item))
			{
				throw new ValidationException($"OptionId {optionId} không hợp lệ hoặc không thuộc về Poll này.");
			}

			// Logic: Đảm bảo người dùng không gửi 2 Option cho cùng 1 Question trong 1 lần submit
			if (!selectedQuestions.Add(item.Question.Id))
			{
				throw new ValidationException($"Bạn chỉ được phép chọn 1 Option cho mỗi câu hỏi (Lỗi tại câu hỏi: {item.Question.Id}).");
			}

			newVotes.Add(new Vote
			{
				UserId = request.UserId,
				QuestionId = item.Question.Id,
				OptionId = item.Option.Id,
				CreatedAt = DateTime.UtcNow,
				IsCurrent = true
			});
		}

		// 4. Mở Transaction để xử lý Database (Đảm bảo Atomicity)
		using var transaction = await _context.BeginTransactionAsync(cancellationToken);
		try
		{
			// Lấy các Vote cũ của user đối với những câu hỏi vừa được trả lời lại
			var currentVotes = await _context.Votes
				.Where(v => v.UserId == request.UserId &&
							v.IsCurrent &&
							selectedQuestions.Contains(v.QuestionId))
				.ToListAsync(cancellationToken);

			// Cập nhật Vote cũ thành false
			foreach (var vote in currentVotes)
			{
				vote.IsCurrent = false;
			}

			// Thêm các Vote mới
			_context.Votes.AddRange(newVotes);

			// Lưu TẤT CẢ thay đổi (Update cũ & Insert mới) trong cùng 1 lần Save
			await _context.SaveChangesAsync(cancellationToken);

			// Chỉ khi SaveChanges thành công mới Commit
			await transaction.CommitAsync(cancellationToken);
		}
		catch
		{
			await transaction.RollbackAsync(cancellationToken);
			throw;
		}
		return true;
	}
}