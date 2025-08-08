
namespace Askly.Api.Shared.Contracts;

public sealed class ApiResponse<T>
{
    public bool IsSuccess { get; init; }
    public T? Data { get; init; }
    public string? Message { get; init; }
    public IEnumerable<string>? Errors { get; init; } = [];

    private ApiResponse() { }

    public static ApiResponse<T> Success(T data, string? message = null)
    {
        return new ApiResponse<T>
        {
            IsSuccess = true,
            Data = data,
            Message = message
        };
    }

    public static ApiResponse<T> Failure(string error, string? message = null)
    {
        return new ApiResponse<T>
        {
            IsSuccess = false,
            Data = default,
            Message = message ?? "Operation failed",
            Errors = [error]
        };
    }

    public static ApiResponse<T> Failure(IEnumerable<string> errors, string? message = null)
    {
        return new ApiResponse<T>
        {
            IsSuccess = false,
            Data = default,
            Message = message ?? "Operation failed",
            Errors = errors
        };
    }
}


public sealed class ApiResponse
{
    public bool IsSuccess { get; init; }
    public string? Message { get; init; }
    public IEnumerable<string>? Errors { get; init; } = [];

    private ApiResponse() { }

    public static ApiResponse Success(string? message = null)
    {
        return new ApiResponse
        {
            IsSuccess = true,
            Message = message
        };
    }

    public static ApiResponse Failure(string error, string? message = null)
    {
        return new ApiResponse
        {
            IsSuccess = false,
            Message = message ?? "Operation failed",
            Errors = [error]
        };
    }

    public static ApiResponse Failure(IEnumerable<string> errors, string? message = null)
    {
        return new ApiResponse
        {
            IsSuccess = false,
            Message = message ?? "Operation failed",
            Errors = errors
        };
    }
}
