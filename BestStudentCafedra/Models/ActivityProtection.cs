using System;
using System.Collections.Generic;

#nullable disable

namespace BestStudentCafedra.Models
{
    public partial class ActivityProtection
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int ActivityId { get; set; }
        public DateTime ProtectionDate { get; set; }
        public float Points { get; set; }

        public virtual Activity Activity { get; set; }
        public virtual Student Student { get; set; }
    }
}
