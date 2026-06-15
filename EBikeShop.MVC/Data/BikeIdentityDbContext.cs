using EBikeShop.MVC.Configs;
using EBikeShop.MVC.Data.Entities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EBikeShop.MVC.Data
{
	public class BikeIdentityDbContext : IdentityDbContext
	{
		public BikeIdentityDbContext(DbContextOptions<BikeIdentityDbContext>
		options)
		 : base(options)
		{ }
		//=== Khai bao DbSet ===//
		public DbSet<BikeIdentityUser> BikeIdentityUsers { get; set; }
		public DbSet<BikeIdentityRole> BikeIdentityRoles { get; set; }
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			modelBuilder.Entity<BikeIdentityUser>()
			.Property(p => p.FullName)
			.HasMaxLength(MaxLengths.FullName);
			modelBuilder.Entity<BikeIdentityUser>()
			.Property(p => p.Avatar)
			.HasMaxLength(MaxLengths.FileName);
			modelBuilder.Entity<BikeIdentityRole>()
			.Property(p => p.Description)
			.HasMaxLength(MaxLengths.Description);
		}
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			if (!optionsBuilder.IsConfigured)
			{
				var connectionString = "Server=DESKTOP-HQP0AN8\\MSSQLSERVER01;Database=EBikeShopIdentityDbContext;User Id=sa;Password=Uyen311003@;TrustServerCertificate=True;Encrypt=True;MultipleActiveResultSets=True;";
				optionsBuilder.UseSqlServer(connectionString);
			}
		}
	}
}
