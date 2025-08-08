using FluentResults;
using FluentValidation.Results;

namespace Askly.Api.Shared.Extensions;

public static class ValidationResultExtensions
{
    public static bool TryGetErrors(this ValidationResult validationResult, out IEnumerable<IError> errors)
    {
        if (!validationResult.IsValid)
        {
            errors = validationResult.Errors.Select(e => new Error(e.ErrorMessage));
            return true;
        }

        errors = [];
        return false;
    }
}
