using System.Data.Common;
using Askly.Api.Shared.Abstractions;
using Askly.Api.Shared.Database;
using Askly.Api.Shared.Services;
using FluentResults;
using Dapper;
using Pgvector;
using FluentValidation;
using Askly.Api.Shared.Extensions;
using FluentValidation.Results;
using Askly.Api.Shared.Caching;
using System.Security.Cryptography;
using System.Text;

namespace Askly.Api.Features.Questions.AskQuestion;

public sealed class AskQuestionQueryHandler(
    IDbConnectionFactory dbConnectionFactory,
    IEmbeddingService embeddingService,
    IValidator<AskQuestionQuery> validator,
    ICacheService cacheService)
    : IQueryHandler<AskQuestionQuery, IEnumerable<QuestionResponse>>, IUsesCaching<AskQuestionQuery>
{
    private const double MAX_DISTANCE = 0.7;

    public string CreateCacheKey(AskQuestionQuery query)
    {
        string normalizedText = query.QuestionText.ToLower().Trim();
        byte[] hash = SHA256.HashData(
            Encoding.UTF8.GetBytes(normalizedText)
        );
        return $"similar_questions:{Convert.ToHexString(hash)[..16]}";
    }

    public async Task<Result<IEnumerable<QuestionResponse>>> Handle(AskQuestionQuery request, CancellationToken cancellationToken)
    {

        ValidationResult validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (validationResult.TryGetErrors(out IEnumerable<IError> errors))
        {
            return Result.Fail(errors);
        }


        string cacheKey = CreateCacheKey(request);

        TimeSpan? expiration = ((IUsesCaching<AskQuestionQuery>)this).GetCacheExpiration();

        IEnumerable<QuestionResponse> cacheResult = await cacheService.GetOrCreateAsync(
            cacheKey,
            async ct => await FetchSimilarQuestions(request, cancellationToken),
            expiration,
            cancellationToken);


        return Result.Ok(cacheResult);
    }

    private async Task<IEnumerable<QuestionResponse>> FetchSimilarQuestions(AskQuestionQuery request, CancellationToken cancellationToken)
    {
        using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync(cancellationToken);

        Vector questionEmbedding = await embeddingService.GenerateEmbeddingAsync(request.QuestionText, cancellationToken);

        const string sql = $"""
                SELECT 
                    question_text AS {nameof(QuestionResponse.QuestionText)}, 
                    answer_text AS {nameof(QuestionResponse.AnswerText)}
                FROM questions 
                WHERE status = 'Answered'
                  AND question_embedding <=> @embedding::vector < @maxDistance
                ORDER BY question_embedding <=> @embedding::vector
                LIMIT 5
                """;

        return await connection.QueryAsync<QuestionResponse>(sql,
            new { embedding = questionEmbedding, maxDistance = MAX_DISTANCE });
    }

}
