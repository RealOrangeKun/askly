using Askly.Api.Shared.Abstractions;

namespace Askly.Api.Features.Questions.GetQuestions;

public sealed record GetQuestionsQuery(int PageNumber = 0, int PageSize = 10, bool Answered = false) : IQuery<IEnumerable<QuestionResponse>>;
