
using Microsoft.AspNetCore.Identity;
using PollBuilder.Application.Interfaces;
using PollBuilder.Domain.Entities.Identity;

namespace PollBuilder.Infrastructure.Services
{
	public class IdentityService : IIdentityService
	{
		private readonly UserManager<User> _userManager;
		private readonly SignInManager<User> _signInManager;
		public IdentityService(
			UserManager<User> userManager,
			SignInManager<User> signInManager)
		{
			_userManager = userManager;
			_signInManager = signInManager;
		}

		public async Task<User?> FindByEmailAsync(string email)
		{
			return await _userManager.FindByEmailAsync(email);
		}

		public async Task<bool> CheckPasswordSignInAsync(User user, string password)
		{
			var result = await _signInManager.CheckPasswordSignInAsync(
				user,
				password,
				false);

			return result.Succeeded;
		}
		public async Task<IdentityResult> CreateAsync(User user, string password)
		{
			return await _userManager.CreateAsync(user, password);
		}

	}

}