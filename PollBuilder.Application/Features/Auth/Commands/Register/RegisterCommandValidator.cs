using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;

namespace PollBuilder.Application.Features.Auth.Commands.Register
{
	public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
	{
		public RegisterCommandValidator()
		{
			RuleFor(x => x.FullName)
				.NotEmpty().WithMessage("Họ tên không được để trống.")
				.MaximumLength(100);

			RuleFor(x => x.Email)
				.NotEmpty().WithMessage("Email không được để trống.")
				.EmailAddress().WithMessage("Email không đúng định dạng.");

			RuleFor(x => x.Password)
				.NotEmpty().WithMessage("Mật khẩu không được để trống.")
				.MinimumLength(6).WithMessage("Mật khẩu tối thiểu 6 ký tự.");

			RuleFor(x => x.ConfirmPassword)
				.Equal(x => x.Password)
				.WithMessage("Mật khẩu xác nhận không khớp.");
		}
	}
	
}
