using System;
using System.Collections.Generic;

#nullable disable

namespace BestStudentCafedra.Models
{
    public partial class SchedulePlanEvent
    {
        public SchedulePlanEvent()
        {
            EventLogs = new HashSet<EventLog>();
        }

        public int Id { get; set; }
        public int SchedulePlanId { get; set; }
        public int EventId { get; set; }
        public DateTime? Date { get; set; }
        public string Class { get; set; }
        public int? ResponsibleTeacherId { get; set; }

        public virtual SchedulePlan SchedulePlan { get; set; }
        public virtual Event Event { get; set; }
        public virtual Teacher ResponsibleTeacher { get; set; }
        public virtual ICollection<EventLog> EventLogs { get; set; }
    }
}
