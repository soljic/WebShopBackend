using System.Text.Json;
using Application.Core;
using AutoMapper;
using Domain;
using Domain.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Caching.Memory;

namespace Application.Movies;

public class MovieService
{
    private readonly MovieHttpClient _httpClient;
    private readonly IMapper _mapper;
    private readonly IMemoryCache _cache;

    public MovieService(MovieHttpClient httpClient, IMapper mapper, IMemoryCache cache)
    {
        _httpClient = httpClient;
        _mapper = mapper;
        _cache = cache;

    }
    public async Task<List<Movie>> GetMovies(MDbParams mDbParams)
    {
        var cacheKey = $"genres_{12344354234}_";
        
        if (!_cache.TryGetValue(cacheKey, out IEnumerable<Genre> combinedGenres))
        {
            var movieGenreUrl = "genre/movie/list?language=en&";
            var tvGenreUrl = "genre/tv/list?language=en&";
            var movieGenres = await _httpClient.GetGenreAsync(movieGenreUrl);
            var tvGenres = await _httpClient.GetGenreAsync(tvGenreUrl);
            combinedGenres = movieGenres.Union(tvGenres,new GenreEqualityComparer());
            _cache.Set(cacheKey, combinedGenres, TimeSpan.FromDays(30)); // Cache for 30 days
        }
        
        var combinedGenresList = combinedGenres.ToList();
        if (mDbParams.Trending != null && mDbParams.Popular != null)
        {
            mDbParams.Popular = null;
        }
        var mDbUrl = mDbParams switch
        {
            { Search: not null } => $"search/movie?language=en-US&page={mDbParams.PageNumber}&include_adult=false&query={mDbParams.Search}&",
            { Popular: not null } => $"movie/popular?language=en-US&page={mDbParams.PageNumber}&",
            _ => $"trending/all/week?page={mDbParams.PageNumber}&"
        };
        
        // Dohvati listu filmova s API-ja
        var apiMovies = await _httpClient.GetMoviesAsync(mDbUrl);
        ArgumentNullException.ThrowIfNull(apiMovies);
        var semaphore = new SemaphoreSlim(10); // Maksimalno 10 simultanih zahtjeva
        var creditTasks = apiMovies.Select(async movie =>
        {
            await semaphore.WaitAsync();

            try
            {
               
                RefactorImageUrl(movie);
                var genreIds = movie.GenreIds.ToList();
                movie.Genres = combinedGenresList.Where(g => genreIds.Contains(g.Id)).ToList();
                
                var creditUrl = $"movie/{movie.Id}/credits?language=en-us&";
                
                var production = await _httpClient.GetMoviesCreditsAsync(creditUrl);
                if (production != null)
                {
                    movie.Production ??= new Production();
                    if(movie.Id == production.Id)
                    {
                        if (production.Cast != null)
                        {
                            foreach (var actor in production.Cast)
                            {
                                movie.Actors ??= new List<ApiCastMember>();
                                if (actor.Name != null)
                                {
                                    movie.Actors.Add(actor);
                                }
                            }
                        }
                        if (production.Crew != null)
                        {
                            foreach (var member in production.Crew)
                            {
                                if (member is { Job: "Director" })
                                {
                                    movie.Director = member.Name;
                                }
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Film nije pronađen.");
                }
               
            }
            finally
            {
                semaphore.Release();
            }
        });

        await Task.WhenAll(creditTasks);
       
        // Mapiraj listu API filmova izravno u listu vaših Movie objekata
        var movies = _mapper.Map<List<Movie>>(apiMovies);
        return movies;
    }

    public async Task<Movie> GetMovieByIdAsync(int  id)
    {
        var apiMovie =  await _httpClient.GetMovieByIdAsync(id);
        var movie = _mapper.Map<Movie>(apiMovie);
        return movie;
    }

    private void RefactorImageUrl(ApiMovie movie)
    {
        string baseImageUrl = "https://image.tmdb.org/t/p/original";
        if (movie.BackdropPath != null)
        { 
            movie.PosterPath = $"{baseImageUrl}{movie.BackdropPath}";
        }
    }
    
}