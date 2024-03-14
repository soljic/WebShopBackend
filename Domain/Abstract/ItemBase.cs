namespace Domain.Abstract;

public abstract class ItemBase
{   
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string DateOfRelease { get; set; }
    
    public string MediaType { get; set; }
    
}