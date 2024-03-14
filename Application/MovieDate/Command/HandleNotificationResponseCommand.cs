using Application.Core;
using Domain;
using MediatR;

namespace Application.MovieDate.Command;

public class HandleNotificationResponseCommand : IRequest<Result<NotificationRequestAnswer>>
{ 
    public NotificationResponse NotificationResponse { get; set; }   
    public HandleNotificationResponseCommand(NotificationResponse notificationResponse)
    {
        NotificationResponse = notificationResponse;
    }
    
}