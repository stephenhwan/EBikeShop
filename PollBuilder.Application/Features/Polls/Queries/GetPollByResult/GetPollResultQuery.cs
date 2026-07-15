
using MediatR;
using PollBuilder.Application.DTOs;

namespace PollBuilder.Application.Features.Polls.Queries.GetPollByResult
{
	// GetPollResultsQuery.cs
	public record GetPollResultQuery(string url) : IRequest<PollResultDto>;
}
