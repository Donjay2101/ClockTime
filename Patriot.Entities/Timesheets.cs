using System;

namespace Patriot.Entities
{
    public class Timesheets:BaseEntity
    {
        public DateTime? ClockedIn { get; set; }
        public DateTime? ClockedOut { get; set; }
        public DateTime? LunchStart { get; set; }
        public DateTime? LunchEnd { get; set; }
        public DateTime? SubmitTime { get; set; }

        public int StatusID { get; set; }
        public bool IsAutoSplit { get; set; }
        public int? SplitLogId { get; set; }
        
    }
}
