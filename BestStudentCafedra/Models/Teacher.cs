using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace BestStudentCafedra.Models
{
    public partial class Teacher: Person
    {
        public Teacher()
        {
            GraduationWorksAdvice = new HashSet<GraduationWork>();
            GraduationWorksReview = new HashSet<GraduationWork>();
            TeacherDisciplines = new HashSet<TeacherDiscipline>();
            Events = new HashSet<Event>();
        }
        public int Id { get; set; }
        [Required(ErrorMessage = "Не указана должность")]
        [StringLength(100, ErrorMessage = "Должность должна содержать менее 100 символов")]
        [Display(Name = "Должность")]
        public string Post { get; set; }
        [Display(Name = "Ученая степень")]
        public string AcademicDegree { get; set; }

        public virtual ICollection<GraduationWork> GraduationWorksAdvice { get; set; }
        public virtual ICollection<GraduationWork> GraduationWorksReview { get; set; }
        public virtual ICollection<Event> Events { get; set; }
        public virtual ICollection<TeacherDiscipline> TeacherDisciplines { get; set; }
    }
}
