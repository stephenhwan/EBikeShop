using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PollBuilder.Application.Features.Auth.Commands.Login;
using PollBuilder.Application.Features.Auth.Commands.Register;
using PollBuilder.Domain.Entities.Identity;
using PollBuilder.Infrastructure.Services;

namespace PollBuilder.API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class AuthenticationController : ControllerBase
	{
		private readonly IMediator _mediator;

		public AuthenticationController(IMediator mediator) 
		{ 
			_mediator = mediator; 
		}

		[HttpPost("login")]
		public async Task<IActionResult> Login(LoginCommand request)
		{
			var token = await _mediator.Send(request);
			return Ok(new { token });
		}

		[HttpPost("register")]
		public async Task<IActionResult> Register(RegisterCommand request)
		{
			var token = await _mediator.Send(request);
			return Ok(new { token });
		}
	}
	
}
