using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using Patriot.Core.Services.Services.Interfaces;
using Patriot.Entities;
using Timesheet.Models;

namespace Timesheet.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDailyLogService _service;
        private readonly IDailyPuchLogs _pService;

        public HomeController(ILogger<HomeController> logger,IDailyLogService service, IDailyPuchLogs pService)
        {
            _logger = logger;
            _service = service;
            _pService = pService;
        }

        public IActionResult Index()
        {            
            SetStatus();
            return View();
        }


        public IActionResult Index2()
        {
            Status(); 
            return View();
        }

        private void SetStatus()
        {

            ViewBag.Status = Common.Constants.NoStatus;
            var data = _service.GetCurrentStatus();
            if (data != null)
            {
                ViewBag.Status = data.PunchName;
            }
          
        }


        public void Status()
        {                  
            ViewBag.Status = _pService.GetCurrentStatus();
            
        }




        public async Task<IActionResult> Schedule()
        {
          
            var data =  _service.GetTodayLog();
            return PartialView("Schedule", data);
        }


        public async Task<IActionResult> Schedule1()
        {
            
            var data = _pService.GetDailyLogs();
            return PartialView("Schedule", data);
        }

        [HttpPost]
        public async Task<IActionResult> Schedule1(DailyLog log)
        {

            _pService.insert(log);
            return Json("data inserted successfully");
        }

        [HttpPost]
        public async Task<IActionResult> Schedule(DailyLog log)
        {
              _service.Insert(log);
            return Json("data inserted successfully");
        }

        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
