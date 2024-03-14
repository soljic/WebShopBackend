using System.ComponentModel;
using Domain.Enums;
using Newtonsoft.Json;

namespace Domain;

public class ApiMovie
{
    public bool Adult { get; set; }
    [JsonProperty("backdrop_path")]
    public string BackdropPath { get; set; }
    public string Type { get; set; }
    public int Id { get; set; }
    public string Name { get; set; }
    public string Title { get; set; }
    public string OriginalLanguage { get; set; }
    public string OriginalTitle { get; set; }
    
    public List<Genre> Genres { get; set; }
    public string Overview { get; set; }
    [JsonProperty("poster_path")]
    public string PosterPath { get; set; }
    [JsonProperty("media_type")]
    public string MediaType { get; set; }
    [JsonProperty("genre_ids")]
    public List<int> GenreIds { get; set; } 
    [JsonProperty("cast")]
    public List<ApiCastMember> Actors { get; set; }
    public double Popularity { get; set; }
    [JsonProperty("release_date")]
    public string ReleaseDate { get; set; }
    [JsonProperty("first_air_date")]
    public string FirstAirDate { get; set; }
    public Production Production { get; set; }
    public string Director { get; set; }
    public bool Video { get; set; }
    [JsonProperty("vote_average")]
    public double VoteAverage { get; set; }
    public int VoteCount { get; set; }
}