using Askly.Api.Entities;
using Askly.Api.Shared.Database;
using Microsoft.EntityFrameworkCore;

namespace Askly.Api.Infrastructure.Database.Repositories;

public class QuestionRepository(AsklyDbContext dbContext) : IQuestionRepository
{
    public async Task<Question> AddAsync(Question entity, CancellationToken cancellationToken = default)
    {
        await dbContext.Questions.AddAsync(entity, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task DeleteAsync(Question entity, CancellationToken cancellationToken = default)
    {
        await dbContext.Questions
            .Where(q => q.Id == entity.Id)
            .ExecuteDeleteAsync(cancellationToken);
    }

    public async Task UpdateAsync(Question entity, CancellationToken cancellationToken = default)
    {
        dbContext.Questions.Update(entity);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
