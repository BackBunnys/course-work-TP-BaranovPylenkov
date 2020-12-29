using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BestStudentCafedra.Models
{
    public class SemesterDiscipline
    {
        public SemesterDiscipline()
        {
            Activities = new HashSet<Activity>();
            RatingControls = new HashSet<RatingControl>();
        }

        public int Id { get; set; }
        public int DisciplineId { get; set; }
        [Display(Name = "Курс")]
        [Required(ErrorMessage = "Не указан учебный курс")]
        [Range(1, 6, ErrorMessage = "Курс должен быть в диапозоне от 1 до 6")]
        public int Year { get; set; }
        [Display(Name = "Семестр")]
        [Required(ErrorMessage = "Не указан семестр")]
        [Range(1, 12, ErrorMessage = "Семестр должен быть в диапозоне от 1 до 12")]
        public int Semester { get; set; }
        [UIHint("Enum")]
        [EnumDataType(typeof(ControlType))]
        [Display(Name = "Тип контроля")]
        public ControlType ControlType { get; set; }

        public virtual Discipline Discipline { get; set; }
        public virtual ICollection<Activity> Activities { get; set; }
        public virtual ICollection<RatingControl> RatingControls { get; set; }
    }

    public enum ControlType
    {
        [Display(Name = "Экзамен")]
        Exam,
        [Display(Name = "Дифференцированный зачет")]
        DifferentialCredit,
        [Display(Name = "Зачет")]
        Credit
    }
}
