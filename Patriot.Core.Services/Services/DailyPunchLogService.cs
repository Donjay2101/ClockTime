using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;
using Patriot.Core.Services.IOW;
using Patriot.Core.Services.Services.Interfaces;
using Patriot.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Patriot.Core.Services.Services
{
    public class DailyPunchLogService: IDailyPuchLogs
    {
        private readonly IUnitOfWork _work;
        public DailyPunchLogService(IUnitOfWork work)
        {
            _work = work;
        }

        private string[] columns = new string[] { "clockedin", "Clockedout", "lunchstart", "lunchend" }; 


        public IEnumerable<DailyLog> GetDailyLogs()
        {
            var log = DailyPunch();
            var dailylogs = GetPropValue(log);
            return dailylogs;
        }

        public DailyPunchLogs DailyPunch()
        {
            var log = _work.DailyPunchLogsRepo.Get(x => x.ClockedIn.Value.Date == DateTime.Now.Date, o => o.OrderByDescending(x => x.CreatedDate)).FirstOrDefault();
            return log;
        }

        private List<DailyLog> GetPropValue(DailyPunchLogs log )
        {
            if(log == null)
            {
                return new List<DailyLog>() ;
            }
            List<DailyLog> dailyLogs = new List<DailyLog>();
            var type = log.GetType();
            var properties = type.GetProperties();
            foreach(var prop in properties)
            {
                if(columns.Contains(prop.Name.ToLower()))
                {
                    if(prop.GetValue(log) !=null)
                    {
                        var dailylog = new DailyLog
                        {
                            PunchName = prop.Name,
                            PunchTime = Convert.ToDateTime(prop.GetValue(log))
                        };
                        dailyLogs.Add(dailylog);
                    }
                    
                }
            }
            return dailyLogs;
        }


        private DailyPunchLogs SetPropValue(DailyLog log)
        {
            var dailyPunchLog = DailyPunch();
            if (dailyPunchLog == null)
                dailyPunchLog = new DailyPunchLogs();
            List<DailyLog> dailyLogs = new List<DailyLog>();
            var type = typeof(DailyPunchLogs);
            var properties = type.GetProperties();
            var propName = log.PunchName.Replace(" ", "").ToLower();
            foreach (var prop in properties)
            {
                if (prop.Name.ToLower() == propName)
                {
                    prop.SetValue(dailyPunchLog, log.PunchTime);
                }
            }
            return dailyPunchLog;
        }


        public void insert(DailyLog log)
        {
            var logs = DailyPunch();
            var punchlog = SetPropValue(log);
            if (logs != null && logs.ClockedOut == null)
            {
                
                _work.DailyPunchLogsRepo.Update(punchlog);
                _work.Commit();
            }
            else
            {
                punchlog.ID = Common.Common.GenerateID();
                _work.DailyPunchLogsRepo.Insert(punchlog);
                _work.Commit();
            }
        }

        //public IEnumerable<DailyPunchLogs> GetTodayLog()
        //{
        //    var logs = _work.DailyPunchLogsRepo.Get(x => x.PunchTime.Date == DateTime.Now.Date, o => o.OrderByDescending(e => e.PunchTime));
        //    return logs;
        //}



        //public IEnumerable<DailyPunchLogs> GetTodayLog()
        //{
        //    var logs = _work.DailyPunchLogsRepo.Get(x => x.PunchTime.Date == DateTime.Now.Date, o => o.OrderByDescending(e => e.PunchTime));
        //    return logs;
        //}




    }
}
