using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Controllers;
using Application.Activities;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace API
{
    public class ActivitiesController : BaseController
    {

        [HttpGet]
        public async Task<ActionResult<List<Activity>>> List()
        {
            try
            {
                var activities = await Mediator.Send(new ListActivities.Query());
                if (activities == null)
                {
                    return NoContent();
                }
                return Ok(activities);
            }
            catch (Exception e)
            {
                Logger.LogError(e.Message);
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Activity>> Get(Guid id)
        {
            try
            {
                var activity = await Mediator.Send(new GetActivity.Query { Id = id });
                if (activity == null)
                {
                    return NoContent();
                }
                return Ok(activity);
            }
            catch (Exception e)
            {
                Logger.LogError(e.Message);
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<Unit>> Create([FromBody] CreateActivity.Command command)
        {
            return await Mediator.Send(command);
            //Logger.LogInformation("Hereee");
            //try
            //{
            //    return await Mediator.Send(command);
            //}
            //catch (Exception e)
            //{
            //    Logger.LogError(e.Message);
            //    return BadRequest(e.Message);
            //}
        }

        //[HttpPost]
        //public async Task<ActionResult<Unit>> Create([FromBody] Activity activity)
        //{
        //    try
        //    {
        //        return await Mediator.Send(new CreateActivity.Command {
        //            Id = activity.Id,
        //            Category = activity.Category,
        //            City = activity.City,
        //            Date = activity.Date,
        //            Description = activity.Description,
        //            Title = activity.Title,
        //            Venue = activity.Venue
        //        });
        //    }
        //    catch (Exception e)
        //    {
        //        Logger.LogError(e.Message);
        //        return BadRequest(e.Message);
        //    }
        //}

        [HttpPut("{id}")]
        public async Task<ActionResult<Unit>> Edit(Guid id, [FromBody] EditActivity.Command command)
        {
            return await Mediator.Send(command);
            //try
            //{
            //    command.Id = id;
            //    return await Mediator.Send(command);
            //}
            //catch (Exception e)
            //{
            //    Logger.LogError(e.Message);
            //    return BadRequest(e.Message);
            //}
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Unit>> Delete(Guid id)
        {
            return await Mediator.Send(new DeleteActivity.Command { Id = id });
            //try
            //{
            //    return await Mediator.Send(new DeleteActivity.Command { Id = id });
            //}
            //catch (Exception e)
            //{
            //    Logger.LogError(e.Message);
            //    return BadRequest(e.Message);
            //}
        }
    }
}
