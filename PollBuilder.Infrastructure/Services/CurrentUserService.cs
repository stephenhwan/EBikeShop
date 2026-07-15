using System.Security.Claims;
using Microsoft.AspNetCore.Http;

using PollBuilder.Application.Interfaces;

namespace PollBuilder.Infrastructure.Services
{
	public class CurrentUserService : ICurrentUserService
	{
		private readonly IHttpContextAccessor _httpContextAccessor;

		public CurrentUserService(IHttpContextAccessor httpContextAccessor)
		{
			_httpContextAccessor = httpContextAccessor;
		}

		public string UserId
		{
			get
			{
				var httpContext = _httpContextAccessor.HttpContext;

				Console.WriteLine(httpContext == null);

				Console.WriteLine(httpContext?.User.Identity?.IsAuthenticated);

				foreach (var claim in httpContext?.User.Claims ?? Enumerable.Empty<Claim>())
				{
					Console.WriteLine($"{claim.Type} = {claim.Value}");
				}
				var userIdClaim = _httpContextAccessor.HttpContext?.User?
					.FindFirstValue(ClaimTypes.NameIdentifier);

				if (string.IsNullOrEmpty(userIdClaim))
					throw new UnauthorizedAccessException("User chưa đăng nhập.");

				return userIdClaim;
			}
		}
	}
}
