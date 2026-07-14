
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using PollBuilder.Application.Interfaces;
using PollBuilder.Domain.Entities.Identity;

namespace PollBuilder.Application.Features.Auth.Commands.Login
{
	public class LoginCommandHandler : IRequestHandler<LoginCommand, string>
	{
		private readonly ICurrentUserService _currentUserService;
		private readonly IPollBuilderDbContext _context;
		private readonly SignInManager<User> _signInManager;
		private readonly UserManager<User> _userManager;
		private readonly IJwtTokenService _jwtTokenService;
		private readonly ILogger<LoginCommandHandler> _logger;

		// Tiêm (Inject) IPollBuilderDbContext vào để tương tác với Database
		public LoginCommandHandler(IPollBuilderDbContext context,
			ICurrentUserService currentUserService,
			SignInManager<User> signInManager,
			IJwtTokenService jwtTokenService,
			UserManager<User> userManager,
			ILogger<LoginCommandHandler> logger)
		{
			_currentUserService = currentUserService;
			_context = context;
			_signInManager = signInManager;
			_userManager = userManager;
			_logger = logger;
			_jwtTokenService = jwtTokenService;
		}


		public async Task<string> Handle(LoginCommand request, CancellationToken cancellationToken)
		{
			var user = await _userManager.FindByEmailAsync(request.Email);

			if (user == null)
			{
				_logger.LogWarning("Đăng nhập thất bại: không tìm thấy email {Email}", request.Email);
				throw new UnauthorizedAccessException("Email hoặc mật khẩu không đúng.");
			}

			var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: false);

			if (!result.Succeeded)
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
