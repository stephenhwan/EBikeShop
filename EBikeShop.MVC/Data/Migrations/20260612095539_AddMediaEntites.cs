using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EBikeShop.MVC.Data.Migrations
{
	/// <inheritdoc />
	public partial class AddMediaEntites : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "Medias",
				columns: table => new
				{
					Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
					StoredFileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
					FilePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
					FileType = table.Column<string>(type: "nvarchar(max)", nullable: true),
					FileSize = table.Column<long>(type: "bigint", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Medias", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "BikeMedias",
				columns: table => new
				{
					Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					BikeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					MediaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_BikeMedias", x => x.Id);
					table.ForeignKey(
						name: "FK_BikeMedias_Bikes_BikeId",
						column: x => x.BikeId,
						principalTable: "Bikes",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
					table.ForeignKey(
						name: "FK_BikeMedias_Medias_MediaId",
						column: x => x.MediaId,
						principalTable: "Medias",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateIndex(
				name: "IX_BikeMedias_BikeId",
				table: "BikeMedias",
				column: "BikeId");

			migrationBuilder.CreateIndex(
				name: "IX_BikeMedias_MediaId",
				table: "BikeMedias",
				column: "MediaId");
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(name: "BikeMedias");
			migrationBuilder.DropTable(name: "Medias");
		}
	}
}