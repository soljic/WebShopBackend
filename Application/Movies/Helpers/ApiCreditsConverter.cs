using Domain;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Application.Movies.Helpers;

public class ApiCreditsConverter : JsonConverter<ApiCredits>
{
    public override void WriteJson(JsonWriter writer, ApiCredits value, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }

    public override ApiCredits ReadJson(JsonReader reader, Type objectType, ApiCredits existingValue, bool hasExistingValue,
        JsonSerializer serializer)
    {
        var jsonObject = JObject.Load(reader);
        var apiResponse = new ApiCredits();

        // Map 'page' property
        apiResponse.Id = jsonObject["id"].Value<int>();

        // Map 'results' property
        apiResponse.Cast = jsonObject["cast"].ToObject<List<ApiCastMember>>();
        apiResponse.Crew = jsonObject["crew"].ToObject<List<ApiCrewMember>>();
        return apiResponse;
    }
}