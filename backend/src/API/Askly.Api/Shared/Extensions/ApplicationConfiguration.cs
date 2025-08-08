using Askly.Api.Shared.Filters;
using Carter;
using FluentValidation;

namespace Askly.Api.Shared.Extensions;

public static class ApplicationConfiguration
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(AssemblyReference.Assembly);
            });

        services.AddValidatorsFromAssembly(AssemblyReference.Assembly, includeInternalTypes: true);

        services.AddCarter();

        services.AddScoped<ApiKeyAuthFilter>();


        return services;
    }
}
