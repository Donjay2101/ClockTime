using Patriot.Core.Repositories;
using Patriot.Core.Repositories.Interfaces;
using Patriot.Data.Context;
using Patriot.Entities;

namespace Patriot.Core.Services.IOW
{
    public class UnitOfWork:IUnitOfWork
    {
        private ApplicationDBContext _dbContext;
        private BaseRepository<DailyLog> _dailyLogs;
        private BaseRepository<Timesheets> _dailypunchLogs;
        private BaseRepository<TimesheetStatus> _timeSheetStatus;

        public UnitOfWork(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IBaseRepository<DailyLog> DailyLogsRepo
        {
            get
            {
                return _dailyLogs ??
                    (_dailyLogs = new BaseRepository<DailyLog>(_dbContext));
            }
        }

        public IBaseRepository<Timesheets> DailyPunchLogsRepo
        {
            get
            {
                return _dailypunchLogs ??
                    (_dailypunchLogs = new BaseRepository<Timesheets>(_dbContext));
            }
        }


        public IBaseRepository<TimesheetStatus> TimesheetStatusRepo
        {
            get
            {
                return _timeSheetStatus ??
                    (_timeSheetStatus = new BaseRepository<TimesheetStatus>(_dbContext));
            }
        }



        public void Commit()
        {
            _dbContext.SaveChanges();
        }
    }
}
