using System;

namespace Patriot.Entities
{
    public  class DailyLog:  BaseEntity, ICloneable
    {
        public string PunchName { get; set; }
        //this property includes both date and time
        public DateTime PunchTime { get; set; }
        public string Description { get; set; }
        public bool IsSubmitted { get; set; }
        public bool IsAutoSplit { get; set; }

        public DateTime PunchDate { get; set; }

        public int GroupID { get; set; }
        // this property include only time
        public string Time { get; set; } 

        public bool IsEdit { get; set; }

        public int SplitLogID { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
