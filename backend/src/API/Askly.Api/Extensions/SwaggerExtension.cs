using Microsoft.OpenApi.Models;

namespace Askly.Api.Extensions;

internal static class SwaggerExtension
{
    internal static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
    {
        services.AddOpenApi(options =>
        {
            options.AddDocumentTransformer((document, context, cancellationToken) =>
            {
                document.Info = new OpenApiInfo
                {
                    Title = "Askly API",
                    Version = "v1",
                    Description = "Askly API for managing and answering questions in the Askly question-answering system.",
                };

                document.Components ??= new OpenApiComponents();
                document.Components.SecuritySchemes = new Dictionary<string, OpenApiSecurityScheme>
                {
                    ["ApiKey"] = new OpenApiSecurityScheme
                    {
                        Description = "API Key needed to access the endpoints. Example: 'x-api-key: {key}'",
                        Name = "x-api-key",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.ApiKey
                    }
                };

                return Task.CompletedTask;
            });
        });

        return services;
    }

    internal static IApplicationBuilder UseSwaggerUIWithOpenApi(this IApplicationBuilder app)
    {
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/openapi/v1.json", "Askly API v1");
            options.RoutePrefix = "swagger";
        });
        return app;
    }
}
