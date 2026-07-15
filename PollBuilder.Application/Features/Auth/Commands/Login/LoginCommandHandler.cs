
using MediatR;

using Microsoft.Extensions.Logging;
using PollBuilder.Application.Interfaces;

namespace PollBuilder.Application.Features.Auth.Commands.Login
{
	public class LoginCommandHandler : IRequestHandler<LoginCommand, string>
	{

		private readonly IIdentityService _identityService;
		private readonly IJwtTokenService _jwtTokenService;
		private readonly ILogger<LoginCommandHandler> _logger;

		// Tiêm (Inject) IPollBuilderDbContext vào để tương tác với Database
		public LoginCommandHandler(

			IJwtTokenService jwtTokenService,
			IIdentityService identityService,
			ILogger<LoginCommandHandler> logger)
		{

			_identityService = identityService;
			_logger = logger;
			_jwtTokenService = jwtTokenService;
		}


		public async Task<string> Handle(LoginCommand request, CancellationToken cancellationToken)
		{
			var user = await _identityService.FindByEmailAsync(request.Email);

			if (user == null)
			{
				_logger.LogWarning("Đăng nhập thất bại: không tìm thấy email {Email}", request.Email);
				throw new UnauthorizedAccessException("Email hoặc mật khẩu không đúng.");
			}

			var result = await _identityService.CheckPasswordSignInAsync(user, request.Password);

			if (!result)
			{
				_logger.LogWarning("Đăng nhập thất bại: sai mật khẩu cho email {Email}", request.Email);
				throw new UnauthorizedAccessException("Email hoặc mật khẩu không đúng.");
			}

			var token = _jwtTokenService.GenerateToken(user);   // ⭐ gọi đúng tên

			_logger.LogInformation("User {Email} đăng nhập thành công", request.Email);

			return token;
		}
	}

}
