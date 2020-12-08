using System;
using System.Collections.Generic;

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
        public string Theme { get; set; }
        public DateTime? ArchievedDate { get; set; }
        public bool? Result { get; set; }

        public virtual Student Student { get; set; }
        public virtual ICollection<AssignedStaff> AssignedStaffs { get; set; }
        public virtual ICollection<EventLog> EventLogs { get; set; }
    }
}
