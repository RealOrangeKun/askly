using Askly.Api.Entities;

namespace Askly.Api.Shared.Database;

public interface IQuestionRepository : IRepository<Question>
{
    Task<Question?> GetQuestionById(Guid Id, CancellationToken cancellationToken = default);
    Task<bool> ExistsWithTextAsync(string questionText, CancellationToken cancellationToken = default);
}
