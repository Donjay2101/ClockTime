using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;
using Patriot.Core.Services.Common;
using Patriot.Core.Services.IOW;
using Patriot.Core.Services.Services.Interfaces;
using Patriot.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

    


        public IEnumerable<DailyLog> GetDailyLogs()
        {
            var log = DailyPunch();
            var dailylogs = GetDailyLogs(log).OrderByDescending(x=>x.PunchTime);
            return dailylogs;
        }

        public DailyPunchLogs DailyPunch()
        {
            var log = _work.DailyPunchLogsRepo.Get(x => x.ClockedIn.Value.Date == DateTime.Now.Date, o => o.OrderByDescending(x => x.CreatedDate)).FirstOrDefault();
            return log;
        }


        public string GetCurrentStatus()
        {
            var log = _work.DailyPunchLogsRepo.Get(x=>x.CreatedDate.Value.Date == DateTime.Now.Date && !x.IsSubmitted).OrderByDescending(x=>x.CreatedDate).FirstOrDefault();
            return GetStatus(log);
        }

        private string GetStatus(DailyPunchLogs log)
        {
            string status = Common.Constants.NoStatus;
            if (log == null)
            {
                return status;
            }
            DateTime date = DateTime.MinValue;
            foreach(var column in Constants.Columns)
            {
                var prop = GetPropValue(log, column);
                var value = prop.GetValue(log);
                if (value != null)
                {
                    var newDate = Convert.ToDateTime(value);
                    if (newDate > date)
                    {
                        date = newDate;
                        status = prop.Name;
                    }
                }
            }
            return status;
        }
        
        private List<DailyLog> GetDailyLogs(DailyPunchLogs log)
        {
            List<DailyLog> dailyLogs = new List<DailyLog>();
            foreach (var column in Constants.Columns)
            {
                var prop= GetPropValue(log, column);
                var value = prop.GetValue(log);
                if (value != null)
                {
                    var dailyLog = new DailyLog
                    {
                        PunchName = prop.Name,
                        PunchTime = Convert.ToDateTime(value)
                  
                    };
                    dailyLogs.Add(dailyLog);
                }                             
            }
            return dailyLogs;
        }

        private PropertyInfo GetPropValue(DailyPunchLogs log,string name="")
        {
            if(log != null)
            {
                var type = log.GetType();
                var properties = type.GetProperties();
                foreach (var prop in properties)
                {
                    if (prop.Name.Replace(" ", "").ToLower() == name.ToLower())
                    {
                        return prop;
                        // break; 
                    }
                }
            }                        
            return null;
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
                if (prop.Name.Replace(" ","").ToLower() == propName)
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
