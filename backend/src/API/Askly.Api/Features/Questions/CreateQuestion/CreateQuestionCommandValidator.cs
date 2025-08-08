using FluentValidation;

namespace Askly.Api.Features.Questions.CreateQuestion;

public sealed class CreateQuestionCommandValidator : AbstractValidator<CreateQuestionCommand>
{
    public CreateQuestionCommandValidator()
    {
        RuleFor(x => x.QuestionText)
            .NotEmpty().WithMessage("Question text is required")
            .MaximumLength(1000).WithMessage("Question text cannot exceed 1000 characters")
            .MinimumLength(10).WithMessage("Question text must be at least 10 characters")
            .Must(IsValidQuestion).WithMessage("This doesn't look like a valid question. Please start with a question word, end with '?', or ask something specific.");
    }

    private static bool IsValidQuestion(string? text)
    {
        if (string.IsNullOrWhiteSpace(text)) return false;
        string trimmedText = text.Trim();
        string lowerText = trimmedText.ToLowerInvariant();
        string[] words = lowerText.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        string[] genericPhrases = [
            "hi", "hello", "hey", "thanks", "thank you", "ok", "okay", "yes", "no",
            "good", "bad", "test", "testing", "help", "please"
        ];
        if (genericPhrases.Contains(lowerText) || (words.Length <= 2 && genericPhrases.Any(p => lowerText.Contains(p))))
            return false;

        string[] questionWords = [
            "what", "how", "why", "when", "where", "who", "which", "whose",
            "can", "could", "would", "should", "will", "do", "does", "did",
            "is", "are", "was", "were", "am", "have", "has", "had"
        ];
        bool startsWithQuestionWord = questionWords.Any(qw => lowerText.StartsWith(qw + " ") || lowerText.StartsWith(qw + "'"));
        bool endsWithQuestionMark = trimmedText.EndsWith('?');
        bool containsQuestionWords = words.Any(w => questionWords.Contains(w));

        // Imperative/question patterns
        string[] imperativePatterns = [
            "tell me", "explain", "describe", "help", "give me", "show me", "find", "how to", "what is", "what are", "what's"
        ];
        bool hasImperativePattern = imperativePatterns.Any(p => lowerText.Contains(p));

        return endsWithQuestionMark || startsWithQuestionWord || containsQuestionWords || hasImperativePattern;
    }
}
