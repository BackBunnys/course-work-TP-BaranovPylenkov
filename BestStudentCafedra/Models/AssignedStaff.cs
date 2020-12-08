using System;
using System.Collections.Generic;

#nullable disable

namespace BestStudentCafedra.Models
{
    public partial class AssignedStaff
    {
        public int Id { get; set; }
        public int GraduationWorkId { get; set; }
        public int TeacherId { get; set; }
        public string Type { get; set; }

        public virtual GraduationWork GraduationWork { get; set; }
        public virtual Teacher Teacher { get; set; }
    }
}
