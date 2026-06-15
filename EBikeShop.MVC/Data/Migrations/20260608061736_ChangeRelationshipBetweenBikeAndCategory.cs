using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EBikeShop.MVC.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangeRelationshipBetweenBikeAndCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CategoryId",
                table: "Bikes",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bikes_CategoryId",
                table: "Bikes",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bikes_Categories_CategoryId",
                table: "Bikes",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bikes_Categories_CategoryId",
                table: "Bikes");

            migrationBuilder.DropIndex(
                name: "IX_Bikes_CategoryId",
                table: "Bikes");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Bikes");
        }
    }
}
