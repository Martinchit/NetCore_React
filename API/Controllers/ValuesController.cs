using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Persistence;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController: ControllerBase
    {
        private readonly DataContext _dataContext;
        private readonly ILogger<ValuesController> _logger;

        public ValuesController(ILogger<ValuesController> logger, DataContext dataContext)
        {
            _dataContext = dataContext;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Value>>> Get()
        {
            try
            {
                var values = await _dataContext.Values.ToListAsync();
                return Ok(values);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Value>> Get(int id)
        {
            try
            {
                var value = await _dataContext.Values.FirstOrDefaultAsync(c => c.Id == id);
                if (value == null)
                {
                   return NotFound("Values Not Found");
                }
                return Ok(value);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
