using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using PollBuilder.Application.Interfaces;
using PollBuilder.Domain.Entities.Identity;

namespace PollBuilder.Application.Features.Auth.Commands.Register
{
	public class RegisterCommandHandler : IRequestHandler<RegisterCommand, string>
	{
		private readonly UserManager<User> _userManager;
		private readonly IJwtTokenService _jwtTokenService;
		private readonly ILogger<RegisterCommandHandler> _logger;

		public RegisterCommandHandler(
			UserManager<User> userManager,
			IJwtTokenService jwtTokenService,
			ILogger<RegisterCommandHandler> logger)
		{
			_userManager = userManager;
			_jwtTokenService = jwtTokenService;
			_logger = logger;
		}

		public async Task<string> Handle(RegisterCommand request, CancellationToken cancellationToken)
		{
			// Bước 1: Check 2 mật khẩu có khớp không
			if (request.Password != request.ConfirmPassword)
			{
				throw new InvalidOperationException("Mật khẩu xác nhận không khớp.");
			}

			// Bước 2: Check email đã tồn tại chưa
			var existingUser = await _userManager.FindByEmailAsync(request.Email);
			if (existingUser != null)
			{
				throw new InvalidOperationException("Email này đã được đăng ký.");
			}

			// Bước 3: Tạo user mới
			var user = new User
			{
				Email = request.Email,
				UserName = request.FullName,   // Identity yêu cầu UserName, dùng luôn Email cho đơn giản

			};

			var result = await _userManager.CreateAsync(user, request.Password);

			if (!result.Succeeded)
			{
				// Gom tất cả lỗi lại thành 1 chuỗi để dễ đọc (VD: mật khẩu quá yếu, ký tự không hợp lệ...)
				var errors = string.Join("; ", result.Errors.Select(e => e.Description));
				_logger.LogWarning("Đăng ký thất bại cho email {Email}: {Errors}", request.Email, errors);
				throw new InvalidOperationException(errors);
			}

			_logger.LogInformation("User {Email} đăng ký thành công", request.Email);

			// Bước 4: Đăng ký xong thì tự động tạo token luôn (cho user login ngay, không cần login lại)
			var token = _jwtTokenService.GenerateToken(user);

			return token;
		}
	}
}
