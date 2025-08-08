using FluentResults;
using MediatR;

namespace Askly.Api.Shared.Abstractions;

public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>, IBaseQueryHandler
    where TQuery : IQuery<TResponse>;


public interface IBaseQueryHandler;
