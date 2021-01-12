using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BestStudentCafedra.Models
{
    public class TeacherRequest: Request
    {
        public int Id { get; set; }
        public int GraduationWorkId { get; set; }
        public int TeacherId { get; set; }
        public RequestType RequestType { get; set; }

        public override void Approve()
        {
            base.Approve();
            if (RequestType == RequestType.ADVISER)
                GraduationWork.ScientificAdviserId = TeacherId;
            else if (RequestType == RequestType.REVIEWER)
                GraduationWork.ReviewerId = TeacherId;
        }

        public virtual Teacher Teacher { get; set; }
        public virtual GraduationWork GraduationWork { get; set; }
    }

    public enum RequestType { ADVISER, REVIEWER };
}
