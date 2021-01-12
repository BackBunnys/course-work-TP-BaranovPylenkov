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
        public DateTime CreatingDate { get; set; }
        public DateTime ResponseDate { get; set; }

        public Request()
        {
            CreatingDate = DateTime.Now;
        }

        public virtual void Approve()
        {
            Status = Models.Status.APPROVED;
            ResponseDate = DateTime.Now;
        }

        public void Reject(string reason)
        {
            Status = Models.Status.REJECTED;
            RejectReason = reason;
            ResponseDate = DateTime.Now;
        }
    }

    public enum Status { APPROVED, REJECTED };
}
