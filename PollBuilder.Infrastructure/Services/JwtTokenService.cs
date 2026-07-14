using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PollBuilder.Domain.Entities.Identity;

namespace PollBuilder.Infrastructure.Services
{
	public class JwtTokenService
	{
		private readonly IConfiguration _config;
		// Nhận object IConfiguration chứa dữ liệu của appsettings.json
		public JwtTokenService(IConfiguration config) => _config = config;

		public string GenerateToken(User user)
		{
			var claims = new List<Claim>
			{
				new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
				new Claim(ClaimTypes.Email, user.Email)
			};

			var key = new SymmetricSecurityKey(
				Encoding.UTF8.GetBytes(_config["Jwt:Key"]));

			var token = new JwtSecurityToken(
				issuer: _config["Jwt:Issuer"],
				audience: _config["Jwt:Audience"],
				claims: claims,
				expires: DateTime.UtcNow.AddHours(2),
				signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

			return new JwtSecurityTokenHandler().WriteToken(token);
		}
	}
}
