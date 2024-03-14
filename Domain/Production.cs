namespace Domain;

public class Production
{
    public int ProductionId { get; set; }
    public List<ApiCastMember> Cast { get; set; }
    public string Director { get; set; }
}