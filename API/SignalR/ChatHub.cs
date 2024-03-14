using System;
using System.Threading.Tasks;
using Application.Comments;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace API.SignalR
{
    public class ChatHub : Hub
    {
        private readonly IMediator _mediator;
        public ChatHub(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task SendComment(Create.Command command)
        {
            var comment = await _mediator.Send(command);

            await Clients.Group(command.ActivityId.ToString())
                .SendAsync("ReceiveComment", comment.Value);
        }

        // do something when a client connects, like joins the group
        public override async Task OnConnectedAsync()
        {
            // get the activity id from the query string with query string paramaters
            var httpContext = Context.GetHttpContext();
            var activityId = httpContext.Request.Query["activityId"];

            //add connected client to a gruoup, we pass in context connection id and then we give the group the name(activityId), which group we want to 
            await Groups.AddToGroupAsync(Context.ConnectionId, activityId);
            var result = await _mediator.Send(new List.Query { ActivityId = Guid.Parse(activityId) });
            // send list of comment from database to a caller, the person who connects
            await Clients.Caller.SendAsync("LoadComments", result.Value);
        }
    }
}