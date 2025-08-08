using Askly.Api.Shared.Abstractions;

namespace Askly.Api.Features.Questions.AnswerQuestion;

public sealed record AnswerQuestionCommand(Guid QuestionId, string AnswerText) : ICommand;
