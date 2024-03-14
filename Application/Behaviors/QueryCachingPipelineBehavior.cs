using Application.Caching;
using MediatR;

namespace Application.Behaviors;

public sealed class QueryCachingPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : ICachedQuery
{
    private readonly ICachedService _cachedService;
    
    public QueryCachingPipelineBehavior(ICachedService cachedService)
    {
        _cachedService = cachedService;
    }
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        return await _cachedService.GetOrCreateAsync(request.Key, _ => next(), request.Expiration, cancellationToken);
    }
}

