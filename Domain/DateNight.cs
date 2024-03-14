using System.ComponentModel.DataAnnotations.Schema;

namespace Domain;

public class DateNight
{
    public int Id { get; set; }
    public DateTime Time { get; set; }
    public string MediaType { get; set; }
    public int MediaTypeId { get; set; }
    public string PartnerId { get; set; }
    public string AppUserId { get; set; }
    
    public bool IsAccepted { get; set; } = false;
    [ForeignKey("DateNightId")]
    public ICollection<Notification> Notifications { get; set; }

}