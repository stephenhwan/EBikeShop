
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PollBuilder.Application.Interfaces;
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
				services.AddHttpContextAccessor();
				services.AddScoped<ICurrentUserService, CurrentUserService>();


			return services;
		}
		
	}
}
