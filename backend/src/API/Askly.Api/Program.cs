using Askly.Api.Extensions;
using Askly.Api.Infrastructure.Extensions;
using Askly.Api.Middlewares;
using Askly.Api.Shared.Extensions;
using Askly.Api.Shared.Filters;
using Carter;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

builder.Services.AddProblemDetailsWithExtensions();
builder.Services.AddExceptionHandler<ValidationExceptionHandler>();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddSwaggerDocumentation();

builder.Services.AddScoped<ApiKeyAuthFilter>();

builder.Services
    .AddInfrastructure(builder.Configuration)
    .AddApplication();

builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
});


WebApplication app = builder.Build();

app.UseExceptionHandler();

app.UseResponseCompression();

app.UseOutputCache();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUIWithOpenApi();
}

app.MapCarter();

app.MapHealthChecks("/health", new HealthCheckOptions
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

await app.RunAsync();
