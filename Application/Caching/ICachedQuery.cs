using CloudinaryDotNet.Actions;

namespace Application.Caching;

public interface ICachedQuery
{
    public string Key { get; set; }
    public TimeSpan? Expiration { get; set; }
}

public interface ICachedQuery<TResponse> : IQuery<TResponse>, ICachedQuery
{
}