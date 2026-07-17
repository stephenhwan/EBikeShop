using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PollBuilder.MVC.Contracts.Requests;
using PollBuilder.MVC.Services.Interfaces;

namespace PollBuilder.MVC.Controllers
{
	[Authorize]   // cần using Microsoft.AspNetCore.Authorization;
	[Route("poll")]
	public class PollsController : Controller
	{
		private readonly IPollApiClient _pollApiClient;

		[HttpGet("{url}/vote")]
		public async Task<IActionResult> Vote(string url)
		{
			var poll = await _pollApiClient.GetPollAsync(url);
			if (poll == null) return NotFound();
			return View(poll);
		}

		// Action POST Vote đã có sẵn — chỉ cần sửa thêm [FromBody]
		// vì JS bên dưới gửi JSON array, không phải form data:
		[HttpPost("{url}/vote")]
		public async Task<IActionResult> Vote(string url, [FromBody] List<Guid> selectedOptionIds)
		{
			var success = await _pollApiClient.SubmitVoteAsync(url, selectedOptionIds);
			if (!success) return BadRequest("Vote thất bại.");
			return Ok(new { success });
		}

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


		[HttpPost("create")]
		public async Task<IActionResult> Create([FromBody] CreatePollRequestDto request)
		{
			var url = await _pollApiClient.CreatePollAsync(request);

			if (string.IsNullOrEmpty(url))
				return BadRequest("Tạo poll thất bại.");

			return Ok(new { url });
		}
	}
}
