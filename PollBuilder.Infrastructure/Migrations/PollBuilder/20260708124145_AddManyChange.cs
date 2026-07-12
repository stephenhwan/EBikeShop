using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PollBuilder.Infrastructure.Migrations.PollBuilder
{
    /// <inheritdoc />
    public partial class AddManyChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Polls_Pollid",
                table: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_Votes_UserId_QuestionId",
                table: "Votes");

            migrationBuilder.RenameColumn(
                name: "Pollid",
                table: "Questions",
                newName: "PollId");

            migrationBuilder.RenameColumn(
                name: "Text",
                table: "Questions",
                newName: "QuestionText");

            migrationBuilder.RenameIndex(
                name: "IX_Questions_Pollid_Position",
                table: "Questions",
                newName: "IX_Questions_PollId_Position");

            migrationBuilder.AddColumn<bool>(
                name: "IsCurrent",
                table: "Votes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsClosed",
                table: "Polls",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Votes_UserId_QuestionId",
                table: "Votes",
                columns: new[] { "UserId", "QuestionId" },
                unique: true,
                filter: "[IsCurrent] = 1");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Polls_PollId",
                table: "Questions",
                column: "PollId",
                principalTable: "Polls",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Polls_PollId",
                table: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_Votes_UserId_QuestionId",
                table: "Votes");

            migrationBuilder.DropColumn(
                name: "IsCurrent",
                table: "Votes");

            migrationBuilder.DropColumn(
                name: "IsClosed",
                table: "Polls");

            migrationBuilder.RenameColumn(
                name: "PollId",
                table: "Questions",
                newName: "Pollid");

            migrationBuilder.RenameColumn(
                name: "QuestionText",
                table: "Questions",
                newName: "Text");

            migrationBuilder.RenameIndex(
                name: "IX_Questions_PollId_Position",
                table: "Questions",
                newName: "IX_Questions_Pollid_Position");

            migrationBuilder.CreateIndex(
                name: "IX_Votes_UserId_QuestionId",
                table: "Votes",
                columns: new[] { "UserId", "QuestionId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Polls_Pollid",
                table: "Questions",
                column: "Pollid",
                principalTable: "Polls",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
