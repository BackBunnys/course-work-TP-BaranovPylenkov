using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BestStudentCafedra.Models
{
    public class ThemeRequest : Request
    {
        public int Id { get; set; }
        public int GraduationWorkId { get; set; }
        public string Theme { get; set; }
        public string Motivation { get; set; }
        public Status? TeacherResponse { get; set; }
        public Status? CafedraResponse { get; set; }
         
        public override void Approve(Person approvingPerson)
        {
            if (approvingPerson is Student)
                throw new ArgumentException("Theme request can't be approved by student");
            if(approvingPerson is Teacher teacher)
            {
                if (GraduationWork.ScientificAdviserId != teacher.Id)
                    throw new ArgumentException("Theme request can be approved only by teacher which is scientific adviser of the work");

                TeacherResponse = Models.Status.APPROVED;
            }
            else
            {
                if (TeacherResponse != Models.Status.APPROVED)
                    throw new ArgumentException("Theme request can be approved by cafedra only if it approved by teacher");

                CafedraResponse = Models.Status.REJECTED;
                base.Approve(approvingPerson);
                GraduationWork.Theme = Theme;
            }
        }

        public override void Reject(Person rejectingPerson, string reason)
        {
            if (rejectingPerson is Student)
                throw new ArgumentException("Theme request can't be rejected by student");
            if (rejectingPerson is Teacher)
                TeacherResponse = Models.Status.REJECTED;
            else
                CafedraResponse = Models.Status.REJECTED;
            base.Reject(rejectingPerson, reason);
        }

        public virtual GraduationWork GraduationWork { get; set; }
    }
}
