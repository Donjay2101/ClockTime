using Patriot.Core.Repositories.Interfaces;
using Patriot.Entities;

namespace Patriot.Core.Services.IOW
{
    public interface IUnitOfWork
    {
        IBaseRepository<DailyLog> DailyLogsRepo { get; }
        IBaseRepository<Timesheets> DailyPunchLogsRepo { get; }
        IBaseRepository<TimesheetStatus> TimesheetStatusRepo { get; }
        void Commit();
    }
}
