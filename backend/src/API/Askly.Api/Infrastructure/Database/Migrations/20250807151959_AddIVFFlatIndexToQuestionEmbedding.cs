using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Askly.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddIVFFlatIndexToQuestionEmbedding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "question_text",
                table: "questions",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "ix_questions_question_embedding",
                table: "questions",
                column: "question_embedding")
                .Annotation("Npgsql:IndexMethod", "ivfflat")
                .Annotation("Npgsql:IndexOperators", new[] { "vector_cosine_ops" })
                .Annotation("Npgsql:StorageParameter:lists", 1);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_questions_question_embedding",
                table: "questions");

            migrationBuilder.DropColumn(
                name: "question_text",
                table: "questions");
        }
    }
}
