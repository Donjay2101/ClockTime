using Patriot.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Patriot.Core.Services.Services.Interfaces
{
    public interface IDailyPuchLogs
    {
        IEnumerable<DailyLog> GetDailyLogs();
        void insert(DailyLog log);
        string GetCurrentStatus();
    }
}
