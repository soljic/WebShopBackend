using Newtonsoft.Json;

namespace Domain;

public class ApiCredits
{
    public int Id { get; set; }
    [JsonProperty("crew")]
    public List<ApiCrewMember> Crew { get; set; } 
    [JsonProperty("cast")]
    public List<ApiCastMember> Cast { get; set; } 

}