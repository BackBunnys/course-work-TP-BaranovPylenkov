using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

#nullable disable

namespace BestStudentCafedra.Models
{
    public partial class GraduationWork
    {
        public GraduationWork()
        {
            AssignedStaffs = new HashSet<AssignedStaff>();
            EventLogs = new HashSet<EventLog>();
        }

        public int Id { get; set; }
        public int StudentId { get; set; }
        [Display(Name = "Тема")]
        public string Theme { get; set; }
        public DateTime? ArchievedDate { get; set; }
        public bool? Result { get; set; }

        public virtual Student Student { get; set; }
        public virtual ICollection<AssignedStaff> AssignedStaffs { get; set; }
        public virtual ICollection<EventLog> EventLogs { get; set; } = new List<EventLog>();

        public bool hasMarkForEvent(Event @event)
        {
            return EventLogs.Any(x => x.EventId == @event.Id);
        }
    }
}
