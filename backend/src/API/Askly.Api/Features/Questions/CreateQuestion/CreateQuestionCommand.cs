using Askly.Api.Shared.Abstractions;

namespace Askly.Api.Features.Questions.CreateQuestion;

public sealed record CreateQuestionCommand(string QuestionText) : ICommand<Guid>;

