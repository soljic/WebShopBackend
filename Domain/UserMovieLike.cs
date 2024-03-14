namespace Domain;

public class UserMovieLike
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public AppUser User { get; set; }

    public int MovieId { get; set; }
    public Movie Movie { get; set; }
}