using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BestStudentCafedra.Models
{
    public abstract class Request
    {
        [Display(Name="Статус")]
        public Status? Status { get; set; }
        [Display(Name = "Причина отказа")]
        [StringLength(500, ErrorMessage = "Длина мотивации не должна превышать 500 символов.")]
        public string RejectReason { get; set; }
        [Display(Name = "Дата создания")]
        [DisplayFormat(DataFormatString = "{0:g}", ApplyFormatInEditMode = true)]
        public DateTime CreatingDate { get; set; } = DateTime.Now;
        [Display(Name = "Дата ответа")]
        [DisplayFormat(DataFormatString = "{0:g}", ApplyFormatInEditMode = true)]
        public DateTime? ResponseDate { get; set; }
        [Display(Name = "Имя ответившего сотрудника")]
        public string ResponsePersonName { get; set; }

        public virtual void Approve(Person approvingPerson)
        {
            Response(Models.Status.APPROVED, approvingPerson);
        }

        public virtual void Reject(Person rejectingPerson, string reason)
        {
            RejectReason = reason;
            Response(Models.Status.REJECTED, rejectingPerson);
        }

        private void Response(Status status, Person responsePerson)
        {
            Status = status;
            ResponseDate = DateTime.Now;
            ResponsePersonName = responsePerson.FullName;
        }
    }

    public enum Status 
    {
        [Display(Name = "Принят")]
        APPROVED,
        [Display(Name = "Отклонён")]
        REJECTED 
    };
}
