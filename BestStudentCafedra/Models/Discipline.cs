using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace BestStudentCafedra.Models
{
    public partial class Discipline
    {
        public Discipline()
        {
            TeacherDisciplines = new HashSet<TeacherDiscipline>();
        }

        public int Id { get; set; }
        [Display(Name = "Имя")]
        public string Name { get; set; }

        public virtual ICollection<SemesterDiscipline> SemesterDisciplines { get; set; }
        public virtual ICollection<TeacherDiscipline> TeacherDisciplines { get; set; }
    }
}
