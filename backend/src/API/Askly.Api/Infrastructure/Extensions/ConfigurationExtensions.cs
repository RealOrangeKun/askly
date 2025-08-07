namespace Askly.Api.Infrastructure.Extensions;

public static class ConfigurationExtensions
{
    public static string GetConnectionStringOrThrow(this IConfiguration configuration, string name)
    {
        string? connectionString = configuration.GetConnectionString(name);

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException($"Connection string '{name}' not found or is empty.");
        }

        return connectionString;
    }

    public static string GetRequiredValue(this IConfiguration configuration, string key)
    {
        string? value = configuration[key];

        if (string.IsNullOrWhiteSpace(value))
        {
            throw new InvalidOperationException($"Configuration value for key '{key}' not found or is empty.");
        }

        return value;
    }
}
