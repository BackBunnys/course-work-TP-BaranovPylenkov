using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BestStudentCafedra.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Имя")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Фамилия")]
        public string SecondName { get; set; }

        [Required]
        [Display(Name = "Отчество")]
        public string MiddleName { get; set; }

        [Required]
        [Display(Name = "Почта")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Подтвреждение пароля")]
        [Compare("Password")]
        [DataType(DataType.Password)]
        public string PasswordConfirm { get; set; }
    }
}
