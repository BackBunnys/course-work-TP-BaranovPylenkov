using System;
using System.Collections.Generic;

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
        public int? TypeId { get; set; }
        public int DisciplineId { get; set; }
        public int Number { get; set; }
        public string Title { get; set; }
        public int? MaxPoints { get; set; }

        public virtual SemesterDiscipline SemesterDiscipline { get; set; }
        public virtual ActivityType Type { get; set; }
        public virtual ICollection<ActivityProtection> ActivityProtections { get; set; }
    }
}
