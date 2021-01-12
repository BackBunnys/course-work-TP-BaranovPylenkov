using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BestStudentCafedra.Models
{
    public abstract class Request
    {
        public Status? Status { get; set; }
        public string RejectReason { get; set; }
        public DateTime CreatingDate { get; set; } = DateTime.Now;
        public DateTime ResponseDate { get; set; }
        public string ResponsePersonName { get; set; }

        public virtual void Approve(Person approvingPerson)
        {
            Response(Models.Status.APPROVED, approvingPerson);
        }

        public void Reject(Person rejectingPerson, string reason)
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

    public enum Status { APPROVED, REJECTED };
}
