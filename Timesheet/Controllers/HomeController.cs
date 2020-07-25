using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.Extensions.Logging;
using Patriot.Core.Services.Common;
using Patriot.Core.Services.Services.Interfaces;
using Patriot.Entities;
using Timesheet.Extensions;
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
            CurrentStatus(); 
            return View();
        }

   

        public void CurrentStatus()
        {
            var latestrecord = _pService.GetLatestTimeSheetRecord();
            if(latestrecord != null)
            {
                HttpContext.Session.Set<int>("LogID", latestrecord.ID);
            }
            ViewBag.Status = _pService.GetCurrentStatus(DateTime.Now);
            
        }


        public IActionResult DailySchedule()
        {            
            var data = _pService.GetDailyLogs(DateTime.Now);
            return PartialView("Schedule", data);
        }

        [HttpPost]
        public JsonResult InsertLog(DailyLog log)
        {
            int logID = HttpContext.Session.Get<int>("LogID");
            if (logID > 0)
            {
                log.ID = logID;
                _pService.Update(log);
                if(log.PunchName.ToLower() == Constants.ClockedOut.Replace(" ", "").ToLower())
                {
                    HttpContext.Session.Set<int>("LogID",0);
                }
            }
            else
            {
                
                logID = _pService.insert(log);
                HttpContext.Session.Set("LogID", logID);
            }
            
            
            return Json("data inserted successfully");
        }

      

        public JsonResult GetHours()
        {
            return Json(_pService.GetTopDurations(DateTime.Now));
        }

        [HttpPost]
        public  IActionResult SubmitTimesheet()
        {
            var response = _pService.SubmitTimeSheet();
            return Json(response);
        }

        public IActionResult History()
        {
            var data = _pService.GetHistory();
            return View(data);
        }

        [HttpPost]
        public JsonResult EditLog(DailyLog dailyLog)
        {         
            var response=_pService.Update(dailyLog);
            return Json(response);
        }


     


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
