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
        private BaseRepository<DailyPunchLogs> _dailypunchLogs;

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



        public IBaseRepository<DailyPunchLogs> DailyPunchLogsRepo
        {
            get
            {
                return _dailypunchLogs ??
                    (_dailypunchLogs = new BaseRepository<DailyPunchLogs>(_dbContext));
            }
        }


        public void Commit()
        {
            _dbContext.SaveChanges();
        }
    }
}
