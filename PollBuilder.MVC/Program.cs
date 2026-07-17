using Microsoft.AspNetCore.Authentication.Cookies;
using PollBuilder.MVC.Infrastructures;
using PollBuilder.MVC.Services.Implementations;
using PollBuilder.MVC.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<JwtForwardingHandler>();
builder.Services.AddScoped<IQRCodeService, QRCodeService>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
	.AddCookie(options =>
	{
		options.LoginPath = "/account/login";
		options.AccessDeniedPath = "/account/accessdenied";

		// Nếu request là AJAX/fetch (Content-Type: application/json hoặc header X-Requested-With),
		// trả 401 thay vì redirect 302 tới trang login — vì fetch() sẽ tự follow redirect
		// và nuốt mất luôn HTML của trang login, khiến JS không bao giờ biết là bị văng ra do chưa đăng nhập.
		options.Events.OnRedirectToLogin = context =>
		{
			if (context.Request.Path.StartsWithSegments("/poll") ||
				context.Request.Headers["X-Requested-With"] == "XMLHttpRequest" ||
				context.Request.Headers.Accept.ToString().Contains("application/json"))
			{
				context.Response.StatusCode = StatusCodes.Status401Unauthorized;
				return Task.CompletedTask;
			}

			context.Response.Redirect(context.RedirectUri);
			return Task.CompletedTask;
		};
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