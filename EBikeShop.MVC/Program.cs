using EBikeShop.MVC.Data;
using EBikeShop.MVC.Data.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);
// khai bao EBikeShopDbContext
builder.Services.AddDbContext<EBikeShopDbContext>(options =>
	options.UseSqlServer(
		builder.Configuration.GetConnectionString("EBikeShopConnection")
	)
);
// Khai bao BikeIdentityDbContext
builder.Services.AddDbContext<BikeIdentityDbContext>(options =>

	options.UseSqlServer(
		builder.Configuration.GetConnectionString("EBikeShopIdentityConnection")
	)
);


builder.Services.AddDefaultIdentity<BikeIdentityUser>(options =>
{
	options.SignIn.RequireConfirmedAccount = false;
	// Password settings
	options.Password.RequireDigit = true;
	options.Password.RequiredLength = 8;
	options.Password.RequireNonAlphanumeric = false;
	options.Password.RequireUppercase = true;
	options.Password.RequireLowercase = true;
	// Lockout settings
	options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromHours(10);
	options.Lockout.MaxFailedAccessAttempts = 5;
	options.Lockout.AllowedForNewUsers = true;
	// User settings
	options.User.RequireUniqueEmail = true;
	// Sign - in settings
	options.SignIn.RequireConfirmedEmail = false;
	options.SignIn.RequireConfirmedPhoneNumber = false;
}
)
	.AddEntityFrameworkStores<BikeIdentityDbContext>()
	.AddDefaultTokenProviders();
// Add cookies 
builder.Services.ConfigureApplicationCookie(options =>
{
	options.LoginPath = "/Authentication/Login";
	options.AccessDeniedPath = "/Identity/Account/AccessDenied";
});

// Add confige session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
	options.IdleTimeout = TimeSpan.FromMinutes(60);
	options.Cookie.HttpOnly = true;
	options.Cookie.IsEssential = true;
});
//app role 
builder.Services.AddIdentity<BikeIdentityRole, IdentityRole>()
	.AddEntityFrameworkStores<BikeIdentityDbContext>()
	.AddDefaultTokenProviders();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();
builder.Services.AddRazorPages();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseDeveloperExceptionPage();
}
else
{
	app.UseExceptionHandler("/Home/Error");
	app.UseHsts();
}

//seeding role to database
using (var scope = app.Services.CreateScope())
{
	var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

	string[] roles = { "Admin", "Manager", "Customer" };
	foreach (var role in roles)
	{
		if (!await roleManager.RoleExistsAsync(role))
		{
			await roleManager.CreateAsync(new IdentityRole(role));
		}
	}
}


app.UseHttpsRedirection();

app.UseRouting();

app.MapStaticAssets();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	//pattern: "{controller=Bikes}/{action=Create}/{id?}")
	pattern: "{controller=Home}/{action=Index}/{id?}")
	.WithStaticAssets();
app.MapRazorPages();

app.Run();