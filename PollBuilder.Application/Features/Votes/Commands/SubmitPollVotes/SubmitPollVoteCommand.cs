using MediatR;

namespace PollBuilder.Application.Features.Votes.Commands.SubmitPollVotes
{
	public record SubmitPollVoteCommand(
	string Url,
	string UserId,
	List<Guid> SelectedOptionIds) : IRequest<bool>;

}
