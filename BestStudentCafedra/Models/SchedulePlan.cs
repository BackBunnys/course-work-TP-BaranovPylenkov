using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BestStudentCafedra.Models
{
    public class SchedulePlan
    {
        public SchedulePlan()
        {
            Events = new HashSet<Event>();
        }
        public int Id { get; set; }

        [Required(ErrorMessage = "Выберите группу")]
        [Range(0, int.MaxValue, ErrorMessage = "Не выбрана группа")]
        [Display(Name = "Группа")]
        public int GroupId { get; set; }
        [Display(Name = "Дата утвреждения")]
        public DateTime? ApprovedDate { get; set; }

        [Display(Name = "Последнее изменение")]
        public DateTime? LastChangedDate { get; set; }

        [Display(Name = "Сотрудник")]
        public string ApprovingOfficerName { get; set; }

        [Display(Name = "Группа")]
        public virtual AcademicGroup Group { get; set; }
        public virtual ICollection<Event> Events { get; set; }

        public void Approve(string officer, DateTime time)
        {
            this.ApprovingOfficerName = officer;
            this.ApprovedDate = time;
            this.LastChangedDate = time;
        }
    }
}
