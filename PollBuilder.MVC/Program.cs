using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PollBuilder.Application.Interfaces;
using PollBuilder.Domain.Entities.Identity;
using PollBuilder.Infrastructure.DbContexts;
var builder = WebApplication.CreateBuilder(args);



//for connection string 
builder.Services.AddDbContext<PollBuilderDbContext>(option =>
{
	option.UseSqlServer(builder.Configuration.GetConnectionString("PollBuilderConnection"));
});
builder.Services.AddDbContext<PollBuilderIdentityDbContext>(option =>
{
	option.UseSqlServer(builder.Configuration.GetConnectionString("PollBuilderidentityConnection"));
});



builder.Services.AddIdentity<User, UserRole>(options =>
{
	//options.SignIn.RequireConfirmedAccount = true;
	// Password settings
	options.Password.RequireDigit = true;
	options.Password.RequiredLength = 8;
	options.Password.RequireNonAlphanumeric = true;
	options.Password.RequireUppercase = true;
	options.Password.RequireLowercase = true;
	// Lockout settings
	options.Lockout.MaxFailedAccessAttempts = 5;
	options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromHours(10);
	options.Lockout.AllowedForNewUsers = true;
	// User settings
	options.User.RequireUniqueEmail = true;
	// Sign-in settings
	options.SignIn.RequireConfirmedEmail = true;
	options.SignIn.RequireConfirmedAccount = true;
	options.SignIn.RequireConfirmedPhoneNumber = false;
}
)
	.AddEntityFrameworkStores<PollBuilderIdentityDbContext>()
	.AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
	options.LoginPath = "/Authentication/Login";
	options.AccessDeniedPath = "/Identity/Account/AccessDenied";
});

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
	options.IdleTimeout = TimeSpan.FromMinutes(60);
	options.Cookie.HttpOnly = true;
	options.Cookie.IsEssential = true;
});

// Add services to the container.
builder.Services.AddControllersWithViews();






var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}")
	.WithStaticAssets();


app.Run();
