using MediatR;
using Microsoft.AspNetCore.Mvc;
using PollBuilder.Application.Features.Polls.Commands.ClosePoll;
using PollBuilder.Application.Features.Polls.Commands.CreatePoll;
using PollBuilder.Application.Features.Polls.Queries.GetPollByCode;
using PollBuilder.Application.Features.Polls.Queries.GetPollByResult;
namespace PollBuilder.API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
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
			return CreatedAtAction(nameof(GetByCode), new { Url = url });
		}

		// GET: api/poll/{code}
		[HttpGet("{code}")]
		public async Task<IActionResult> GetByCode(string code, CancellationToken cancellationToken)
		{
			var poll = await _mediator.Send(new GetPollByCode(code), cancellationToken);

			if (poll == null)
				return NotFound();

			return Ok(poll);
		}

		// GET: api/poll/{code}/results
		[HttpGet("{code}/results")]
		public async Task<IActionResult> GetResults(string code, CancellationToken cancellationToken)
		{
			var result = await _mediator.Send(new GetPollResultQuery(code), cancellationToken);
			return Ok(result);
		}

		// PUT: api/polls/{id}/close
		[HttpPut("{id}/close")]
		public async Task<IActionResult> Close(Guid id, CancellationToken cancellationToken)
		{
			var success = await _mediator.Send(new ClosePollCommand(id), cancellationToken);
			return Ok(new { success });
		}
	}
}
