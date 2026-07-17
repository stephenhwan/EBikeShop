using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using PollBuilder.API;
using PollBuilder.Application.Features.Polls.Commands.CreatePoll;
using PollBuilder.Infrastructure;
using Scalar.AspNetCore;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddInfrastructureServices(builder.Configuration);

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

builder.Services.AddMediatR(cfg =>
	cfg.RegisterServicesFromAssembly(typeof(CreatePollCommand).Assembly));
builder.Services
	.AddAuthentication(options =>
	{
		options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
		options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
		options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
	})
	.AddJwtBearer(options =>
	{
		options.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuer = true,
			ValidIssuer = builder.Configuration["Jwt:Issuer"],

			ValidateAudience = true,
			ValidAudience = builder.Configuration["Jwt:Audience"],

			ValidateLifetime = true,
			ValidateIssuerSigningKey = true,

			IssuerSigningKey = new SymmetricSecurityKey(
				Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
		};

		// DEBUG TẠM - xóa sau khi tìm ra lỗi
		options.Events = new JwtBearerEvents
		{
			OnMessageReceived = context =>
			{
				Console.WriteLine("========== API ==========");
				Console.WriteLine("Authorization Header:");
				Console.WriteLine(context.Request.Headers.Authorization.ToString());
				Console.WriteLine("=========================");

				return Task.CompletedTask;
			},

			OnTokenValidated = context =>
			{
				Console.WriteLine("TOKEN VALIDATED");

				foreach (var claim in context.Principal!.Claims)
				{
					Console.WriteLine($"{claim.Type} = {claim.Value}");
				}

				return Task.CompletedTask;
			},

			OnAuthenticationFailed = context =>
			{
				Console.WriteLine($"FAILED: {context.Exception}");
				return Task.CompletedTask;
			},

			OnChallenge = context =>
			{
				Console.WriteLine($"CHALLENGE: {context.Error}");
				return Task.CompletedTask;
			}
		};
	});

builder.Services.AddAuthorization();
IdentityModelEventSource.ShowPII = true;
var app = builder.Build();


app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseHttpsRedirection();
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
