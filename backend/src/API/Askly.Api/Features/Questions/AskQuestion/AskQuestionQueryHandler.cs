using System.Data.Common;
using Askly.Api.Entities;
using Askly.Api.Shared.Abstractions;
using Askly.Api.Shared.Database;
using Askly.Api.Shared.Services;
using FluentResults;
using Dapper;
using Pgvector;
using FluentValidation;
using Askly.Api.Shared.Extensions;
using FluentValidation.Results;

namespace Askly.Api.Features.Questions.AskQuestion;

public sealed class AskQuestionQueryHandler(
    IDbConnectionFactory dbConnectionFactory,
    IEmbeddingService embeddingService,
    IValidator<AskQuestionQuery> validator)
    : IQueryHandler<AskQuestionQuery, IEnumerable<QuestionResponse>>
{
    private const double MAX_DISTANCE = 0.7;

    public async Task<Result<IEnumerable<QuestionResponse>>> Handle(AskQuestionQuery request, CancellationToken cancellationToken)
    {
        ValidationResult validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (validationResult.TryGetErrors(out IEnumerable<IError> errors))
        {
            return Result.Fail(errors);
        }

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

        IEnumerable<QuestionResponse> results = await connection.QueryAsync<QuestionResponse>(sql,
            new { embedding = questionEmbedding, maxDistance = MAX_DISTANCE });

        return Result.Ok(results);
    }
}
