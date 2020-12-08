using System;
using System.Collections.Generic;

#nullable disable

namespace BestStudentCafedra.Models
{
    public partial class Specialty
    {
        public Specialty()
        {
            AcademicGroups = new HashSet<AcademicGroup>();
        }

        public string Code { get; set; }
        public string AcademicDegree { get; set; }
        public string Name { get; set; }

        public virtual ICollection<AcademicGroup> AcademicGroups { get; set; }
    }
}
