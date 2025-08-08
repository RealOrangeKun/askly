using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Askly.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueIndexToQuestionText : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "ix_questions_question_text",
                table: "questions",
                column: "question_text",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_questions_question_text",
                table: "questions");
        }
    }
}
