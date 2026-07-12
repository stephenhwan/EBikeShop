using MediatR;
using PollBuilder.Application.DTOs;

namespace PollBuilder.Application.Features.Polls.Queries.GetPollByCode
{
	// GetPollByCodeQuery.cs
	public record GetPollByCode(
	string Url) : IRequest<PollDto?>;
}
