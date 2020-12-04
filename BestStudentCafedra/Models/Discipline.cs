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
            Activities = new HashSet<Activity>();
            TeacherDisciplines = new HashSet<TeacherDiscipline>();
        }

        public int Id { get; set; }
        [Display(Name = "Имя")]
        public string Name { get; set; }
        [Display(Name = "Курс")]
        public int Year { get; set; }
        [Display(Name = "Семестр")]
        public int Semester { get; set; }
        [Display(Name = "Тип контроля")]
        public string ControlType { get; set; }

        public virtual ICollection<Activity> Activities { get; set; }
        public virtual ICollection<TeacherDiscipline> TeacherDisciplines { get; set; }
    }
}
