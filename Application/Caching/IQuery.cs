using Application.Core;
using Azure;
using MediatR;

namespace Application.Caching;

public interface IQuery<TResponse> : IRequest<Result<Response>>
{
    
}