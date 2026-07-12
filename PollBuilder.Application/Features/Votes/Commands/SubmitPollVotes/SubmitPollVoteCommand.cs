using MediatR;

namespace PollBuilder.Application.Features.Votes.Commands.SubmitPollVotes
{
	public record SubmitPollVoteCommand(
	Guid PollId,
	Guid UserId,
	List<Guid> SelectedOptionIds) : IRequest<bool>;

}
