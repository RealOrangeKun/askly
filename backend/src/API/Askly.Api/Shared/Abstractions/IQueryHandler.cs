using System;
using FluentResults;
using MediatR;

namespace Askly.Api.Shared.Abstractions;

public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>;

