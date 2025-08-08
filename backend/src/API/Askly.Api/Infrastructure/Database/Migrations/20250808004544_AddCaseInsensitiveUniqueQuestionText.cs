using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Askly.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddCaseInsensitiveUniqueQuestionText : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_questions_question_text",
                table: "questions");

            // Create case-insensitive unique index using LOWER function
            migrationBuilder.Sql(
                "CREATE UNIQUE INDEX ix_questions_question_text_lower_unique ON questions (LOWER(question_text))");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                "DROP INDEX IF EXISTS ix_questions_question_text_lower_unique");

            migrationBuilder.CreateIndex(
                name: "ix_questions_question_text",
                table: "questions",
                column: "question_text",
                unique: true);
        }
    }
}
