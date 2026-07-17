using PollBuilder.MVC.Contracts.Requests;
using PollBuilder.MVC.Contracts.Responses;

namespace PollBuilder.MVC.Services.Interfaces
{
	public interface IAuthApiClient
	{
		Task<AuthResponseDto?> LoginAsync(string email, string password);
		Task<AuthResponseDto?> RegisterAsync(string email, string password, string confirmPassword, string? fullName);

	}
}
