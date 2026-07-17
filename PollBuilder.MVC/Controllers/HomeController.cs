using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PollBuilder.MVC.Services.Interfaces;
using PollBuilder.MVC.ViewModels.PollBuilder;

namespace PollBuilder.MVC.Controllers
{
	public class HomeController : Controller
	{
		private readonly IPollApiClient _pollApiClient;

		public HomeController(IPollApiClient pollApiClient)
		{
			_pollApiClient = pollApiClient;
		}
		public IActionResult Index() => View();

		[Authorize]
		[HttpGet]
		public IActionResult CreatePoll() => View(new CreatePollVM());


		[Authorize]
		[HttpGet]
		public async Task<IActionResult> Results()
		{
			var polls = await _pollApiClient.GetAllPollsAsync();
			return View(polls); 
		}
	}
}
