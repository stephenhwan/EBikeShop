using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace PollBuilder.Application.Features.Auth.Commands.Login
{
	public record LoginCommand(
	string Email,
	string Password
	) : IRequest<string>;
}
