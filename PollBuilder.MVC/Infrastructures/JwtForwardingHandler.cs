using Microsoft.AspNetCore.Authentication;
using System.Net.Http.Headers;
namespace PollBuilder.MVC.Infrastructures
{
	public class JwtForwardingHandler : DelegatingHandler
	{
		private readonly IHttpContextAccessor _httpContextAccessor;

		public JwtForwardingHandler(IHttpContextAccessor httpContextAccessor)
		{
			_httpContextAccessor = httpContextAccessor;
		}

		protected override async Task<HttpResponseMessage> SendAsync(
			HttpRequestMessage request, CancellationToken cancellationToken)
		{
			var httpContext = _httpContextAccessor.HttpContext;

			if (httpContext != null)
			{
				var token = await httpContext.GetTokenAsync("access_token");

				// DEBUG TẠM - xóa sau khi tìm ra lỗi
				Console.WriteLine($"[JwtForwardingHandler] IsAuthenticated: {httpContext.User?.Identity?.IsAuthenticated}");
				Console.WriteLine($"[JwtForwardingHandler] token: {(string.IsNullOrEmpty(token) ? "NULL/EMPTY" : token)}");

				if (!string.IsNullOrEmpty(token))
				{
					request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
					Console.WriteLine(request.Headers.Authorization);
				}
			}

			return await base.SendAsync(request, cancellationToken);
		}
	}
}