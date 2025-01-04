using System.Diagnostics;
using System.Net;
using Esti_bus_project.IRepository;
using Esti_bus_project.Models;
using Microsoft.AspNetCore.Mvc;

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
        // GET: Authors/Details/5
        public async Task<IActionResult> Details(int id)
        {
            return View();
        }

        /*[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return (new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        */
    }
}
