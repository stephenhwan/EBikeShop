using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EBikeShop.MVC.Data.Migrations
{
    /// <inheritdoc />
    public partial class NoNeedToKnow : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category222",
                table: "Bikes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Category222",
                table: "Bikes",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "");
        }
    }
}
