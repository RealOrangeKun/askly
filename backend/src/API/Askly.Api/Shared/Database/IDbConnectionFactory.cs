using System.Data.Common;

namespace Askly.Api.Shared.Database;

public interface IDbConnectionFactory
{
    ValueTask<DbConnection> OpenConnectionAsync(CancellationToken cancellationToken = default);
}
