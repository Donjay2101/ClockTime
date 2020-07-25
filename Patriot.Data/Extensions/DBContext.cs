using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Patriot.Data.Context;

namespace Patriot.Data.Extensions
{
    public static class DBContext
    {
        public static void AddContext(this IServiceCollection services,IConfiguration configuration)
        {
            //application DbContext
            var section = configuration.GetSection("DbConfig:ConnectionString").Value;


            services.AddDbContext<ApplicationDBContext>(options => options.UseSqlServer(section),ServiceLifetime.Transient);
        }
    }
}
