using Esti_bus_project.IRepository;
using Esti_bus_project.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Collections.Generic;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Esti_bus_project.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ApiController : ControllerBase
    {

        private readonly IRepository<Models.Route> _routeRepository;
        private readonly IRepository<Stop> _stopRepository;
        private readonly IRepository<Trip> _tripRepository;
        private readonly IRepository<StopTime> _stopTimeRepository;
        private readonly ILogger<ApiController> _logger;

        public ApiController(IRepository<Models.Route> routeRepository,
            IRepository<Stop> stopRepository,
            IRepository<Trip> tripRepository,
            IRepository<StopTime> stopTimeRepository,
            ILogger<ApiController> logger)
        {
            _routeRepository=routeRepository;
            _stopRepository=stopRepository;
            _tripRepository=tripRepository;
            _stopTimeRepository=stopTimeRepository;
            _logger = logger;
        }
        // GET: api/<ApiController>
        [HttpGet("{table}/{count}")]
        public async Task<IActionResult> GetItemsAsync(string table, int count)
        {
            if (table == "routes")
            {
                var items =await _routeRepository.GetQuerableAsync(count);
                return Ok(items);
            }
            else if (table == "stops")
            {
                var items = await _stopRepository.GetQuerableAsync(count);
                return Ok(items);
            }
            else if (table == "trips")
            {
                var items = await _tripRepository.GetQuerableAsync(count);
                return Ok(items);
            }
            else if(table == "stop_time")
            {
                var items = await _stopTimeRepository.GetQuerableAsync(count);
                return Ok(items);
            }
            else
            {
                return BadRequest();
            }

        }
        // GET api/<ApiController>/5
        [HttpGet("{table}/{id}")]
        public async Task<IActionResult> GetItemAsync(string table,int id)
        {
            return null;
        }

        // POST api/<ApiController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ApiController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ApiController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
