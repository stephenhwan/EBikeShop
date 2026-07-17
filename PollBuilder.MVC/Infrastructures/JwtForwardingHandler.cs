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
				if (!string.IsNullOrEmpty(token))
				{
					request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
				}
			}

			return await base.SendAsync(request, cancellationToken);
		}
	}
}
