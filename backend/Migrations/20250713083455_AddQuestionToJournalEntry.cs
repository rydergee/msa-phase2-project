using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MockMate.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddQuestionToJournalEntry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Question",
                table: "JournalEntries",
                type: "TEXT",
                maxLength: 500,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Question",
                table: "JournalEntries");
        }
    }
}
