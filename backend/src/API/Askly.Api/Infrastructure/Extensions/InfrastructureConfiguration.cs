using Askly.Api.Infrastructure.Database;
using Askly.Api.Infrastructure.Database.Repositories;
using Askly.Api.Infrastructure.Database.TypeHandlers;
using Askly.Api.Infrastructure.Services;
using Askly.Api.Shared.Database;
using Askly.Api.Shared.Services;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Npgsql;

namespace Askly.Api.Infrastructure.Extensions;

public static class InfrastructureConfiguration
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        string databaseConnectionString = configuration.GetConnectionStringOrThrow("Database");

        services.AddDbContext<AsklyDbContext>(options =>
            options.UseNpgsql(databaseConnectionString, o => o.UseVector())
                   .UseSnakeCaseNamingConvention()
                   .EnableSensitiveDataLogging(false)
                   .EnableServiceProviderCaching()
                   .EnableDetailedErrors(false));

        services.AddNpgsqlDataSource(databaseConnectionString, builder =>
        {
            builder.UseVector();
            builder.EnableParameterLogging(false);
        });

        services.AddCachingInternal(configuration);



        services.TryAddScoped<IDbConnectionFactory, DbConnectionFactory>();

        services.TryAddScoped<IQuestionRepository, QuestionRepository>();

        services.TryAddScoped<IEmbeddingService, EmbeddingService>();

        SqlMapper.AddTypeHandler(new VectorTypeHandler());

        services.AddHealthChecks()
            .AddNpgSql(databaseConnectionString, name: "database", tags: ["ready"]);

        return services;
    }
}
