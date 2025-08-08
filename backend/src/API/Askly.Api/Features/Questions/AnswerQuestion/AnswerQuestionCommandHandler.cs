using Askly.Api.Entities;
using Askly.Api.Shared.Abstractions;
using Askly.Api.Shared.Database;
using FluentResults;

namespace Askly.Api.Features.Questions.AnswerQuestion;

public sealed class AnswerQuestionCommandHandler(
    IQuestionRepository questionRepository)
    : ICommandHandler<AnswerQuestionCommand>
{
    public async Task<Result> Handle(AnswerQuestionCommand request, CancellationToken cancellationToken)
    {
        Question? question = await questionRepository.GetQuestionById(request.QuestionId, cancellationToken);

        if (question is null)
        {
            return Result.Fail("Question not found");
        }

        question.Answer(request.AnswerText);

        await questionRepository.UpdateAsync(question, cancellationToken);

        return Result.Ok();
    }
}
