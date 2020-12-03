using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace BestStudentCafedra.Models
{
    public partial class ProposedTopic
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Введите тему")]
        [MinLength(10, ErrorMessage = "Тема должна быть длиной больше 10 символов")]
        [Display(Name = "Название")]
        public string Name { get; set; }
    }
}
