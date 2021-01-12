using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace BestStudentCafedra.Models
{
    public partial class Specialty
    {
        public Specialty()
        {
            AcademicGroups = new HashSet<AcademicGroup>();
        }

        [Required(ErrorMessage = "Не указан код")]
        [RegularExpression(@"/^([0-9]{2}[.]){2}([0-9]{2})$/", ErrorMessage = "Код должно соответсвовать паттерну ##.##.##")]
        [Display(Name = "Код")]
        public string Code { get; set; }
        [UIHint("Enum")]
        [EnumDataType(typeof(AcademicDegree))]
        [Display(Name = "Ученая степень")]
        public AcademicDegree AcademicDegree { get; set; }
        [Required(ErrorMessage = "Введите название")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Название должно быть длиной от 5 до 100 символов")]
        [Display(Name = "Название")]
        public string Name { get; set; }

        public virtual ICollection<AcademicGroup> AcademicGroups { get; set; }
    }

    public enum AcademicDegree
    {
        [Display(Name = "Бакалавриат")]
        Bachelor,
        [Display(Name = "Специалитет")]
        Specialty,
        [Display(Name = "Магистратура")]
        Magistracy,
        [Display(Name = "Аспирантура")]
        Postgraduate
    }
}
