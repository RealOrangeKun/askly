
using FluentResults;

namespace Askly.Api.Shared.Exceptions;

public sealed class AsklyException : Exception
{
    public AsklyException(string requestName, Error? error = default, Exception? innerException = null)
        : base($"Application exception occurred in '{requestName}'.", innerException)
    {
        RequestName = requestName;
        Error = error ?? new Error("An unexpected error occurred.");
    }

    public string RequestName { get; }
    public Error? Error { get; }

}
