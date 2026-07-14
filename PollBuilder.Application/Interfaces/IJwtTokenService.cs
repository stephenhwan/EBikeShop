using PollBuilder.Application.DTOs;
using PollBuilder.Domain.Entities;
using PollBuilder.Domain.Entities.Identity;

namespace PollBuilder.Application.Interfaces
{
	public interface IJwtTokenService
	{
		string GenerateToken(User user);

	}
}
