using Askly.Api.Entities;
using Askly.Api.Shared.Abstractions;
using Askly.Api.Shared.Database;
using Askly.Api.Shared.Services;
using Pgvector;
using FluentResults;
using Askly.Api.Shared.ValueObjects;
using FluentValidation;
using FluentValidation.Results;
using Askly.Api.Shared.Extensions;

namespace Askly.Api.Features.Questions.CreateQuestion;

public sealed class CreateQuestionCommandHandler(
    IQuestionRepository questionRepository,
    IValidator<CreateQuestionCommand> validator,
    IEmbeddingService embeddingService) : ICommandHandler<CreateQuestionCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateQuestionCommand request, CancellationToken cancellationToken)
    {
        ValidationResult validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (validationResult.TryGetErrors(out IEnumerable<IError> errors))
        {
            return Result.Fail(errors);
        }

        bool questionExists = await questionRepository.ExistsWithTextAsync(request.QuestionText, cancellationToken);

        if (questionExists)
        {
            return Result.Fail("A question with this text already exists");
        }

        Vector embedding = await embeddingService.GenerateEmbeddingAsync(request.QuestionText, cancellationToken);

        var question = Question.Create(
            embedding,
            QuestionStatus.Unanswered,
            request.QuestionText
        );

        await questionRepository.AddAsync(question, cancellationToken);

        return Result.Ok(question.Id);
    }

}
