using BestStudentCafedra.Models;
using BestStudentCafedra.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BestStudentCafedra.Controllers
{
    public class UsersController : Controller
    {
        UserManager<User> _userManager;

        public UsersController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(bool onlyUnconfirmed = false)
        {
            List<User> users;
            List<UserViewModel> userViewModels = new List<UserViewModel>();
            if (onlyUnconfirmed)
            {
                ViewData["type"] = "Неподтверждённые пользователи";
                users = _userManager.Users.Where(u => !u.IsConfirmed).ToList();
            }
            else
            {
                users = _userManager.Users.ToList();
            }

            foreach (User user in users)
            {
                IList<String> roles = await _userManager.GetRolesAsync(user);
                userViewModels.Add(new UserViewModel
                {
                    Email = user.Email,
                    FullName = user.SecondName + " " + user.FirstName + " " + user.MiddleName,
                    IsConfirmed = user.IsConfirmed,
                    Roles = roles
                });
            }
            return View(userViewModels);
        }
    }
}
