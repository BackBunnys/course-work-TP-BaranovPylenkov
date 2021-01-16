using System;
using System.ComponentModel.DataAnnotations;

namespace BestStudentCafedra.Models.ViewModels
{
    public class ArchiveWorkViewModel
    {
        public int GraduationWorkId { get; set; }
        [Display(Name = "Дата сдачи")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Не указана дата сдачи")]
        public DateTime ArchievedDate { get; set; }
        [Display(Name = "Результат")]
        [Required(ErrorMessage = "Не указан результат работы")]
        public int Result { get; set; }

        public GraduationWork GraduationWork { get; set; }
    }
}
