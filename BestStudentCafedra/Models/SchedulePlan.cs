using System;
using System.Collections.Generic;

#nullable disable

namespace BestStudentCafedra.Models
{
    public partial class SchedulePlan
    {
        public SchedulePlan()
        {
            EventLogs = new HashSet<EventLog>();
        }

        public int Id { get; set; }
        public int GroupId { get; set; }
        public int EventId { get; set; }
        public DateTime? Date { get; set; }
        public string Class { get; set; }
        public int? ResponsibleTeacherId { get; set; }

        public virtual Event Event { get; set; }
        public virtual AcademicGroup Group { get; set; }
        public virtual Teacher ResponsibleTeacher { get; set; }
        public virtual ICollection<EventLog> EventLogs { get; set; }
    }
}
