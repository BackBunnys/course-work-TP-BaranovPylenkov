using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace BestStudentCafedra.Models
{
    public partial class Activity
    {
        public Activity()
        {
            ActivityProtections = new HashSet<ActivityProtection>();
        }

        public int Id { get; set; }
        [Display(Name = "Тип")]
        public int? TypeId { get; set; }
        [Required(ErrorMessage = "Не указана дисциплина")]
        [Display(Name = "Дисциплина")]
        public int DisciplineId { get; set; }
        [Required(ErrorMessage = "Не указан номер работы")]
        [Range(0, int.MaxValue, ErrorMessage = "Номер должен быть равен или больше 0")]
        [Display(Name = "Номер")]
        public int Number { get; set; }
        [Required(ErrorMessage = "Не указано название")]
        [StringLength(maximumLength: 100, MinimumLength = 5, ErrorMessage = "Название должно содержать от 5 до 100 символов")]
        [Display(Name = "Название")]
        public string Title { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Максимальная оценка должна быть больше 0")]
        [Display(Name = "Максимальная оценка")]
        public int? MaxPoints { get; set; }

        public virtual SemesterDiscipline SemesterDiscipline { get; set; }
        [Display(Name = "Тип")]
        public virtual ActivityType Type { get; set; }
        public virtual ICollection<ActivityProtection> ActivityProtections { get; set; }
    }
}
