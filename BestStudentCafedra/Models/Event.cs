using System;
using System.Collections.Generic;

#nullable disable

namespace BestStudentCafedra.Models
{
    public partial class Event
    {
        public Event()
        {
            SchedulePlans = new HashSet<SchedulePlanEvent>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<SchedulePlanEvent> SchedulePlans { get; set; }
    }
}
