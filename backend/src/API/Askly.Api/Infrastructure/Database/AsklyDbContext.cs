using Askly.Api.Entities;
using Askly.Api.Infrastructure.Database.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Askly.Api.Infrastructure.Database;

public sealed class AsklyDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Question> Questions { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("vector");
        modelBuilder.ApplyConfiguration(new QuestionConfiguration());
    }
}
