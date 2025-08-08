using Askly.Api.Shared.Caching;

namespace Askly.Api.Shared.Abstractions;

public interface IUsesCaching<TQuery> where TQuery : class
{
    string CreateCacheKey(TQuery query);
    TimeSpan? GetCacheExpiration() => CachePolicy.DefaultExpiration;
}
