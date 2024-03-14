using Application.Core;
using Application.Order.Queries;
using AutoMapper;
using Domain;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Persistence;

namespace Application.Movies.Queries
{
    public class GetMoviesQueryHandler : IRequestHandler<GetMoviesQuery, Result<PagedListApi<Movie>>>
    {
        private readonly DataContext _context;
        private readonly MovieService _movieService;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;

        public GetMoviesQueryHandler(DataContext context, IMapper mapper, MovieService movieService, IMemoryCache cache)
        {
            _context = context;
            _mapper = mapper;
            _movieService = movieService;
            _cache = cache;
        }


        public async Task<Result<PagedListApi<Movie>>> Handle(GetMoviesQuery request, CancellationToken cancellationToken)
        { 
            var cacheKey = $"movies_{request.MDbparams.Search ?? ""}_{request.MDbparams.Trending ?? ""}_{request.MDbparams.Popular ?? ""}_{request.MDbparams.PageNumber ?? 1}_{(request.MDbparams.PageSize != 0 ? request.MDbparams.PageSize : 10)}";
            if (_cache.TryGetValue(cacheKey, out Result<PagedListApi<Movie>> cachedResult))
            {
                return cachedResult;
            }

            var movies = await _movieService.GetMovies(request.MDbparams);
            if (movies == null)
            {
                throw new ApplicationException("Problem getting the items");
            }
            foreach (var movie in movies) 
            {
                movie.Cast = movie.Cast.Take(5).ToList();
            } 
            var result = Result<PagedListApi<Movie>>.Success(
                await PagedListApi<Movie>.CreateAsync(movies,request.MDbparams.PageNumber!.Value, request.MDbparams.PageSize)
            );

            _cache.Set(cacheKey, result, TimeSpan.FromHours(1)); // Cache for 1 hour

            return result;
        }
    }
}
