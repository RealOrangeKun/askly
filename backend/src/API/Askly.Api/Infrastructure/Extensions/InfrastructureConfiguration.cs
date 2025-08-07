using Askly.Api.Infrastructure.Database;
using Askly.Api.Shared.Database;
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
                   .UseSnakeCaseNamingConvention());

        NpgsqlDataSource npgsqlDataSource = new NpgsqlDataSourceBuilder(databaseConnectionString).Build();
        services.TryAddSingleton(npgsqlDataSource);

        services.TryAddScoped<IDbConnectionFactory, DbConnectionFactory>();


        services.AddHealthChecks()
            .AddNpgSql(databaseConnectionString, name: "database", tags: ["ready"]);

        return services;
    }
}
