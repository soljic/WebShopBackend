using Domain.Enums;

namespace Domain;

public class FeaturedModel
{
    public string Title { get; set; }
    public MediaType Type { get; set; }
    public List<ApiMovie> Data { get; set; }
}