using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
        [Required(ErrorMessage = "Не указано название группы")]
        [Display(Name = "Название группы")]
        public string Name { get; set; }
        [Range(1000, 9999, ErrorMessage = "Год формирования группы должен быть в диапозоне от 1000 до 9999")]
        [Display(Name = "Год формирования группы")]
        public int? FormationYear { get; set; }

        [Display(Name = "Специальность")]
        public virtual Specialty Specialty { get; set; }
        public virtual ICollection<SchedulePlan> SchedulePlans { get; set; }
        [Display(Name = "Студенты")]
        public virtual ICollection<Student> Students { get; set; }
    }
}
