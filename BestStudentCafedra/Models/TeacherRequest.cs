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
        [Required(ErrorMessage = "Не выбрана работа")]
        public int GraduationWorkId { get; set; }
        [Display(Name = "Преподаватель")]
        [Required(ErrorMessage = "Не выбран преподаватель")]
        public int TeacherId { get; set; }
        [Display(Name = "Мотивация")]
        [StringLength(500, ErrorMessage = "Длина мотивации не должна превышать 500 символов.")]
        public string Motivation { get; set; }
        [Display(Name = "Тип запроса")]
        [Required(ErrorMessage = "Не выбран тип запроса")]
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
    public enum RequestType
    {
        [Display(Name = "Научный руководитель")]
        ADVISER, 
        [Display(Name = "Рецензент")]
        REVIEWER 
    };

}
