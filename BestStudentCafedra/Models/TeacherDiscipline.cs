using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BestStudentCafedra.Models
{
    public class TeacherDiscipline
    {
        public int Id { get; set; }
        public int TeacherId { get; set; }
        [Display(Name = "Дисциплина")]
        public int DisciplineId { get; set; }

        public virtual Teacher Teacher { get; set; }
        public virtual Discipline Discipline { get; set; }
    }
}
