using Patriot.Core.Repositories;
using Patriot.Core.Repositories.Interfaces;
using Patriot.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Patriot.Core.Services.IOW
{
    public interface IUnitOfWork
    {
        IBaseRepository<DailyLog> DailyLogsRepo { get; }
        IBaseRepository<DailyPunchLogs> DailyPunchLogsRepo { get; }
        void Commit();
    }
}
