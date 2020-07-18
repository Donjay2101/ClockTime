using Patriot.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Patriot.Core.Services.Services.Interfaces
{
    public interface IDailyLogService
    {
        IEnumerable<DailyLog> GetAll();
        void Insert(DailyLog log);
        DailyLog GetCurrentStatus();

        IEnumerable<DailyLog> GetTodayLog();
    }
}
