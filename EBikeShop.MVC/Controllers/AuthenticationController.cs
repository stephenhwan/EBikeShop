using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using EBikeShop.MVC.ViewModels;

namespace EBikeShop.MVC.Controllers
{
	public class AuthenticationController : Controller
	{
		private readonly IHttpContextAccessor _httpContextAccessor;
		public AuthenticationController(IHttpContextAccessor httpContextAccessor)
		{
			_httpContextAccessor = httpContextAccessor;
		}
		public IActionResult Index()
		{
			var context = _httpContextAccessor.HttpContext;
			// Example: Get request path
			string path = context.Request.Path;
			var user = context?.User;
			// Check login status
			if (user == null || !user.Identity.IsAuthenticated)
			{
				return Unauthorized("You must be logged in.");
			}
			// Check role
			if (user.IsInRole("Admin"))
			{
				return Content("Welcome, Admin!");
			}
			// Example: Access session
			string? userId = context.Session.GetString("UserId");
			return Content($"Request Path: {path}, UserId: {userId}");
		}
	}
}
