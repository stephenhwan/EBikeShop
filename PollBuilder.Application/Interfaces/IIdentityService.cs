using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using PollBuilder.Application.DTOs;
using PollBuilder.Domain.Entities.Identity;

namespace PollBuilder.Application.Interfaces
{
	public interface IIdentityService
	{
		Task<User?> FindByEmailAsync(string email);

		Task<bool> CheckPasswordSignInAsync(User user, string password);
		Task<IdentityResult> CreateAsync(User user, string password);
	}
}
