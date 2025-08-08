using Microsoft.Extensions.Primitives;

namespace Askly.Api.Shared.Filters;

public class ApiKeyAuthFilter(IConfiguration configuration) : IEndpointFilter
{
    private const string ApiKeyHeaderName = "x-api-key";
    private readonly IConfiguration _configuration = configuration;

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        string? configuredApiKey = _configuration["ApiKey"];

        if (string.IsNullOrEmpty(configuredApiKey))
        {
            return await next(context);
        }

        if (!context.HttpContext.Request.Headers.TryGetValue(ApiKeyHeaderName, out StringValues providedApiKey) || !string.Equals(configuredApiKey, providedApiKey, StringComparison.Ordinal))
        {
            return Results.Unauthorized();
        }

        return await next(context);
    }
}
