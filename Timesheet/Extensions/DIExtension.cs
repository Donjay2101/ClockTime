using Microsoft.Extensions.DependencyInjection;
using Patriot.Core.Services.IOW;
using Patriot.Core.Services.Services;
using Patriot.Core.Services.Services.Interfaces;

namespace Timesheet.Extensions
{
    public static class DIExtension
    {
        public static void AddDependencies(this IServiceCollection services)
        {
            services.AddTransient<IDailyLogService, DailyLogService>();
            services.AddTransient<IDailyPuchLogs, DailyPunchLogService>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
        }
    }
}
