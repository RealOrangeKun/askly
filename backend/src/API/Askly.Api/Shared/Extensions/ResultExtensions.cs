using FluentResults;
using Askly.Api.Shared.Contracts;

namespace Askly.Api.Shared.Extensions;

public static class ResultExtensions
{

    public static ApiResponse ToApiResponse(this Result result, string? successMessage = null, string? failureMessage = null)
    {
        return result.IsSuccess
            ? ApiResponse.Success(successMessage)
            : ApiResponse.Failure(result.Errors.Select(e => e.Message), failureMessage);
    }


    public static ApiResponse<T> ToApiResponse<T>(this Result<T> result, string? successMessage = null, string? failureMessage = null)
    {
        return result.IsSuccess
            ? ApiResponse<T>.Success(result.Value, successMessage)
            : ApiResponse<T>.Failure(result.Errors.Select(e => e.Message), failureMessage);
    }


}
