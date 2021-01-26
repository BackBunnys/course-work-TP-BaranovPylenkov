using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace BestStudentCafedra.Models
{
    public partial class Event
    {
        public Event()
        {
            EventLogs = new HashSet<EventLog>();
        }

        public int Id { get; set; }
        public int SchedulePlanId { get; set; }
        [Required(ErrorMessage = "Не указано содержание мероприятия")]
        [StringLength(maximumLength: 150, MinimumLength = 5, ErrorMessage = "Описание мероприятия должно содержать от 5 до 150 символов")]
        [Display(Name = "Описание мероприятия")]
        public string EventDescription { get; set; }

        [Display(Name = "Дата")]
        public DateTime? Date { get; set; }

        [Display(Name = "Аудитория")]
        public string Class { get; set; }

        [Display(Name = "Ответственный преподаватель")]
        public int? ResponsibleTeacherId { get; set; }

        [Display(Name = "План-график")]
        public virtual SchedulePlan SchedulePlan { get; set; }

        [Display(Name = "Ответственный преподаватель")]
        public virtual Teacher ResponsibleTeacher { get; set; }
        public virtual ICollection<EventLog> EventLogs { get; set; }
    }
}
