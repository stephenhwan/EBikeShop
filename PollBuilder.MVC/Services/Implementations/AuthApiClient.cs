
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using PollBuilder.MVC.Contracts.Requests;
using PollBuilder.MVC.Contracts.Responses;
using PollBuilder.MVC.Services.Interfaces;

namespace PollBuilder.MVC.Services.Implementations
{
	public class AuthApiClient : IAuthApiClient
	{
		private readonly HttpClient _httpClient;
		public AuthApiClient(HttpClient httpClient) => _httpClient = httpClient;

		public async Task<AuthResponseDto?> LoginAsync(string email, string password)
		{
			var request = new LoginRequestDto { Email = email, Password = password };
			var response = await _httpClient.PostAsJsonAsync("api/authentication/login", request);

			if (!response.IsSuccessStatusCode) return null;

			return await response.Content.ReadFromJsonAsync<AuthResponseDto>();
		}
		public async Task<AuthResponseDto?> RegisterAsync(string email, string password, string confirmPassword, string? fullName)
		{
			var request = new RegisterRequestDto { Email = email, Password = password, ConfirmPassword = confirmPassword,FullName = fullName };
			var response = await _httpClient.PostAsJsonAsync("api/authentication/register", request);

			if (!response.IsSuccessStatusCode) return null;
			return await response.Content.ReadFromJsonAsync<AuthResponseDto>();
		}

	}
}
