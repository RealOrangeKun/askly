
using Askly.Api.Shared.Abstractions;

namespace Askly.Api.Features.Questions.AskQuestion;

public sealed record AskQuestionQuery(string QuestionText) : IQuery<IEnumerable<QuestionResponse>>;
