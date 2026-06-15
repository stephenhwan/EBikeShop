using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EBikeShop.MVC.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangeCateNameTemporarily : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Category",
                table: "Bikes",
                newName: "Category222");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Category222",
                table: "Bikes",
                newName: "Category");
        }
    }
}
