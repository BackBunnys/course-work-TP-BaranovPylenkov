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
        }

        public int Id { get; set; }
        public int DisciplineId { get; set; }
        [Display(Name = "Курс")]
        public int Year { get; set; }
        [Display(Name = "Семестр")]
        public int Semester { get; set; }
        [UIHint("Enum")]
        [EnumDataType(typeof(ControlType))]
        [Required(ErrorMessage = "Please select supporter tier")]
        [Display(Name = "Тип контроля")]
        public ControlType ControlType { get; set; }

        public virtual Discipline Discipline { get; set; }
        public virtual ICollection<Activity> Activities { get; set; }
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
