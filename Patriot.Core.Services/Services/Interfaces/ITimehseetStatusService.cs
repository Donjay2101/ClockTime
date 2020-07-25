using Patriot.Entities;
using System.Collections.Generic;

namespace Patriot.Core.Services.Services.Interfaces
{
    public interface ITimehseetStatusService
    {
        IEnumerable<TimesheetStatus> GetAll();
        void Insert(TimesheetStatus log);              
    }
}
