using API.SignalR;
using Application.Activity;
using Application.Dtos;
using Application.MovieDate.Command;
using Application.MovieDate.Queries;
using Application.Movies;
using Application.Order.Queries;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace API.Controllers;

public class MovieDateController: BaseApiController
{
    private readonly IMediator _mediator;
    
    private readonly IHubContext<NotificatonHub> _hubContext;
    public MovieDateController(IMediator mediator, IHubContext<NotificatonHub> hubContext)
    {
        _mediator = mediator;
        _hubContext = hubContext;
    }
        
    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> List([FromQuery] MDbParams mDbparams)
    {
        return HandlePagedResultApi<Movie>(await _mediator.Send(new GetMoviesQuery
        {
            MDbparams = mDbparams
        }));
    }

    /*[HttpGet("{id}")]
    public async Task<IActionResult> Details(Guid id)
    {
        return HandleResult(await _mediator.Send(new Details.Query{Id = id}));
    }*/

    [HttpPost]
    public async Task<IActionResult> CreateMovieDate(MovieDateDto movieDateDto)
    {
        var result = await Mediator.Send(new CreateMovieDateCommand(movieDateDto));
        if (result.IsSuccess)
        {
            var command = new Application.Notifications.Create.Command();
            command.Body = "New Movie Date Created";
            command.UserName = result.Value.User.UserName;
            command.PartnerId = movieDateDto.PartnerId;
            command.DateNightId = result.Value.DateNightId;
            await Mediator.Send(command);
            
            await _hubContext.Clients.User(result.Value.User.Id).SendAsync("ReceiveNotification", command);
        }
        return HandleResult(result);
    }
    
    [Authorize(Policy = "IsActivityHost")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Edit(Guid id, Activities command)
    {
        command.Id = id;
        return HandleResult(await _mediator.Send(new Edit.Command{Activity = command }));
    }
    
    
    [HttpPost("respondToNotification/{notificationId}")]
    public async Task<IActionResult> RespondToNotification(NotificationResponse response, Guid notificationId)
    {
        try
        {
            // Ovdje možete obraditi primljeni NotificationResponse
            // Prilagodite ovu logiku prema vašim potrebama
            // Na primjer, možete koristiti _mediator.Send za obradu odgovora

            // Primjer kako možete koristiti Mediator za obradu odgovora
            var command = new HandleNotificationResponseCommand(response);

            var result = await _mediator.Send(command);

            // Provjerite rezultat i obradite ga prema vašim potrebama
            if (result.IsSuccess)
            {
                // Uspješno obrađen odgovor
                return Ok();
            }
            else
            {
                // Neuspješno obrađen odgovor
                return BadRequest(result.Error);
            }
        }
        catch (Exception ex)
        {
            // Uhvatite i obradite eventualne greške
            return StatusCode(500, "Internal Server Error");
        }
    }


    [Authorize(Policy = "IsActivityHost")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
           
        return HandleResult(await _mediator.Send(new Delete.Command{Id = id}));
    }

    [HttpPost("{id}/attend")]
    public async Task<IActionResult> Attend(Guid id)
    {
        return HandleResult(await Mediator.Send(new UpdateAttendance.Command {Id = id }));
    }
}   