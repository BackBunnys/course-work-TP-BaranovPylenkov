using System;
using System.Collections.Generic;

#nullable disable

namespace BestStudentCafedra.Models
{
    public partial class Discipline
    {
        public Discipline()
        {
            Activities = new HashSet<Activity>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int Year { get; set; }
        public int Semester { get; set; }
        public string ControlType { get; set; }

        public virtual ICollection<Activity> Activities { get; set; }
    }
}
