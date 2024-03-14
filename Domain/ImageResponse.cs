using Newtonsoft.Json;

namespace Domain;

public class ImageResponse
{
    [JsonProperty("backdrops")]
    public List<Image> Images { get; set; }
}