namespace Askly.Api.Shared.Caching;

public static class CachePolicy
{
    public static TimeSpan DefaultExpiration => TimeSpan.FromMinutes(5);

    public static class PolicyNames
    {
        public const string QuestionSearch = "QuestionSearch";
    }
}
