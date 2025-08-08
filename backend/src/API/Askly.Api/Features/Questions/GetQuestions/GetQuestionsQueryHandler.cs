using System.Data.Common;
using Askly.Api.Shared.Abstractions;
using Askly.Api.Shared.Database;
using Dapper;
using FluentResults;

namespace Askly.Api.Features.Questions.GetQuestions;

public sealed class GetQuestionsQueryHandler(
    IDbConnectionFactory connectionFactory)
    : IQueryHandler<GetQuestionsQuery, IEnumerable<QuestionResponse>>
{
    public async Task<Result<IEnumerable<QuestionResponse>>> Handle(GetQuestionsQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<QuestionResponse> questions = await FetchQuestions(request, cancellationToken);
        return Result.Ok(questions);
    }

    private async Task<IEnumerable<QuestionResponse>> FetchQuestions(GetQuestionsQuery request, CancellationToken cancellationToken)
    {
        await using DbConnection connection = await connectionFactory.OpenConnectionAsync(cancellationToken);

        int offset = request.PageNumber * request.PageSize;

        string statusCondition = request.Answered ? "= 'Answered'" : "!= 'Answered'";

        string sql = $"""
            SELECT 
                id AS {nameof(QuestionResponse.QuestionId)}, 
                question_text AS {nameof(QuestionResponse.QuestionText)},
                status AS {nameof(QuestionResponse.QuestionStatus)}
            FROM questions 
            WHERE status {statusCondition}
            ORDER BY created_at DESC
            OFFSET @offset 
            LIMIT @pageSize
            """;

        return await connection.QueryAsync<QuestionResponse>(
            sql,
            new { offset, pageSize = request.PageSize });
    }
}
