using System.Diagnostics;
using System.Net.Http.Headers;
using Domain;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using Application.Movies.Helpers;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Application.Movies;

public class MovieHttpClient
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    public readonly string _baseUrl;
    public readonly string _apiKey;

    public MovieHttpClient(IConfiguration configuration)
    {
        _configuration = configuration;
        string _baseUrl = _configuration.GetSection("MovieService:BaseUrl").Value;
        string _apiKey = _configuration.GetSection("MovieService:ApiKey").Value;

        Uri baseUri = new Uri(_baseUrl);
        _httpClient = new HttpClient
        {
            BaseAddress = baseUri
        };

        // Postavljanje tokena u zaglavlje za autorizaciju
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",
            _configuration.GetSection("MovieService:AccessToken").Value);
    }

    public async Task<List<ApiMovie>> GetMoviesAsync(string url)
    {
        try
        {
            var movies = await GetApiResponseAsync<ApiResponse>(url, new ApiMovieConverter() );
            ArgumentNullException.ThrowIfNull(movies);
            return movies.Results;
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            if (ex.InnerException != null)
            {
                Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
            }

            return null;
        }
    }

    public async Task<ApiCredits> GetMoviesCreditsAsync(string url)
    {
        try
        {
            var credits = await GetApiResponseAsync<ApiCredits>(url, new ApiCreditsConverter() );
            ArgumentNullException.ThrowIfNull(credits);
            return credits;
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            if (ex.InnerException != null)
            {
                Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
            }

            return null;
        }
    }

    public async Task<List<Genre>> GetGenreAsync(string url)
    {
        try
        {
            var genre = await GetApiResponseAsync<GenresResponse>(url);
            ArgumentNullException.ThrowIfNull(genre);
            return genre.Genres;
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            if (ex.InnerException != null)
            {
                Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
            }

            return null;
        }
    }

    public async Task<ApiMovie> GetMovieByIdAsync(int id)
    {
        var url = $"movie/{id}?language=en-us&";
        try
        {
            var movie = await GetApiResponseAsync<ApiMovie>(url);
            ArgumentNullException.ThrowIfNull(movie);
            return movie;
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            if (ex.InnerException != null)
            {
                Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
            }

            return null;
        }
    }

    private async Task<T> GetApiResponseAsync<T>(string url, JsonConverter customConverter = null )
    {
        string apiKey = _configuration.GetSection("MovieService:ApiKey").Value;
        var response = await _httpClient.GetAsync($"{url}api_key={apiKey}");

        if (response.IsSuccessStatusCode)
        {
            var jsonString = await response.Content.ReadAsStringAsync();
            var settings = new JsonSerializerSettings
            {
                ContractResolver =  new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                },
                Formatting = Formatting.Indented
            };
            
            if (customConverter != null)
            {
                settings.Converters.Add(customConverter);
            }

            var production = JsonConvert.DeserializeObject<T>(jsonString, settings);
            return production;
        }
        else
        {
            throw new HttpRequestException($"Request to {url} failed with status code {response.StatusCode}"); // Handle unsuccessful HTTP response
        }
    }
}

