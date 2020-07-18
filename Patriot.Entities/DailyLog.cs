using System;

namespace Patriot.Entities
{
    public  class DailyLog: BaseEntity
    {
        public string PunchName { get; set; }
        public DateTime PunchTime { get; set; }
        public string Description { get; set; }
    }
}
