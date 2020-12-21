using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace BestStudentCafedra.Models
{
    public partial class EventLog
    {
        public int Id { get; set; }
        public int GraduationWorkId { get; set; }
        public int EventId { get; set; }
        [Display(Name = "Отметка")]
        public string Mark { get; set; }

        public virtual GraduationWork GraduationWork { get; set; }
        public virtual Event Event { get; set; }
    }
}
