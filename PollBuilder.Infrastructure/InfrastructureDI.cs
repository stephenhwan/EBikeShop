
using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PollBuilder.Application.Behaviors;
using PollBuilder.Application.Interfaces;
using PollBuilder.Domain.Entities.Identity;
using PollBuilder.Infrastructure.DbContexts;
using PollBuilder.Infrastructure.Services;

namespace PollBuilder.Infrastructure
{
	public static class InfrastructureDI
	{
		public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddDbContext<PollBuilderDbContext>(options =>
				options.UseSqlServer(configuration.GetConnectionString("PollBuilderConnection")));
			services.AddScoped<IPollBuilderDbContext>(provider =>
				provider.GetRequiredService<PollBuilderDbContext>());

			services.AddScoped<IIdentityService, IdentityService>();
			// THÊM DÒNG NÀY — context Identity phải được đăng ký riêng
			services.AddDbContext<PollBuilderIdentityDbContext>(options =>
				options.UseSqlServer(configuration.GetConnectionString("PollBuilderIdentityConnection")));

			services.AddHttpContextAccessor();
			services.AddScoped<ICurrentUserService, CurrentUserService>();
			services.AddScoped<IJwtTokenService, JwtTokenService>();


			services.AddIdentity<User, UserRole>(options =>
			{
				options.Password.RequiredLength = 6;
				options.Password.RequireNonAlphanumeric = false;
				options.Password.RequireUppercase = false;
			})
			.AddEntityFrameworkStores<PollBuilderIdentityDbContext>()
			.AddDefaultTokenProviders();


			return services;
		}
		
	}
}
