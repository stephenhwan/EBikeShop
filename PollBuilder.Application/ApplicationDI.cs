using MediatR;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using PollBuilder.Application.Behaviors;

namespace PollBuilder.Application
{
	public static class ApplicationDI
	{
		public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)	
		{
		services.AddMediatR(cfg =>
			{
				cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
			});

			services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

			services.AddTransient(
				typeof(IPipelineBehavior<,>),
				typeof(ValidationBehavior<,>)
			);
			return services;
		}
	}
}
