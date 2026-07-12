using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PollBuilder.Application.Interfaces;
using PollBuilder.Infrastructure.DbContexts;

namespace PollBuilder.Infrastructure
{
	public static class InfrastructureDI
	{
		public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
		{
			// Cấu hình EF Core ở ĐÂY, dự án API sẽ không cần biết chi tiết này
			services.AddDbContext<PollBuilderDbContext>(options =>
				options.UseSqlServer(configuration.GetConnectionString("PollBuilderConnection")));
			services.AddScoped<IPollBuilderDbContext>(provider =>
				provider.GetRequiredService<PollBuilderDbContext>());


			return services;
		}
	}
}
