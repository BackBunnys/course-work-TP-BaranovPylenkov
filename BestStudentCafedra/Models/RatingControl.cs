using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BestStudentCafedra.Models
{
    public class RatingControl
    {
        public RatingControl()
        {
            StudentRatings = new HashSet<StudentRating>();
        }

        public int Id { get; set; }
        public int SemesterDisciplineId { get; set; }
        public int GroupId { get; set; }
        [Display(Name = "Номер")]
        public int Number { get; set; }
        [Display(Name = "Дата формирования")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime CompletionDate { get; set; }

        public virtual SemesterDiscipline SemesterDiscipline { get; set; }
        public virtual AcademicGroup AcademicGroup { get; set; }

        public virtual ICollection<StudentRating> StudentRatings { get; set; }
    }
}
