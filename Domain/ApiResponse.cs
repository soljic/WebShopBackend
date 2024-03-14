namespace Domain;

public class ApiResponse
{
    public int Page { get; set; }
    public List<ApiMovie> Results { get; set; }
    public Production ProductionResult { get; set; }
}