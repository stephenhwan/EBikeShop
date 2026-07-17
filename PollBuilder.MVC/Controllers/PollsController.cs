using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PollBuilder.MVC.Services.Interfaces;

namespace PollBuilder.MVC.Controllers
{
	[Authorize]   // cần using Microsoft.AspNetCore.Authorization;
	[Route("poll")]
	public class PollsController : Controller
	{
		private readonly IPollApiClient _pollApiClient;

		public PollsController(IPollApiClient pollApiClient)
		{
			_pollApiClient = pollApiClient;
		}

		[HttpGet("{url}/results")]
		public async Task<IActionResult> Result(string url)
		{
			var dto = await _pollApiClient.GetPollResultAsync(url);

			if (dto == null)
				return NotFound();

			return View(dto);
		}

		[HttpPost("{url}/vote")]
		public async Task<IActionResult> Vote(string url, List<Guid> selectedOptionIds)
		{
			var success = await _pollApiClient.SubmitVoteAsync(url, selectedOptionIds);

			if (!success)
				return BadRequest("Vote thất bại.");

			return RedirectToAction("Result", new { url });
		}
	}
}
