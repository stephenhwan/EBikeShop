
using EBikeShop.MVC.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace EBikeShop.MVC.Data
{
    public class EBikeShopDbContext: DbContext
    {
        //=== Step 1: Constructor ===//
        public EBikeShopDbContext(DbContextOptions<EBikeShopDbContext> options)
        : base(options)
        {
            //=== Không cần khai báo gì thêm ===//
        }

        //=== Step 2: Khai báo DbSet ===//
        public DbSet<Bike> Bikes { get; set; }
        public DbSet<Category> Categories { get; set; }
		//=== Step 3: Khai báo OnConfiguring (nếu cần) ===//

		//=== Step 4: Khai báo OnModelCreating ===//
		protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ===== Student =====
            modelBuilder.Entity<Bike>()
                .ToTable(t =>
                {
                    t.HasCheckConstraint(
                        "CK_Bike_Year",
                        "[Year] >= 1900 AND [Year] <= 9999"
                    );
                });
            modelBuilder.Entity<Bike>(entity =>
            {
                //TODO: Xem lại ánh xạ các tables
                //entity.ToTable("Bikes");
                //entity.HasKey(e => e.Id);
                entity.Property(e => e.Name)
                      .IsRequired()
                      .HasMaxLength(150);
                entity.Property(e => e.Description)
                      .HasMaxLength(5000);
                entity.Property(e => e.BrandName)
                      .IsRequired()
                      .HasMaxLength(150);
                entity.Property(e => e.Category)
                      .IsRequired()
                      .HasMaxLength(150);

            });

        }



    }
}
