using System;
using System.Collections.Generic;

#nullable disable

namespace BestStudentCafedra.Models
{
    public partial class AcademicGroup
    {
        public AcademicGroup()
        {
            SchedulePlans = new HashSet<SchedulePlan>();
            Students = new HashSet<Student>();
        }

        public int Id { get; set; }
        public string SpecialtyId { get; set; }
        public string Name { get; set; }
        public int? FormationYear { get; set; }

        public virtual Specialty Specialty { get; set; }
        public virtual ICollection<SchedulePlan> SchedulePlans { get; set; }
        public virtual ICollection<Student> Students { get; set; }
    }
}
