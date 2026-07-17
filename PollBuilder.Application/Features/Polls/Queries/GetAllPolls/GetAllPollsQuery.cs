using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using PollBuilder.Application.DTOs;

namespace PollBuilder.Application.Features.Polls.Queries.GetAllPolls
{
	public record GetAllPollsQuery() : IRequest<List<PollSummaryDto>>;
}
