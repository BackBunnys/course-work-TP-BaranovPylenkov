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
        public IList<String> Roles { get; set; }
        [Display(Name = "Имя")]
        public string FullName { get; set; }
    }
}
