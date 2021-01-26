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
        [Display(Name = "Название")]
        [Required(ErrorMessage = "Не указано название дисцплины")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Название дисциплины должно быть длиной от 5 до 100 символов")]
        public string Name { get; set; }

        public virtual ICollection<GroupDiscipline> GroupDiscipline { get; set; }
        public virtual ICollection<SemesterDiscipline> SemesterDisciplines { get; set; }
        public virtual ICollection<TeacherDiscipline> TeacherDisciplines { get; set; }
    }
}
