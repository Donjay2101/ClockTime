using System;
using System.Collections.Generic;
using System.Text;

namespace Patriot.Entities
{
    public class DailyPunchLogs:BaseEntity
    {
        public DateTime? ClockedIn { get; set; }
        public DateTime? ClockedOut { get; set; }
        public DateTime? LunchStart { get; set; }
        public DateTime? LunchEnd { get; set; }

        public bool IsSubmitted { get; set; }
        public bool IsAutoSplit { get; set; }
    }
}
