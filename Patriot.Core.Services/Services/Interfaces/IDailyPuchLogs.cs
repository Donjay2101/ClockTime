using Patriot.Core.Services.ResponseViewModels;
using Patriot.Entities;
using System;
using System.Collections.Generic;

namespace Patriot.Core.Services.Services.Interfaces
{
    public interface IDailyPuchLogs
    {
        IEnumerable<DailyLog> GetDailyLogs(DateTime date);
        int insert(DailyLog log);
        string GetCurrentStatus(DateTime date);
        string SubmitTimeSheet();
        List<DailyLog> GetHistory();
        Timesheets GetLatestTimeSheetRecord();
        BasicResponse Update(DailyLog log, bool isOverNight = false);
        BasicResponse GetTopDurations(DateTime date);
    }
}
