using System.Text;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using PollBuilder.Application.Features.Polls.Commands.CreatePoll;
using PollBuilder.Infrastructure;
using Scalar.AspNetCore;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddInfrastructureServices(builder.Configuration);

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddMediatR(cfg =>
	cfg.RegisterServicesFromAssembly(typeof(CreatePollCommand).Assembly));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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
	});

builder.Services.AddAuthorization();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.MapOpenApi();
	app.MapScalarApiReference(options => {
		options.WithTitle("Hệ thống API Của Tôi")
		.WithTheme(ScalarTheme.Purple);
	});
}

app.UseHttpsRedirection();
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
