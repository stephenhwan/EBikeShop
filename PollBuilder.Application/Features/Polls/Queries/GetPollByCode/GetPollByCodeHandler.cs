using MediatR;
using Microsoft.EntityFrameworkCore;
using PollBuilder.Application.DTOs;
using PollBuilder.Application.Exceptions;
using PollBuilder.Application.Interfaces;
using PollBuilder.Domain.Entities;


namespace PollBuilder.Application.Features.Polls.Queries.GetPollByCode
{
	public class GetPollByCodeHandler : IRequestHandler<GetPollByCode, PollDto?>
	{
		private readonly IPollBuilderDbContext _context;

		public GetPollByCodeHandler(IPollBuilderDbContext context)
		{
			_context = context;
		}

		public async Task<PollDto?> Handle(GetPollByCode request, CancellationToken cancellationToken)
		{
			var pollDto = await _context.Polls
						.AsNoTracking() // Kỹ thuật tối ưu hiệu năng tối quan trọng cho Query
						.Where(p => p.Url == request.Url)
						.Select(p => new PollDto
						{
							Id = p.Id,
							Title = p.Title,
							Url = p.Url,
							CreatedAt = p.CreatedAt,
							ClosedAt = p.ClosedAt,
							// Ánh xạ danh sách Question từ Entity sang QuestionDto
							Questions = p.Questions.Select(q => new QuestionDto
							{
								Id = q.Id,
								QuestionText = q.QuestionText,
								Position = q.Position,
								// Ánh xạ danh sách Option từ Entity sang OptionDto
								Options = q.Options.Select(o => new OptionDto
								{
									Id = o.Id,
									OptionText = o.OptionText,
									Position = o.Position,
								}).ToList()

							}).ToList()
						})
						.FirstOrDefaultAsync(cancellationToken);

			return pollDto;
		}
	}
}


