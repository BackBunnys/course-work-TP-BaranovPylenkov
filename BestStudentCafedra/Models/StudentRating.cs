using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BestStudentCafedra.Models
{
    public class StudentRating
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int RatingId { get; set; }
        public float Points { get; set; }

        public virtual Student Student { get; set; }
        public virtual RatingControl RatingControl { get; set; }
    }
}
