using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EBikeShop.MVC.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitializeDbContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bikes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: true),
                    Year = table.Column<int>(type: "int", nullable: false),
                    BrandName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Category = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bikes", x => x.Id);
                    table.CheckConstraint("CK_Bike_Year", "[Year] >= 1900 AND [Year] <= 9999");
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bikes");
        }
    }
}
