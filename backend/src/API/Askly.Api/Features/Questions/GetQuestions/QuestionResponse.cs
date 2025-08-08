using Askly.Api.Shared.ValueObjects;

namespace Askly.Api.Features.Questions.GetQuestions;

public sealed record QuestionResponse(Guid QuestionId, string QuestionText, string QuestionStatus);