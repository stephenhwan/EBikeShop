using EBikeShop.MVC.Data;
using EBikeShop.MVC.Data.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<EBikeShopDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("EBikeShopConnection")
    )
);

builder.Services.AddDbContext<BikeIdentityDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("EBikeShopIdentityConnection")
        )
    );


builder.Services.AddDefaultIdentity<BikeIdentityUser>(options =>
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
    .AddEntityFrameworkStores<BikeIdentityDbContext>()
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

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();
builder.Services.AddRazorPages();

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

app.UseAuthentication();
app.UseAuthorization();


app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    //pattern: "{controller=Bikes}/{action=Create}/{id?}")
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages();

app.Run();
