using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

#nullable disable

namespace BestStudentCafedra.Models
{
    public partial class GraduationWork
    {
        public GraduationWork()
        {
            EventLogs = new HashSet<EventLog>();
            TeacherRequests = new HashSet<TeacherRequest>();
            ThemeRequests = new HashSet<ThemeRequest>();
        }

        public int Id { get; set; }
        [Display(Name = "Студент")]
        [Required(ErrorMessage = "Не указан студент")]
        public int StudentId { get; set; }
        [Display(Name = "Научный руководитель")]
        public int? ScientificAdviserId { get; set; }
        [Display(Name = "Рецензент")]
        public int? ReviewerId { get; set; }

        [Display(Name = "Тема")]
        public string Theme { get; set; }
        [Display(Name = "Дата сдачи")]
        [DataType(DataType.Date)]
        public DateTime? ArchievedDate { get; set; }
        [Display(Name = "Результат")]
        public int? Result { get; set; }

        [Display(Name = "Студент")]
        public virtual Student Student { get; set; }
        public virtual Teacher ScientificAdviser { get; set; }
        public virtual Teacher Reviewer { get; set; }
        public virtual ICollection<EventLog> EventLogs { get; set; }
        public virtual ICollection<TeacherRequest> TeacherRequests { get; set; }
        public virtual ICollection<ThemeRequest> ThemeRequests { get; set; }

        public void Archive(int result, DateTime date)
        {
            Result = result;
            ArchievedDate = date;
        }

        public bool hasMarkForEvent(Event @event)
        {
            return EventLogs.Any(x => x.EventId == @event.Id);
        }
    }
}
