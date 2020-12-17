using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace BestStudentCafedra.Models
{
    public partial class EventTemplate
    {
        public int Id { get; set; }
        [Display(Name = "№ п/п")]
        public int SequentialNumber { get; set; }
        [Display(Name = "Описание мероприятия")]
        [Required(ErrorMessage = "Не указано содержание мероприятия")]
        [StringLength(maximumLength: 150, MinimumLength = 5, ErrorMessage = "Описание мероприятия должно содержать от 5 до 150 символов")]
        public string Description { get; set; }
    }
}
