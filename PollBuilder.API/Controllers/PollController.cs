using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PollBuilder.Application.Features.Polls.Commands.ClosePoll;
using PollBuilder.Application.Features.Polls.Commands.CreatePoll;
using PollBuilder.Application.Features.Polls.Queries.GetPollByCode;
using PollBuilder.Application.Features.Polls.Queries.GetPollByResult;
using PollBuilder.Application.Features.Votes.Commands.SubmitPollVotes;
namespace PollBuilder.API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
	public class PollController : ControllerBase
	{
		private readonly IMediator _mediator;

		public PollController(IMediator mediator)
		{
			_mediator = mediator;
		}

		// POST: api/poll
		[HttpPost]
		public async Task<IActionResult> Create([FromBody] CreatePollCommand command, CancellationToken cancellationToken)
		{
			var url = await _mediator.Send(command, cancellationToken);
			return Ok(new { Url = url });
		}

		// GET: api/poll/{url}
		[HttpGet("{url}")]
		public async Task<IActionResult> GetByCode(string url, CancellationToken cancellationToken)
		{
			var poll = await _mediator.Send(new GetPollByCode(url), cancellationToken);

			if (poll == null)
				return NotFound();

			return Ok(poll);
		}

		// GET: api/poll/{url}/results
		[HttpGet("{url}/results")]
		public async Task<IActionResult> GetResults(string url, CancellationToken cancellationToken)
		{
			var result = await _mediator.Send(new GetPollResultQuery(url), cancellationToken);
			return Ok(result);
		}

		// PUT: api/polls/{id}/close
		[HttpPut("{id}/close")]
		public async Task<IActionResult> Close(Guid id, CancellationToken cancellationToken)
		{
			var success = await _mediator.Send(new ClosePollCommand(id), cancellationToken);
			return Ok(new { success });
		}
		// POST: api/poll/vote
		[HttpPost("{url}/vote")]
		public async Task<IActionResult> SubmitVote(
		string url,
		[FromBody] List<Guid> selectedOptionIds,
		CancellationToken cancellationToken)
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			var command = new SubmitPollVoteCommand(url, userId, selectedOptionIds);
			var result = await _mediator.Send(command, cancellationToken);
			return Ok(new { success = result });
		}
	}
}
