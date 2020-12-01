using System;
using System.Collections.Generic;

#nullable disable

namespace BestStudentCafedra.Models
{
    public partial class Teacher
    {
        public Teacher()
        {
            AssignedStaffs = new HashSet<AssignedStaff>();
            SchedulePlans = new HashSet<SchedulePlan>();
        }

        public int Id { get; set; }
        public string FullName { get; set; }
        public string Post { get; set; }
        public string AcademicDegree { get; set; }

        public virtual ICollection<AssignedStaff> AssignedStaffs { get; set; }
        public virtual ICollection<SchedulePlan> SchedulePlans { get; set; }
    }
}
