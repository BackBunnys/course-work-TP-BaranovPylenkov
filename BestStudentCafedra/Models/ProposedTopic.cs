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
        [StringLength(100, MinimumLength = 10, ErrorMessage = "Тема должна быть длиной от 10 до 100 символов")]
        [Display(Name = "Название")]
        public string Name { get; set; }
    }
}
