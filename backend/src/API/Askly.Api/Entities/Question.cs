using Askly.Api.Shared.Database;
using Askly.Api.Shared.ValueObjects;
using Pgvector;

namespace Askly.Api.Entities;

public sealed class Question : Entity
{
    private Question() { }

    public string QuestionText { get; private set; }

    public Vector QuestionEmbedding { get; private set; }

    public QuestionStatus Status { get; private set; }

    public string AnswerText { get; private set; }

    public static Question Create(Vector questionEmbedding, QuestionStatus status, string questionText)
    {
        return new Question
        {
            Id = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            QuestionEmbedding = questionEmbedding,
            Status = status,
            AnswerText = string.Empty,
            QuestionText = questionText
        };
    }
    public void Answer(string answerText)
    {
        AnswerText = answerText;
        Status = QuestionStatus.Answered;
        UpdatedAt = DateTime.UtcNow;
    }
}
