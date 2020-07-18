using Microsoft.EntityFrameworkCore;
using Patriot.Entities;

namespace Patriot.Data.Context
{
    public  class ApplicationDBContext:DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options):base(options)
        {

        }

        public DbSet<DailyLog> DailyLogs { get; set; }

        public DbSet<DailyPunchLogs> DailyPunchLogs { get; set; }

    }
}
