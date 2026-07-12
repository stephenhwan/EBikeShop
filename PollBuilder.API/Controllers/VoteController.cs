using MediatR;
using Microsoft.AspNetCore.Mvc;
using PollBuilder.Application.Features.Votes.Commands.SubmitPollVotes;
namespace PollBuilder.API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class VotesController : ControllerBase
	{
		private readonly IMediator _mediator;

		public VotesController(IMediator mediator)
		{
			_mediator = mediator;
		}

		// POST: api/votes
		[HttpPost]
		public async Task<IActionResult> Submit([FromBody] SubmitPollVoteCommand command, CancellationToken cancellationToken)
		{
			var success = await _mediator.Send(command, cancellationToken);
			return Ok(new { success });
		}
	}
}
