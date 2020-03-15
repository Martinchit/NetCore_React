using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Activities;
using Application.ActivityReference;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace API
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivitiesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ActivitiesController> _logger;

        public ActivitiesController(ILogger<ActivitiesController> logger, IMediator mediator)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<List<Activity>>> List()
        {
            try
            {
                var activities = await _mediator.Send(new ListActivities.Query());
                if (activities == null)
                {
                    return NoContent();
                }
                return Ok(activities);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Activity>> Get(Guid id)
        {
            try
            {
                var activity = await _mediator.Send(new GetActivity.Query { Id = id });
                if (activity == null)
                {
                    return NoContent();
                }
                return Ok(activity);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<Unit>> Create([FromBody] CreateActivity.Command command)
        {
            return await _mediator.Send(command);
            //_logger.LogInformation("Hereee");
            //try
            //{
            //    return await _mediator.Send(command);
            //}
            //catch (Exception e)
            //{
            //    _logger.LogError(e.Message);
            //    return BadRequest(e.Message);
            //}
        }

        //[HttpPost]
        //public async Task<ActionResult<Unit>> Create([FromBody] Activity activity)
        //{
        //    try
        //    {
        //        return await _mediator.Send(new CreateActivity.Command {
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
        //        _logger.LogError(e.Message);
        //        return BadRequest(e.Message);
        //    }
        //}

        [HttpPut("{id}")]
        public async Task<ActionResult<Unit>> Edit(Guid id, [FromBody] EditActivity.Command command)
        {
            return await _mediator.Send(command);
            //try
            //{
            //    command.Id = id;
            //    return await _mediator.Send(command);
            //}
            //catch (Exception e)
            //{
            //    _logger.LogError(e.Message);
            //    return BadRequest(e.Message);
            //}
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Unit>> Delete(Guid id)
        {
            return await _mediator.Send(new DeleteActivity.Command { Id = id });
            //try
            //{
            //    return await _mediator.Send(new DeleteActivity.Command { Id = id });
            //}
            //catch (Exception e)
            //{
            //    _logger.LogError(e.Message);
            //    return BadRequest(e.Message);
            //}
        }
    }
}
