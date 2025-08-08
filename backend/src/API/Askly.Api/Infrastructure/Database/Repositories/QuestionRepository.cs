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

    public async Task<Question?> GetQuestionById(Guid Id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Questions
            .FirstOrDefaultAsync(q => q.Id == Id, cancellationToken);
    }

    public async Task UpdateAsync(Question entity, CancellationToken cancellationToken = default)
    {
        dbContext.Questions.Update(entity);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> ExistsWithTextAsync(string questionText, CancellationToken cancellationToken = default)
    {
        return await dbContext.Questions
            .AnyAsync(q => q.QuestionText.ToLower() == questionText.ToLower(), cancellationToken);
    }
}
