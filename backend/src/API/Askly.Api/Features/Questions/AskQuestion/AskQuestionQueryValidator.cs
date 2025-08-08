using FluentValidation;

namespace Askly.Api.Features.Questions.AskQuestion;

public sealed class AskQuestionQueryValidator : AbstractValidator<AskQuestionQuery>
{
    public AskQuestionQueryValidator()
    {
        RuleFor(x => x.QuestionText)
            .NotEmpty()
            .WithMessage("Question cannot be empty")
            .MinimumLength(10)
            .WithMessage("Question must be at least 10 characters")
            .MaximumLength(500)
            .WithMessage("Question cannot exceed 500 characters")
            .Must(IsValidQuestion)
            .WithMessage("This doesn't look like a valid question. Please start with a question word, end with '?', or ask something specific.");
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
        bool endsWithQuestionMark = trimmedText.EndsWith("?");
        bool containsQuestionWords = words.Any(w => questionWords.Contains(w));

        string[] imperativePatterns = [
            "tell me", "explain", "describe", "help", "give me", "show me", "find", "how to", "what is", "what are", "what's"
        ];
        bool hasImperativePattern = imperativePatterns.Any(p => lowerText.Contains(p));

        return endsWithQuestionMark || startsWithQuestionWord || containsQuestionWords || hasImperativePattern;
    }
}
