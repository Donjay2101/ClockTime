using Patriot.Core.Services.Common;
using Patriot.Core.Services.IOW;
using Patriot.Core.Services.ResponseViewModels;
using Patriot.Core.Services.Services.Interfaces;
using Patriot.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;

namespace Patriot.Core.Services.Services
{
    public class DailyPunchLogService: IDailyPuchLogs
    {
        private readonly IUnitOfWork _work;
        private DateTime currentDate = DateTime.Now;
        public DailyPunchLogService(IUnitOfWork work)
        {
            _work = work;
        }

    


        public IEnumerable<DailyLog> GetDailyLogs(DateTime date)
        {
            var punchlogs= DailyLogByStatus(date);
            var logs = ConverttoDailyLogs(punchlogs);
            if (logs == null)
            {
                return new List<DailyLog>();
            }
            var dailylogs = GenrateHistoryToShow(logs);
            return dailylogs;
        }

        public Timesheets DailyPunch(DateTime date)
        {
            var log = _work.DailyPunchLogsRepo.Get(x => x.ClockedIn.Value.Date == date.Date && x.SubmitTime == null , o => o.OrderByDescending(x => x.CreatedDate)).FirstOrDefault();
            return log;
        }

        /// <summary>
        /// this method is get dailypunch log once clockedout is been done and user wants to clock in again
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public Timesheets DailyLogForInsert(DateTime date)
        {
            var log = _work.DailyPunchLogsRepo.Get(x => x.ClockedIn.Value.Date == date.Date && x.ClockedOut ==null, o => o.OrderByDescending(x => x.CreatedDate)).FirstOrDefault();
            return log;
        }

        /// <summary>
        /// this method is get dailypunch log to update the value
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public Timesheets DailyLogForUpdate(DateTime date)
        {
            var log = _work.DailyPunchLogsRepo.Get(x => x.ClockedIn.Value.Date == date.Date, o => o.OrderByDescending(x => x.CreatedDate)).FirstOrDefault();
            return log;
        }

        public IEnumerable<Timesheets> DailyLogByStatus(DateTime date)
        {
            var l = _work.DailyPunchLogsRepo.Get().ToList() ;
            //needd to get information according to user currently no user is present inside our table
            var log = _work.DailyPunchLogsRepo.Get(x => x.SubmitTime == null, o => o.OrderBy(x => x.CreatedDate));
            
            return log;
        }


        public string GetCurrentStatus(DateTime date)
        {
            var log = _work.DailyPunchLogsRepo.Get(x=> x.SubmitTime == null).OrderByDescending(x=>x.CreatedDate).FirstOrDefault();
            if(log !=null && log.ClockedOut.HasValue && log.ClockedOut.Value.Date != date.Date)
            {
                return Constants.OverDue.Replace(" ", "").ToLower() ;
            }
            return GetStatus(log);
        }


        public Timesheets GetLatestTimeSheetRecord()
        {
            var log = _work.DailyPunchLogsRepo.Get(x => x.SubmitTime == null && x.ClockedOut ==null  && !x.IsAutoSplit).OrderByDescending(x => x.CreatedDate).FirstOrDefault();
            return log;
        }
        private string GetStatus(Timesheets log)
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
        
        private List<DailyLog> GetDailyLogs(Timesheets log,int groupID=0)
        {
            List<DailyLog> dailyLogs = new List<DailyLog>();
            if(log.SplitLogId.HasValue && log.SplitLogId.Value>0)
            {
                return null;
            }
            foreach (var column in Constants.Columns)
            {
                var prop= GetPropValue(log, column);
                var value = prop.GetValue(log);

                var idProp = GetPropValue(log, "ID");
                var idVal = idProp.GetValue(log);

                if(prop.Name.ToLower() == Constants.Columns[1].ToLower())
                {
                    var isAutoSplitProp = GetPropValue(log, "IsAutoSplit");
                    var isAutoSplitVal = (bool)isAutoSplitProp.GetValue(log);
                    if (isAutoSplitVal)
                    {
                        var splitLog = _work.DailyPunchLogsRepo.Get(x => x.SplitLogId.HasValue && x.SplitLogId.Value == log.ID).FirstOrDefault();
                        value = splitLog.ClockedOut;
                     
                    }
                }                

                if (value != null)
                {
                    var time = Convert.ToDateTime(value);
                    var dailyLog = new DailyLog
                    {
                        ID = (int)idVal,
                        PunchName = prop.Name,
                        PunchTime = time,
                        Time = time.ToString("HH:mm"),
                        PunchDate = time.Date,
                        GroupID=groupID
                    };
                    dailyLogs.Add(dailyLog);
                }                             
            }
            return dailyLogs;
        }

        private PropertyInfo GetPropValue(Timesheets log,string name="")
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


        private Timesheets SetPropValue(DailyLog log)
        {
            var dailyPunchLog = _work.DailyPunchLogsRepo.GetByID(log.ID);      //DailyLogForInsert(log.PunchTime) ;
            if (dailyPunchLog == null)
                dailyPunchLog = new Timesheets();
            List<DailyLog> dailyLogs = new List<DailyLog>();
            var type = typeof(Timesheets);
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

        public BasicResponse Update(DailyLog log,bool isOverNight=false)
        {           
            

            if (!IsValidDate(log))
            {
                return Common.Common.SendResponse(500, "date entered is not correct");
            }

            var punchlog = SetPropValue(log);
            if (isOverNight)
            {
                punchlog.IsAutoSplit = isOverNight;
            }

            if (!IsOverNight(log))
            {
                if (punchlog.IsAutoSplit)
                {
                    Delete(punchlog.ID);
                    punchlog.SplitLogId = 0;
                    punchlog.IsAutoSplit = false;
                }
                _work.DailyPunchLogsRepo.Update(punchlog);
                _work.Commit();
            }

            return Common.Common.SendResponse(200, "record updated successfully");
        }

        public void Delete(int ID)
        {
            var record = _work.DailyPunchLogsRepo.Get(x => x.SplitLogId == ID).FirstOrDefault();
            if (record != null)
            {
                _work.DailyPunchLogsRepo.Delete(record.ID);
            }
        }
       
        public int insert(DailyLog log)
        {
            var punchlog = SetPropValue(log);
            punchlog.SplitLogId = log.SplitLogID;
            _work.DailyPunchLogsRepo.Insert(punchlog);
            _work.Commit();
             return punchlog.ID;
        }


        private Timesheets SetStatus(Timesheets timesheet, string status)
        {
            var statusRecord = _work.TimesheetStatusRepo.Get(x => x.Name.ToLower() == status.ToLower()).FirstOrDefault();
            if(statusRecord != null)
            {
                timesheet.StatusID = statusRecord.ID;
            }
            return timesheet;
                
        }
        public bool IsOverNight(DailyLog dLog)
        {
            var log = DailyPunch(dLog.PunchTime.AddDays(-1));
            if(log != null && log.SubmitTime == null && !dLog.IsAutoSplit)
            {
                if (log.ClockedIn.Value.Date < dLog.PunchTime.Date)
                {
                    var newLog = (DailyLog)dLog.Clone();
                    var newDate = new DateTime(log.ClockedIn.Value.Date.Year, log.ClockedIn.Value.Date.Month, log.ClockedIn.Value.Date.Day, 23, 59, 59);
                    newLog.PunchTime = newDate;
                    newLog.IsAutoSplit = true;
                    //insert entry to old
                    Update(newLog, true);

                    //clock In entry for current Day
                    var newDayClockIn = new DailyLog
                    {
                        SplitLogID = log.ID,
                        PunchName = "clockedIn",
                        PunchTime = new DateTime(dLog.PunchTime.Date.Year, dLog.PunchTime.Date.Month, dLog.PunchTime.Date.Day, 00, 00, 00),
                        IsAutoSplit = true
                    };
                    var id =  insert(newDayClockIn);

                    //clockout for the day
                    dLog.ID = id;
                    dLog.IsAutoSplit = true;
                    Update(dLog);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
          
        }

        public string SubmitTimeSheet()
        {
            var logs = _work.DailyPunchLogsRepo.Get(x=>x.SubmitTime == null);
            if(logs != null && logs.Count() >0)
            {
                foreach(var log in logs)
                {
                    var currDate = DateTime.Now;
                    log.SubmitTime = currDate;
                    SetStatus(log, Status.Submitted);
                    _work.DailyPunchLogsRepo.Update(log);
                    if (log.IsAutoSplit)
                    {
                        var autoLog = _work.DailyPunchLogsRepo.Get(x => x.SplitLogId.HasValue && x.SplitLogId == log.ID).FirstOrDefault();
                        autoLog.SubmitTime = currDate;
                       
                        _work.DailyPunchLogsRepo.Update(autoLog);
                    }
                    _work.Commit();
                }
                
                return ResponseMessages.TimeSheetSubmitted;
            }
            else
            {
                return ResponseMessages.TimesheetAlreadySubmitted;
            }
        }

        private List<DailyLog> ConverttoDailyLogs(IEnumerable<Timesheets> punchLogs)
        {
            List<DailyLog> logs = new List<DailyLog>();
            if (punchLogs != null)
            {
                
                int count = 1;
                
                foreach (var punchlog in punchLogs)
                {
                    var dailylogs = GetDailyLogs(punchlog, count++);
                    if (dailylogs != null)
                    {
                        logs.AddRange(dailylogs);
                    }
                }
                
                //return logs;
            }
            return logs;
        }


        public List<DailyLog> GetHistory()
        {
            var punchLogs = _work.DailyPunchLogsRepo.Get();
            var logs = ConverttoDailyLogs(punchLogs.OrderBy(x=>x.ClockedIn));
            return GenrateHistoryToShow(logs);           
        }


        /// <summary>
        /// made this to have the row with total
        /// </summary>
        /// <param name="logs"></param>
        /// <returns></returns>
        public List<DailyLog> GenrateHistoryToShow(List<DailyLog> logs)
        {
            var newList = new List<DailyLog>();
            var group = logs.GroupBy(x => x.GroupID).OrderByDescending(x => x.Key).ToList();
            if(group !=null && group.Count > 0)
            {
                foreach(var gr in group)
                {
                    newList.AddRange(gr.OrderBy(x=>x.PunchTime).ToList());

                    var span = GetDuration(gr.ToList(),currentDate);
                    var time = GenerateString(span);
                    var dailyLogTotal = new DailyLog
                    {
                        PunchName = "Total",
                        Time = $"{time}"
                    };
                    newList.Add(dailyLogTotal);

                }
            }
            return newList;
        }

        public BasicResponse GetTopDurations(DateTime date)
        {
            currentDate = date;
            var logs = _work.DailyPunchLogsRepo.Get(x => x.CreatedDate.HasValue && x.CreatedDate.Value.Date == currentDate.Date).OrderByDescending(x => x.CreatedDate);
            var currenthours = GetTotalHours(logs,true);
            var totalHours = GetTotalHours(logs);
            var lunchHours = GetLunchHours(logs);

            var result = $"{currenthours}|{totalHours}|{lunchHours}";

            return new BasicResponse { StatusCode = 200, Result = result };
        }
       
        public string GetTotalHours(IEnumerable<Timesheets> logs, bool isCurrent = false)
        {
          
            TimeSpan totalSpan= new TimeSpan();           
            if(logs !=null && logs.Count()>0)
            {
                foreach(var log in logs)
                {
                    
                    var dailylogs = GetDailyLogs(log);                   
                    totalSpan += GetDuration(dailylogs, currentDate);

                    if (isCurrent) 
                    {
                        break;
                    }
                }
            }

            return GenerateString(totalSpan);
           
        }

        public string GetLunchHours(IEnumerable<Timesheets> logs)
        {
            TimeSpan totalSpan = new TimeSpan();           
            if (logs != null && logs.Count() > 0)
            {
                foreach (var log in logs)
                {

                    var dailylogs = GetDailyLogs(log);
                    totalSpan += GetDuration(dailylogs, currentDate,true);                    
                }
            }
            return GenerateString(totalSpan);
            //return totalSpan;
            
        }

        //public TimeSpan GetDifferencBetweenDate(DateTime startDate, DateTime endDate)
        //{
        //    var span = endDate - startDate;
        //    return span;           
        //}

        public string GenerateString(TimeSpan span)
        {
            string time;
            if (span.Days > 0)
            {
                time = $"{span.Days} days & {span.Hours} hour/s & {span.Minutes} minute/s";
            }
            else if (span.Hours > 0)
            {
                time = $"{span.Hours} hour/s & {span.Minutes} minute/s";
            }
            else
            {
                time = $"{span.Minutes} minute/s";
            }

            return time;
        }

        private TimeSpan GetDuration(List<DailyLog> list,DateTime currentDate,bool isOnlyLunch=false)
        {
            int days, hours, minutes;
            TimeSpan lunchSpan,totalSpan;
            var clockedIn = list.FirstOrDefault(x => x.PunchName.ToLower() == Constants.ClockedIn.Replace(" ", "").ToLower());
            var clockedOut = list.FirstOrDefault(x => x.PunchName.ToLower() == Constants.ClockedOut.Replace(" ", "").ToLower());                       
            if (clockedIn != null && clockedOut != null)
            {
                totalSpan = clockedOut.PunchTime - clockedIn.PunchTime;
                //days = span.Days;
                //hours = span.Hours;
                //minutes = span.Minutes;                                
               // return totalSpan;
                
            }else if(clockedOut == null)
            {
                totalSpan = currentDate - clockedIn.PunchTime;
            }

            var lunchin = list.FirstOrDefault(x => x.PunchName.ToLower() == Constants.Lunch.Replace(" ", "").ToLower());
            var lunchout = list.FirstOrDefault(x => x.PunchName.ToLower() == Constants.Lunchout.Replace(" ", "").ToLower());                     
            if (lunchin != null && lunchout != null)
            {
                lunchSpan = lunchout.PunchTime - lunchin.PunchTime;

            }
            else if (lunchin != null && lunchout ==null)
            {
                lunchSpan = currentDate - lunchin.PunchTime;
            }
            
            if (lunchSpan != null)
            {
                totalSpan = totalSpan - lunchSpan;
            }
            if (isOnlyLunch)
            {
                return lunchSpan;
            }
            return totalSpan;
        }



        public bool IsValidDate(DailyLog dailyLog)
        {
            var log = _work.DailyPunchLogsRepo.GetByID(dailyLog.ID);
            var dailylogs = GetDailyLogs(log, 0);
            if(dailyLog.PunchName.ToLower() == Constants.ClockedOut.Replace(" ", "").ToLower())
            {
                if(dailyLog.PunchTime > DateTime.Now)
                {
                    return false;
                }
                var d = dailylogs.Where(x => x.PunchTime >= dailyLog.PunchTime && x.PunchName.ToLower() != dailyLog.PunchName.ToLower() );
                if(d != null && d.Count()>0)
                {
                    return false;
                }

                var clockedin = dailylogs.FirstOrDefault(x => x.PunchName.ToLower() == Constants.ClockedIn.Replace(" ", "").ToLower());
                if (clockedin != null)
                {
                    var timeSpan = dailyLog.PunchTime - clockedin.PunchTime;
                    if (timeSpan.TotalDays >= 1)
                    {
                        return false;
                    }
                }

            }
            else if(dailyLog.PunchName.ToLower() == Constants.Lunchout.Replace(" ", "").ToLower())
            {
                var d = dailylogs.Where(x => x.PunchTime >= dailyLog.PunchTime  && x.PunchName.ToLower() != Constants.ClockedOut.Replace(" ", "").ToLower() &&  
                x.PunchName.ToLower() != dailyLog.PunchName.ToLower());
                if (d != null && d.Count()>0)
                {
                    return false;
                }

                d = dailylogs.Where(x => x.PunchTime <= dailyLog.PunchTime && x.PunchName.ToLower() != dailyLog.PunchName.ToLower());

                if (d != null && d.Count() > 2)
                {
                    return false;
                }

            }
            else if (dailyLog.PunchName.ToLower() == Constants.Lunch.Replace(" ", "").ToLower())
            {
                //less than condition for date validation .. edit should not be less then checkin
                var d = dailylogs.Where(x => x.PunchName.ToLower() == Constants.ClockedIn.Replace(" ", "").ToLower() && x.PunchTime >= dailyLog.PunchTime);
                if (d != null && d.Count()>0)
                {
                    return false;
                }

                d = dailylogs.Where(x => x.PunchTime <= dailyLog.PunchTime && x.PunchName.ToLower() != dailyLog.PunchName.ToLower());
                if (d != null && d.Count() >1)
                {
                    return false;
                }

            }
            else if(dailyLog.PunchName.ToLower() == Constants.ClockedIn.Replace(" ", "").ToLower())
            {
                var date = LastSubmittedTime();                                                             
                if(date.HasValue && dailyLog.PunchTime < date.Value)
                {
                    return false;
                }
                //logic to check total manhours should not be more than 1 day.. for edit validation
                var clockedout = dailylogs.FirstOrDefault(x => x.PunchName.ToLower() == Constants.ClockedOut.Replace(" ", "").ToLower());
                if (clockedout != null)
                {
                    var timeSpan = clockedout.PunchTime - dailyLog.PunchTime;
                    if (timeSpan.TotalDays >= 1)
                    {
                        return false;
                    }
                }



                //logic for greater date then other bands
                var d = dailylogs.Where(x => x.PunchTime <= dailyLog.PunchTime && x.PunchName.ToLower() != dailyLog.PunchName.ToLower());
                if (d != null && d.Count() > 0)
                {
                    return false;
                }
            }

                
                return true;
        }

        private DateTime? LastSubmittedTime()
        {
            var lastClockOut = _work.DailyPunchLogsRepo.Get().OrderByDescending(x => x.SubmitTime).FirstOrDefault();
            if(lastClockOut != null)
            {
                return lastClockOut.SubmitTime;
            }
            return null;
        }



    }
}
