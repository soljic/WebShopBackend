using Application.Activity;
using Application.Core;
using Application.Movies;
using Application.Movies.Queries;
using Application.Order.Queries;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/movies")]
    [ApiController]
    public class MoviesController : BaseApiController
    {
        private readonly IMediator _mediator;
        public MoviesController(IMediator mediator)
        {
            _mediator = mediator;
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

         [HttpGet("{id}")]
        public async Task<IActionResult> Details(int id)
        {
            return HandleResult(await _mediator.Send(new GetMovieByIdQuery {Id = id}));
        }

        [HttpPost]
        public async Task<IActionResult> CreateActivity(Activities activitiy)
        {
            return HandleResult(await Mediator.Send(new Create.Command {Activity = activitiy }));
        }
         [Authorize(Policy = "IsActivityHost")]
         [HttpPut("{id}")]
        public async Task<IActionResult> Edit(Guid id, Activities command)
        {
            command.Id = id;
            return HandleResult(await _mediator.Send(new Edit.Command{Activity = command }));
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
}   
