using Application.Behaviors;
using Microsoft.Extensions.Caching.Memory;

namespace Application.Infrastructure.CachingService;

public class CacheService : ICachedService
{
    private static readonly TimeSpan DefaultExpiration = TimeSpan.FromMinutes(60);
    private IMemoryCache _memoryCache;
    
    public CacheService(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }

    public async Task<T> GetOrCreateAsync<T>(string key, Func<CancellationToken, Task<T>> factory, TimeSpan? expiration = null,
        CancellationToken cancellationToken = default)
    {
        T result = await _memoryCache.GetOrCreateAsync(
            key,
            entry =>
            {
                entry.SetAbsoluteExpiration(expiration ?? DefaultExpiration);
                return factory(cancellationToken);
            }
        );
        return result;
    }
}