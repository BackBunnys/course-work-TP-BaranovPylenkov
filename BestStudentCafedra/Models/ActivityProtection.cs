using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace BestStudentCafedra.Models
{
    public partial class ActivityProtection
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int ActivityId { get; set; }
        [Display(Name = "Время защиты")]
        public DateTime ProtectionDate { get; set; }
        [Display(Name = "Баллы")]
        public int Points { get; set; }

        public virtual Activity Activity { get; set; }
        public virtual Student Student { get; set; }
    }
}
