using Microsoft.Extensions.Logging;
using Patriot.Core.Services.IOW;
using Patriot.Core.Services.Services.Interfaces;
using Patriot.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Patriot.Core.Services.Services
{
    public class DailyLogService : IDailyLogService
    {
        private readonly IUnitOfWork _work;
        public DailyLogService(IUnitOfWork work)
        {
            _work = work;
        }
        public IEnumerable<DailyLog> GetAll()
        {
            return _work.DailyLogsRepo.Get();
        }

        public void Insert(DailyLog log)
        {
            //log.ID = Common.Common.GenerateID();
            _work.DailyLogsRepo.Insert(log);
            _work.Commit();
        }

        public IEnumerable<DailyLog>  GetTodayLog()
        {
            var logs= _work.DailyLogsRepo.Get(x => x.PunchTime.Date == DateTime.Now.Date, o => o.OrderByDescending(e => e.PunchTime));
            return logs;
        }


        public DailyLog GetCurrentStatus()
        {
            var log = _work.DailyLogsRepo.Get(x => x.PunchTime.Date == DateTime.Now.Date, o => o.OrderByDescending(e => e.PunchTime)).FirstOrDefault();
            return log;
        }
    }
}
