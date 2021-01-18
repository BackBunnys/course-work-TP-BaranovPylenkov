using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BestStudentCafedra.Models
{
    public class TeacherRequest: Request
    {
        public int Id { get; set; }
        [Display(Name = "Выпускная квалификационная работа")]
        [Required]
        public int GraduationWorkId { get; set; }
        [Display(Name = "Преподаватель")]
        [Required]
        public int TeacherId { get; set; }
        [Display(Name = "Мотивация")]
        public string Motivation { get; set; }
        [Display(Name = "Тип запроса")]
        [Required]
        public RequestType RequestType { get; set; }

        public override void Approve(Person approvingPerson)
        {
            if (approvingPerson is Teacher teacher)
            {
                if (teacher.Id != TeacherId)
                    throw new ArgumentException("Teacher request can be approved only by teacher to which this request is directed");
                
                base.Approve(teacher);
                if (RequestType == RequestType.ADVISER)
                    GraduationWork.ScientificAdviserId = TeacherId;
                else if (RequestType == RequestType.REVIEWER)
                    GraduationWork.ReviewerId = TeacherId;
            }
            else throw new ArgumentException("Teacher request can be approved only by teacher");
        }

        public virtual Teacher Teacher { get; set; }
        public virtual GraduationWork GraduationWork { get; set; }
    }
    public enum RequestType { ADVISER, REVIEWER };
}
