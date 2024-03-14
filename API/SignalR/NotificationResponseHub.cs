using System;
using System.Threading.Tasks;
using Application.MovieDate.Command;
using Application.Notifications;
using Domain;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace API.SignalR
{
    public class NotificationResponseHub : Hub
    {
        private readonly IMediator _mediator;

        public NotificationResponseHub(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task SendNotificationResponse(NotificationResponse response, Guid notificationId)
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

        public override async Task OnConnectedAsync()
        {
            // var httpContext = Context.GetHttpContext();
            // var userName = httpContext.User.Identity.Name;
            //
            // var result = await _mediator.Send(new List.Query { });
            //
            // // Pošaljite klijentu njegove notifikacije
            // await Clients.Caller.SendAsync("LoadNotification", result.Value);

            //Nastavite s originalnom logikom OnConnectedAsync
            await base.OnConnectedAsync();
        }
    }
}