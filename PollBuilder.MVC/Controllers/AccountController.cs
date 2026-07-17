using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using PollBuilder.MVC.Services.Interfaces;
using PollBuilder.MVC.ViewModels.Identity;
namespace PollBuilder.MVC.Controllers
{
	[Route("account")]
	public class AccountController : Controller
	{
		private readonly IAuthApiClient _authApiClient;
		public AccountController(IAuthApiClient authApiClient) => _authApiClient = authApiClient;

		[HttpGet("login")]
		public IActionResult Login(string returnUrl = "/")
		{
			var loginVM = new LoginVM
			{
				ReturnUrl = returnUrl
			};
			return View(loginVM);
		}

		[HttpPost("login")]
		public async Task<IActionResult> Login(string email, string password)
		{
			var result = await _authApiClient.LoginAsync(email, password);

			if (result == null || string.IsNullOrEmpty(result.Token))
			{
				ModelState.AddModelError(string.Empty, "Email hoặc mật khẩu không đúng.");
				return View();
			}

			// Decode JWT để lấy claims có sẵn trong token (UserId, Email...)
			var handler = new JwtSecurityTokenHandler();
			var jwtToken = handler.ReadJwtToken(result.Token);

			// ClaimsIdentity mới cho Cookie, LẤY LẠI toàn bộ claims đã có trong JWT
			var identity = new ClaimsIdentity(
				jwtToken.Claims,
				CookieAuthenticationDefaults.AuthenticationScheme,
				nameType: JwtRegisteredClaimNames.Sub, // hoặc ClaimTypes.NameIdentifier tùy API set tên claim nào
				roleType: ClaimTypes.Role);

			var principal = new ClaimsPrincipal(identity);

			var authProperties = new AuthenticationProperties();
			authProperties.StoreTokens(new[]
			{
				new AuthenticationToken { Name = "access_token", Value = result.Token }
			});

			await HttpContext.SignInAsync(
				CookieAuthenticationDefaults.AuthenticationScheme,
				principal,
				authProperties);

			return RedirectToAction("Index", "Home");
		}

	//// for register controller 
		[HttpGet("register")]
		public IActionResult Register(string returnUrl = "/")
		{
			var registerVM = new RegisterVM
			{
				ReturnUrl = returnUrl
			};
			return View(registerVM);
		}
		[HttpPost("register")]
		public async Task<IActionResult> Register(RegisterVM registerVM)
		{
			if (!ModelState.IsValid)
				return View(registerVM);

			var result = await _authApiClient.RegisterAsync(
				registerVM.Email, registerVM.Password, registerVM.ConfirmPassword, registerVM.FullName);

			if (result == null || string.IsNullOrEmpty(result.Token))
			{
				ModelState.AddModelError(string.Empty, "Đăng ký thất bại. Email có thể đã tồn tại.");
				return View(registerVM);
			}

			// Auto login luôn sau khi register thành công
			SignInWithToken(result.Token);
			return Redirect(registerVM.ReturnUrl);
		}

		[HttpPost("logout")]
		public async Task<IActionResult> Logout()
		{
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			return RedirectToAction("Index", "Home");
		}

		// Gom logic decode JWT + SignIn dùng chung cho cả Login và Register
		private void SignInWithToken(string token)
		{
			var handler = new JwtSecurityTokenHandler();
			var jwtToken = handler.ReadJwtToken(token);

			var identity = new ClaimsIdentity(
				jwtToken.Claims,
				CookieAuthenticationDefaults.AuthenticationScheme,
				nameType: JwtRegisteredClaimNames.Sub,
				roleType: ClaimTypes.Role);		

			var principal = new ClaimsPrincipal(identity);

			var authProperties = new AuthenticationProperties();
			authProperties.StoreTokens(new[]
			{
				new AuthenticationToken { Name = "access_token", Value = token }
			});

			HttpContext.SignInAsync(
				CookieAuthenticationDefaults.AuthenticationScheme,
				principal,
				authProperties).GetAwaiter().GetResult();
		}
	}
}
