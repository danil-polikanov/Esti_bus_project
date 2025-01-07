using System.Diagnostics;
using System.Linq;
using System.Net;
using Esti_bus_project.Data.Repository;
using Esti_bus_project.IRepository;
using Esti_bus_project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Esti_bus_project.Controllers
{
    [Route("[controller]/[action]")]
    public class HomeController : Controller
    {
        private readonly IRepository<Models.Route> _routeRepository;
        private readonly IRepository<Stop> _stopRepository;
        private readonly IRepository<Trip> _tripRepository;
        private readonly IRepository<StopTime> _stopTimeRepository;
        private readonly ILogger<HomeController> _logger;

        public HomeController(IRepository<Models.Route> routeRepository,
            IRepository<Stop> stopRepository,
            IRepository<Trip> tripRepository,
            IRepository<StopTime> stopTimeRepository,
            ILogger<HomeController> logger)
        {
            _routeRepository = routeRepository;
            _stopRepository= stopRepository;
            _tripRepository= tripRepository;
            _stopTimeRepository= stopTimeRepository;
            _logger = logger;
        }
        [Route("/")]
        [HttpGet]
        // GET: Authors
        public async Task<IActionResult> Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> GetRegionsAsync(string query)
        {
            var regions = await _stopRepository.GetFilteredAsync(
                filter: r => string.IsNullOrEmpty(query) || r.StopArea.Contains(query),
                selector: r => new { id = r.StopId, name = r.StopArea }
            );
            var regionsDistinct=regions.DistinctBy(r=>r.name).ToList();
            
            return Ok(regionsDistinct);
        }
        [HttpPost]
        public async Task<IActionResult> SubmitRegionAsync([FromBody] string region)
        {
            if (string.IsNullOrEmpty(region))
            {
                return BadRequest(new { message = "Region is required." });
            }
            var stops = await _stopRepository.GetFilteredAsync(
                filter: r => r.StopArea == region,
                selector: r => new { Name = r.StopName }
                );
            var stopsDistinct = stops.DistinctBy(r => r.Name).ToList();
            // Логика обработки выбранного региона
            return Ok(stopsDistinct);
        }
        [HttpGet]
        public async Task<IActionResult> GetRouteShortNamesAsync([FromBody] string stopName)
        {
            var stopExists = await _stopRepository.GetFilteredAsync(
                filter: s =>s.StopName == stopName,
                selector: s => s.StopId
            );

            if (!stopExists.Any())
            {
                return NotFound("Stop not found.");
            }
            var StopIdList=stopExists.ToList();
            var routeShortNames = await _stopTimeRepository.GetJoinedFilteredAsync<Models.Route, string, string>(
            filter: st => StopIdList.Contains(st.StopId),
            outerKeySelector: st => st.TripId.ToString(), 
            innerKeySelector: r => r.RouteId,             
            resultSelector: (st, r) => r.RouteShortName,  
            joinDbSet: _routeRepository.GetDbSet()
            );

            var routeSorted = routeShortNames.ToList().OrderBy(x=>x).Distinct();

            return Ok(routeShortNames);
        }
        [HttpGet]
        public async Task<IActionResult> GetNearestStopAsync(double? latitude, double? longitude)
        {
            const double EarthRadiusKm = 6371;

            // Haversine Formula в проекции
            var nearestStop = await _stopRepository.GetNearestAsync(
                filter: _ => true, 
                projection: stop => new
                {
                    stopId=stop.StopId,
                    stopName=stop.StopName,
                    Stop = stop,
                    Region = stop.StopArea, 
                    Distance = EarthRadiusKm * 2 * Math.Asin(Math.Sqrt(
                        Math.Pow(Math.Sin((double)((stop.StopLat - latitude) * Math.PI / 180 / 2)), 2) +
                        Math.Cos((double)(latitude * Math.PI / 180)) * Math.Cos((double)(stop.StopLat * Math.PI / 180)) *
                        Math.Pow(Math.Sin((double)((stop.StopLon - longitude) * Math.PI / 180 / 2)), 2)
                    ))
                }
            );

            if (nearestStop == null)
            {
                return NotFound("No stops found.");
            }

            return Ok(new
            {
                StopId = nearestStop.stopId,
                StopName = nearestStop.Stop.StopName,
                Region = nearestStop.Region,
                Latitude = nearestStop.Stop.StopLat,
                Longitude = nearestStop.Stop.StopLon,
                Distance = nearestStop.Distance
            });
        }
    }
}
