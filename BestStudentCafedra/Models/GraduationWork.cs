using System;
using System.Collections.Generic;

#nullable disable

namespace BestStudentCafedra.Models
{
    public partial class GraduationWork
    {
        public GraduationWork()
        {
            EventLogs = new HashSet<EventLog>();
        }

        public int Id { get; set; }
        public int StudentId { get; set; }
        public int? ScientificAdviserId { get; set; }
        public int? ReviewerId { get; set; }
        public string Theme { get; set; }
        public DateTime? ArchievedDate { get; set; }
        public bool? Result { get; set; }

        public virtual Student Student { get; set; }
        public virtual Teacher ScientificAdviser { get; set; }
        public virtual Teacher Reviewer { get; set; }
        public virtual ICollection<EventLog> EventLogs { get; set; }
    }
}
