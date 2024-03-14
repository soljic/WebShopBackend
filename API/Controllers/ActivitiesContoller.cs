using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Activity;
using Application.Core;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/activities")]
    [ApiController]
    public class ActivitiesContoller : BaseApiController
    {
        private readonly IMediator _mediator;
        public ActivitiesContoller(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> List([FromQuery] ActivityParams param)
        {
            return HandlePagedResult(await _mediator.Send(new List.Query { Params = param}));
        }

         [HttpGet("{id}")]
        public async Task<IActionResult> Details(Guid id)
        {
            return HandleResult(await _mediator.Send(new Details.Query{Id = id}));
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
