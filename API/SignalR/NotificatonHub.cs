using Application.Notifications;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.MovieDate.Command;
using Domain;
using Microsoft.AspNetCore.SignalR;

namespace API.SignalR
{
    public class NotificatonHub : Hub
    {
        private readonly IMediator _mediator;
        public NotificatonHub(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task SendNotification(Create.Command command)
        {
            var notification = await _mediator.Send(command);

            await Clients.Caller.SendAsync("ReceiveNotification", notification.Value);
        }
        
        public async Task SendNotificationResponse(NotificationResponse response)
        {
            try
            {
                var command = new HandleNotificationResponseCommand(response);

                var result = await _mediator.Send(command);

                // Ažurirajte klijenta s rezultatom obrade odgovora
                await Clients.Caller.SendAsync("ReceiveNotificationResponse", result);
            }
            catch (Exception ex)
            {
                // Uhvatite grešku i ispišite je u konzoli ili sustavu za logiranje
                Console.WriteLine($"Greška prilikom obrade odgovora: {ex.Message}");
                // Možete koristiti ILogger za logiranje, ako je konfiguriran u vašoj aplikaciji
                // _logger.LogError($"Greška prilikom obrade odgovora: {ex.Message}", ex);
            }
        }


        // do something when a client connects, like joins the group
        public override async Task OnConnectedAsync()
        {
            // get the activity id from the query string with query string paramaters
            var httpContext = Context.GetHttpContext();
            var userName = httpContext.User.Identity.Name;

            var result = await _mediator.Send(new List.Query { });
            // send list of comment from database to a caller, the person who connects
            await Clients.Caller.SendAsync("LoadNotification", result.Value);
            await Clients.Caller.SendAsync("LoadNotificationResponses", result.Value);
        }
    }
}

