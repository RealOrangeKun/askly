using Pgvector;

namespace Askly.Api.Shared.Services;

public interface IEmbeddingService
{
    Task<Vector> GenerateEmbeddingAsync(string text, CancellationToken cancellationToken = default);
}
