using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
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
                selector: r => new {stopId=r.StopId, Name = r.StopName }
                );
            var stopsDistinct = stops.DistinctBy(r => r.Name).ToList();
            return Ok(stopsDistinct);
        }
        [HttpPost]
        public async Task<IActionResult> GetRouteShortNamesAsync([FromBody] string stopName)
        {

            var stopExists = await _stopRepository.GetFilteredAsync(
                filter: s => s.StopName == stopName,
                selector: s => s.StopId
            );
            int stopId = stopExists.FirstOrDefault();
            var routeExists = await _stopTimeRepository.GetJoinedFilteredAsync<Trip, int, string>(
            filter: st => stopExists.Contains(st.StopId),  // Фильтруем по StopId
            outerKeySelector: st => st.TripId,               // Соединяем по TripId в StopTime
            innerKeySelector: t => t.TripId,                 // Соединяем по TripId в Trip
            resultSelector: (st, t) => t.RouteId,             // Мы просто возвращаем TripId
            joinDbSet: _tripRepository.GetDbSet()            // Соединяем с таблицей Trip
            );


            var routeShortNames = await _tripRepository.GetJoinedFilteredAsync<Models.Route, string, Dictionary<string, string>>(
            filter: t => routeExists.Contains(t.RouteId),
            outerKeySelector: t => t.RouteId,
            innerKeySelector: r => r.RouteId,
            resultSelector: (t, r) => new Dictionary<string, string>()
            {
                {r.RouteId, r.RouteShortName }
            },
            joinDbSet: _routeRepository.GetDbSet()
            );
            var dict = routeShortNames.ToList();
            Dictionary<string, string> Routes = new Dictionary<string, string>();
            foreach (var i in dict)
            {
                foreach (var j in i.Keys)
                {
                    if (!Routes.ContainsKey(j))
                    {
                        Routes.Add(j, i[j]);
                    }
                }

                Routes = Routes.OrderBy(kvp => kvp.Value).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            }
            return Json(new
            {
                Routes = Routes,
                stopId = stopId,
            });
        }
            
            
        [HttpGet]
        public async Task<IActionResult> GetNearestStopAsync(double? latitude, double? longitude)
        {
            var stops = await _stopRepository.GetQuerableAsync(0);

            if (stops == null || !stops.Any())
            {
                return NotFound("No stops found.");
            }

            return Ok(stops.Select(stop => new
            {
                stop.StopId,
                stop.StopName,
                stop.StopArea,
                StopLat = stop.StopLat.ToString().Replace('.', ','),
                StopLon = stop.StopLon.ToString().Replace('.', ','),
                Distance=0
            }));         
        }


        [HttpGet]
        public async Task<IActionResult> GetBusArrivalsWithDirection(string busNumber, string routeId,string time,int stopId)
        {
            TimeSpan tSpan = TimeSpan.Parse(time);
    
            if (string.IsNullOrWhiteSpace(busNumber))
            {
                return BadRequest("Bus number is required.");
            }
            try
            {
                var arrivals = await _stopTimeRepository.GetFilteredAsync(
                    filter: r => r.StopId==stopId,
                    selector: r => new
                    {
                        ArrivalTime=TimeSpan.Parse(r.ArrivalTime),
                        r.TripId
                    }
                );

                var nearArrivals = arrivals.Where(ar => ar.ArrivalTime >= tSpan)
                    .ToList();

                List<int> trips = nearArrivals.Select(t => t.TripId).ToList();

                var direction = await _tripRepository.GetFilteredAsync(
                    filter: t=>t.RouteId==routeId,
                    selector: t => new
                    {
                       t.TripId,
                       t.TripLongName
                    }
                );                
                List<ArrivalsModel> models = new List<ArrivalsModel>();
                foreach(var i in nearArrivals)
                {
                    foreach(var j in direction)
                    {
                        if(i.TripId == j.TripId)
                        {
                            ArrivalsModel model = new ArrivalsModel();
                            {
                                model.Bus = busNumber;
                                model.Arrivals = i.ArrivalTime.ToString();
                                model.TripName = j.TripLongName;
                            }
                            models.Add(model);
                        }
                    }
                }
                return Ok(models.OrderBy(r=>r.Arrivals).Take(5));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching bus arrivals.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

    }
}
