using FluentResults;
using MediatR;

namespace Askly.Api.Shared.Abstractions;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>;

