namespace Domain;

public class ApiTvShow
{
    public bool Adult { get; set; }
    public string BackdropPath { get; set; }
    public int Id { get; set; }
    public string Name { get; set; }
    public string OriginalLanguage { get; set; }
    public string OriginalName { get; set; }
    public string Overview { get; set; }
    public string PosterPath { get; set; }
    public string MediaType { get; set; }
    public List<int> GenreIds { get; set; }
    public double Popularity { get; set; }
    public DateTime FirstAirDate { get; set; }
    public double VoteAverage { get; set; }
    public int VoteCount { get; set; }
    public List<string> OriginCountry { get; set; }
}
