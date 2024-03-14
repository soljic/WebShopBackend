using Domain;

namespace Application.Dtos;

public class MovieDateDto
{
    public DateTime Date { get; set; }
    public int MovieId { get; set; }
    public string MediaType { get; set; }
    public string PartnerId { get; set; }
    public AppUser User { get; set; }
    public int DateNightId { get; set; }
}