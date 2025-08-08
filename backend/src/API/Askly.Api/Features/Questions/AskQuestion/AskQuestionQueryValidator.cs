using FluentValidation;

namespace Askly.Api.Features.Questions.AskQuestion;

public sealed class AskQuestionQueryValidator : AbstractValidator<AskQuestionQuery>
{
    public AskQuestionQueryValidator()
    {
        RuleFor(x => x.QuestionText)
            .NotEmpty()
            .WithMessage("Question cannot be empty")
            .MaximumLength(500)
            .WithMessage("Question cannot exceed 500 characters");
    }
}
