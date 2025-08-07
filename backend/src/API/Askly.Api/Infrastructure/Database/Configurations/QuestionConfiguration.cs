using Askly.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Askly.Api.Infrastructure.Database.Configurations;

public class QuestionConfiguration : IEntityTypeConfiguration<Question>
{
    public void Configure(EntityTypeBuilder<Question> builder)
    {
        builder.HasKey(q => q.Id);

        builder.Property(q => q.QuestionEmbedding)
            .HasColumnType("vector(384)")
            .IsRequired();

        builder.Property(q => q.Status)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(q => q.AnswerText)
            .HasMaxLength(5000)
            .IsRequired();

        builder.Property(q => q.CreatedAt)
            .IsRequired();

        builder.Property(q => q.UpdatedAt)
            .IsRequired();
    }
}
