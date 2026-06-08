using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EBikeShop.MVC.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddImageNameToBike : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageName",
                table: "Bikes",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageName",
                table: "Bikes");
        }
    }
}
