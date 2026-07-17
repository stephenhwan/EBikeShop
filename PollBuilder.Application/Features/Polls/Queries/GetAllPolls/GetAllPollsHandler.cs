using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PollBuilder.Application.DTOs;
using PollBuilder.Application.Interfaces;
using PollBuilder.Domain.Entities.PollBuilder;

namespace PollBuilder.Application.Features.Polls.Queries.GetAllPolls
{
	public class GetAllPollsHandler : IRequestHandler<GetAllPollsQuery, List<PollSummaryDto>>
	{
		private readonly IPollBuilderDbContext _context;

		public GetAllPollsHandler(IPollBuilderDbContext context)
		{
			_context = context;
		}

		public async Task<List<PollSummaryDto>> Handle(GetAllPollsQuery request, CancellationToken cancellationToken)
		{
			return await _context.Polls
				.AsNoTracking() // chỉ đọc, không cần track
				.OrderByDescending(p => p.CreatedAt)
				.Select(p => new PollSummaryDto
				{
					Id = p.Id,
					Title = p.Title,
					Url = p.Url,
					Status = p.Status,
					CreatedAt = p.CreatedAt
				})
				.ToListAsync(cancellationToken);
		}
	}
}
