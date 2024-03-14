namespace Domain;

public class NotificationRequestAnswer
{
    public NotificationRequestAnswer()
    {
        
    }

    public NotificationRequestAnswer(bool isAccepted, string message, int dateNightId)
    {
        IsAccepted = isAccepted;
        Message = message;
        DateNightId = dateNightId;
    }

    public bool IsAccepted { get; set; }
    public string Message { get; set; }
    public int DateNightId { get; set; }
    
}