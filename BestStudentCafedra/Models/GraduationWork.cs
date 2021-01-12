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
            EventLogs = new HashSet<EventLog>();
            TeacherRequests = new HashSet<TeacherRequest>();
            ThemeRequests = new HashSet<ThemeRequest>();
        }

        public int Id { get; set; }
        public int StudentId { get; set; }

        public int? ScientificAdviserId { get; set; }
        public int? ReviewerId { get; set; }

        [Display(Name = "Тема")]
        public string Theme { get; set; }
        public DateTime? ArchievedDate { get; set; }
        public bool? Result { get; set; }

        public virtual Student Student { get; set; }
        public virtual Teacher ScientificAdviser { get; set; }
        public virtual Teacher Reviewer { get; set; }
        public virtual ICollection<EventLog> EventLogs { get; set; }
        public virtual ICollection<TeacherRequest> TeacherRequests { get; set; }
        public virtual ICollection<ThemeRequest> ThemeRequests { get; set; }

        public bool hasMarkForEvent(Event @event)
        {
            return EventLogs.Any(x => x.EventId == @event.Id);
        }
    }
}
