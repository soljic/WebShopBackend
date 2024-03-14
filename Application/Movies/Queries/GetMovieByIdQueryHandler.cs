using Application.Core;
using AutoMapper;
using Domain;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace Application.Movies.Queries;

public class GetMovieByIdQueryHandler : IRequestHandler<GetMovieByIdQuery, Result<Movie>>
{
    private readonly MovieService _movieService;
    private readonly IMapper _mapper;
    private readonly IMemoryCache _cache;

    public GetMovieByIdQueryHandler(MovieService movieService, IMapper mapper, IMemoryCache cache)
    {
        _movieService = movieService;
        _mapper = mapper;
        _cache = cache;
    }


    public async Task<Result<Movie>> Handle(GetMovieByIdQuery request, CancellationToken cancellationToken)
    {
        var movie =  await _movieService.GetMovieByIdAsync(request.Id);
        if (movie == null) return Result<Movie>.Failure($"Movie with {request.Id} not found");
        return Result<Movie>.Success(movie);
    }
}