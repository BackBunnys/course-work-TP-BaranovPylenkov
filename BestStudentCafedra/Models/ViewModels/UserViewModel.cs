using BestStudentCafedra.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BestStudentCafedra.Models.ViewModels
{
    public class UserViewModel
    {
        [Display(Name = "Почта")]
        public string Email { get; set; }

        [Display(Name = "Подтверждён")]
        public bool IsConfirmed { get; set; }

        [Display(Name = "Роли")]
        [Required(ErrorMessage = "Укажите роли")]
        [RolesValidation("student", ErrorMessage = "Студент не может содержать больше ролей")]
        public List<string> Roles { get; set; }

        [Display(Name = "Имя")]
        public string FullName { get; set; }

        [Display(Name = "Id предметной области")]
        public int? SubjectAreaId { get; set; }

        public UserViewModel() { }
        public UserViewModel(User user, List<String> roles)
        {
            Email = user.Email;
            FullName = user.SecondName + " " + user.FirstName + " " + user.MiddleName;
            Roles = roles;
            IsConfirmed = user.IsConfirmed;
            SubjectAreaId = user.SubjectAreaId;
        }
    }
}
