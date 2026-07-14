using MediatR;

namespace PollBuilder.Application.Features.Auth.Commands.Register
{
	public record RegisterCommand(
		string Email,
		string Password,
		string ConfirmPassword,
		string FullName
	) : IRequest<string>;

}
