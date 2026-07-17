using Microsoft.AspNetCore.Mvc;

namespace PollBuilder.MVC.Controllers
{
	public class HomeController : Controller
	{
		public IActionResult Index() => View();
	}
}
