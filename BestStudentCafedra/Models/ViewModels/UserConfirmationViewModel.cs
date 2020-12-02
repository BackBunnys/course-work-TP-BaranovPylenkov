using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BestStudentCafedra.Models.ViewModels
{
    public class UserConfirmationViewModel
    {
        [Display(Name = "Почта")]
        public string Email { get; set; }

        [Display(Name = "Имя")]
        public string FullName { get; set; }

        [Display(Name = "Id предметной области")]
        public int SubjectAreaId { get; set; }
        public List<string> Roles { get; set; } = new List<string>();

        public List<IdentityRole> AllRoles { get; set; }

    }
}
