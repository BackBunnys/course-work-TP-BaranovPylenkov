using System;
using System.Collections.Generic;
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
        public int Number { get; set; }
        public DateTime CompletionDate { get; set; }

        public virtual SemesterDiscipline SemesterDiscipline { get; set; }

        public virtual ICollection<StudentRating> StudentRatings { get; set; }
    }
}
