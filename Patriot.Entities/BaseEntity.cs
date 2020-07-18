using System;
using System.Collections.Generic;
using System.Text;

namespace Patriot.Entities
{
    public class BaseEntity
    {
        public string ID { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }

    }
}
