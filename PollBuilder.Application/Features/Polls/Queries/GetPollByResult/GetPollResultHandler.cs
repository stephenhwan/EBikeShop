using MediatR;
using Microsoft.EntityFrameworkCore;
using PollBuilder.Application.DTOs;
using PollBuilder.Application.Exceptions;
using PollBuilder.Application.Interfaces;
using PollBuilder.Domain.Entities.PollBuilder;

namespace PollBuilder.Application.Features.Polls.Queries.GetPollByResult
{
	public class GetPollResultHandler : IRequestHandler<GetPollResultQuery, PollResultDto>
	{
		private readonly IPollBuilderDbContext _context;

		public GetPollResultHandler(IPollBuilderDbContext context)
		{
			_context = context;
		}

		public async Task<PollResultDto> Handle(GetPollResultQuery request, CancellationToken cancellationToken)
		{
			// 1. Lấy Poll + Question + Option
			var poll = await _context.Polls
				.Include(p => p.Questions)
					.ThenInclude(q => q.Options)
				.FirstOrDefaultAsync(p => p.Url == request.url, cancellationToken)
				?? throw new NotFoundException(nameof(Poll), request.url);
			// 2. Lấy danh sách QuestionId
			var questionId = poll.Questions
				.Select(q => q.Id)
				.ToList();

			// Group vote theo OptionId (đã denormalize sẵn nên query nhanh, không cần join Question)

			var voteCounts = await _context.Votes
					.Where(v =>
						v.IsCurrent &&
						questionId.Contains(v.QuestionId))
					.GroupBy(v => v.OptionId)
					.Select(g => new
					{
						OptionId = g.Key,
						Count = g.Count()
					})
					.ToDictionaryAsync(
						x => x.OptionId,
						x => x.Count,
						cancellationToken);


			// 4. Map Question -> DTO
			var questions = poll.Questions
				.OrderBy(q => q.Position)
				.Select(q => new QuestionResultDto
				{
					Id = q.Id,
					QuestionText = q.QuestionText,

					Options = q.Options
						.OrderBy(o => o.Position)
						.Select(o => new OptionResultDto
						{
							Id = o.Id,
							Position = o.Position,
							OptionText = o.OptionText,
							IsCurrent = voteCounts.ContainsKey(o.Id)
						})
						.ToList()
				})
				.ToList();
			return new PollResultDto
			{
				Url = poll.Url,
				Questions = questions,
				VoteCount = voteCounts.Values.Sum()
			};
		}
	}
}

