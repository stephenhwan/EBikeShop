using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PollBuilder.MVC.Controllers
{
	public class HomeController : Controller
	{
		public IActionResult Index() => View();
		[Authorize]
		[HttpGet]
		public IActionResult CreatePoll() => View();
	}
}
