using FluentValidation;

namespace Askly.Api.Features.Questions.CreateQuestion;

public sealed class CreateQuestionCommandValidator : AbstractValidator<CreateQuestionCommand>
{
    public CreateQuestionCommandValidator()
    {
        RuleFor(x => x.QuestionText)
            .NotEmpty()
            .WithMessage("Question text is required")
            .MaximumLength(1000)
            .WithMessage("Question text cannot exceed 1000 characters");

    }
}
