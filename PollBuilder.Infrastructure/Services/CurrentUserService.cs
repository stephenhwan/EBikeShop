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

		public Guid UserId
		{
			get
			{
				var userIdClaim = _httpContextAccessor.HttpContext?.User?
					.FindFirstValue(ClaimTypes.NameIdentifier);

				if (string.IsNullOrEmpty(userIdClaim))
					throw new UnauthorizedAccessException("User chưa đăng nhập.");

				return Guid.Parse(userIdClaim);
			}
		}
	}
}
