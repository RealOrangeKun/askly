using System.Data.Common;
using Askly.Api.Shared.Database;
using Npgsql;

namespace Askly.Api.Infrastructure.Database;

public class DbConnectionFactory(NpgsqlDataSource dataSource) : IDbConnectionFactory
{
    public async ValueTask<DbConnection> OpenConnectionAsync(CancellationToken cancellationToken = default)
    {
        return await dataSource.OpenConnectionAsync(cancellationToken);
    }
}
