using System;
using System.Collections.Generic;

#nullable disable

namespace BestStudentCafedra.Models
{
    public partial class Student
    {
        public Student()
        {
            ActivityProtections = new HashSet<ActivityProtection>();
            GraduationWorks = new HashSet<GraduationWork>();
        }

        public int GradebookNumber { get; set; }
        public int GroupId { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }

        public virtual AcademicGroup Group { get; set; }
        public virtual ICollection<ActivityProtection> ActivityProtections { get; set; }
        public virtual ICollection<GraduationWork> GraduationWorks { get; set; }
    }
}
