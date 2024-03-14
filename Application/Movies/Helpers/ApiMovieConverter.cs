
using System.Diagnostics;
using Domain;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;



namespace Application.Movies.Helpers;
public class ApiMovieConverter : JsonConverter<ApiMovie>
{
    public override void WriteJson(JsonWriter writer, ApiMovie value, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }

    public override ApiMovie ReadJson(JsonReader reader, Type objectType, ApiMovie existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        JObject jsonObject = JObject.Load(reader);

        if (jsonObject.ContainsKey("title"))
        {
            // Mapiraj svojstva za filmove
            return JsonConvert.DeserializeObject<ApiMovie>(jsonObject.ToString());
        }
        else if (jsonObject.ContainsKey("name") || jsonObject.ContainsKey("first_air_date"))
        {
            // Mapiraj svojstva za serije
            var apiMovie = JsonConvert.DeserializeObject<ApiMovie>(jsonObject.ToString());
            apiMovie.Title = apiMovie.Name; // Mapiraj naziv serije na Title
            apiMovie.ReleaseDate = apiMovie.FirstAirDate;
            return apiMovie;
        } 

        return null;
    }
}




