using Askly.Api.Infrastructure.Caching;
using Askly.Api.Shared.Caching;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Askly.Api.Infrastructure.Extensions;

public static class CacheExensions
{
    public static IServiceCollection AddCachingInternal(this IServiceCollection services, IConfiguration configuration)
    {
        string redisConnectionString = configuration.GetConnectionStringOrThrow("Redis");
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = redisConnectionString;
        });

        services.ConfigureOptions<HybridCacheConfigureOptions>();

        services.AddHybridCache();

        services.TryAddScoped<ICacheService, CacheService>();

        services.AddOutputCache(options =>
        {
            options.DefaultExpirationTimeSpan = CachePolicy.DefaultExpiration;

            options.AddPolicy(CachePolicy.PolicyNames.QuestionSearch, policy =>
            {
                policy.Expire(CachePolicy.DefaultExpiration);
                policy.SetVaryByQuery("questionText");
            });
        });

        return services;
    }
}
