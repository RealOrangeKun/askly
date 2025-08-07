using Askly.Api.Shared.Database;
using Askly.Api.Shared.ValueObjects;
using Pgvector;

namespace Askly.Api.Entities;

public sealed class Question : Entity
{
    private Question() { }

    public Vector QuestionEmbedding { get; private set; }

    public QuestionStatus Status { get; private set; }

    public string AnswerText { get; private set; }

    public static Question Create(Vector questionEmbedding, QuestionStatus status, string answerText)
    {
        return new Question
        {
            Id = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            QuestionEmbedding = questionEmbedding,
            Status = status,
            AnswerText = answerText
        };
    }

    public void Update(QuestionStatus status, string answerText)
    {
        Status = status;
        AnswerText = answerText;
        UpdatedAt = DateTime.UtcNow;
    }
}
