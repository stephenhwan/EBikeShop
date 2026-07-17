using Microsoft.AspNetCore.Authentication.Cookies;
using PollBuilder.MVC.Infrastructures;
using PollBuilder.MVC.Services.Implementations;
using PollBuilder.MVC.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<JwtForwardingHandler>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
	.AddCookie(options =>
	{
		options.LoginPath = "/account/login";
		options.AccessDeniedPath = "/account/accessdenied";
	});

builder.Services.AddHttpClient<IPollApiClient, PollApiClient>(client =>
{
	client.BaseAddress = new Uri(builder.Configuration["ApiSettings:BaseUrl"]!);
}).AddHttpMessageHandler<JwtForwardingHandler>();

builder.Services.AddHttpClient<IAuthApiClient, AuthApiClient>(client =>
{
	client.BaseAddress = new Uri(builder.Configuration["ApiSettings:BaseUrl"]!);
}).AddHttpMessageHandler<JwtForwardingHandler>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=home}/{action=index}/{id?}");

app.Run();