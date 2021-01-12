using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BestStudentCafedra.Models
{
    public class Person
    {
        [Required(ErrorMessage = "Не указано имя")]
        [RegularExpression(@"^[А-Я][а-я]+\s+[А-Я|а-я][а-я]+(\s+[А-Я|а-я][а-я]+)*$", ErrorMessage = "Имя должно состоять минимум из двух слов по две буквы")]
        [Display(Name = "Имя")]
        public string FullName { get; set; }
    }
}
