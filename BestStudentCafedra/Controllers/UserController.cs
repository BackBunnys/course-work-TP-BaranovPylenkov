using BestStudentCafedra.Data;
using BestStudentCafedra.Models;
using BestStudentCafedra.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BestStudentCafedra.Controllers
{
    public class UserController : Controller
    {
        UserManager<User> _userManager;
        RoleManager<IdentityRole> _roleManager;
        SubjectAreaDbContext _subjectAreaContext;

        public UserController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, 
            SubjectAreaDbContext subjectAreaContext)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _subjectAreaContext = subjectAreaContext;
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
        [HttpGet]
        public async Task<IActionResult> Confirm(string email) 
        {
            if (email == null) return NotFound();
            User user = await _userManager.FindByEmailAsync(email);
            if (user == null) return NotFound();
            UserConfirmationViewModel viewModel = new UserConfirmationViewModel()
            {
                Email = user.Email,
                FullName = user.SecondName + " " + user.FirstName + " " + user.MiddleName,
                AllRoles = _roleManager.Roles.ToList()
            };
            return View(viewModel);
        }

        [HttpPost, ActionName("Confirm")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Confirm(UserConfirmationViewModel viewModel)
        {
            if (viewModel.Email == null) return NotFound();
            User user = await _userManager.FindByEmailAsync(viewModel.Email);
            if (user == null) return NotFound();
            if (viewModel.Roles.Contains("teacher") && viewModel.Roles.Contains("student"))
            {
                ModelState.AddModelError("AllRoles", "Пользователь не может одновременно иметь роль ученика и учителя");
                return View(FillByRoles(viewModel));
            }
            if(viewModel.Roles.Contains("teacher"))
            {
                if (_subjectAreaContext.Teachers.FirstOrDefault(t => t.Id == viewModel.SubjectAreaId) == null)
                {
                    ModelState.AddModelError("SubjectAreaId", "Преподавателя с таким id нет в системе");
                    return View(FillByRoles(viewModel));
                }
                else
                    user.SubjectAreaId = viewModel.SubjectAreaId;
            }
            if (viewModel.Roles.Contains("student"))
            {
                if (_subjectAreaContext.Students.FirstOrDefault(s => s.GradebookNumber == viewModel.SubjectAreaId) == null)
                {
                    ModelState.AddModelError("SubjectAreaId", "Студента с таким id нет в системе");
                    return View(FillByRoles(viewModel));
                }
                else
                    user.SubjectAreaId = viewModel.SubjectAreaId;
            }
            user.IsConfirmed = true;
            await _userManager.UpdateAsync(user);
            await _userManager.AddToRolesAsync(user, viewModel.Roles);
            return RedirectToAction("Index");
        }

        private object FillByRoles(UserConfirmationViewModel viewModel)
        {
            viewModel.AllRoles = _roleManager.Roles.ToList();
            return viewModel;
        }

        public async Task<IActionResult> Delete(string email)
        {
            if (email == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return NotFound();
            }
            return View(new UserViewModel
            {
                Email = user.Email,
                FullName = user.SecondName + " " + user.FirstName + " " + user.MiddleName,
                IsConfirmed = user.IsConfirmed,
                Roles = await _userManager.GetRolesAsync(user)
            });
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string email)
        {
            if (email == null) return NotFound();
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return NotFound();
            await _userManager.DeleteAsync(user);
            return RedirectToAction(nameof(Index));
        }

        /*
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            IdentityRole role = await _roleManager.FindByIdAsync(id);
            if (role != null)
            {
                IdentityResult result = await _roleManager.DeleteAsync(role);
            }
            return RedirectToAction("Index");
        }

        public IActionResult UserList() => View(_userManager.Users.ToList());

        public async Task<IActionResult> Edit(string userId)
        {
            // получаем пользователя
            User user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                // получем список ролей пользователя
                var userRoles = await _userManager.GetRolesAsync(user);
                var allRoles = _roleManager.Roles.ToList();
                ChangeRoleViewModel model = new ChangeRoleViewModel
                {
                    UserId = user.Id,
                    UserEmail = user.Email,
                    UserRoles = userRoles,
                    AllRoles = allRoles
                };
                return View(model);
            }

            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> Edit(string userId, List<string> roles)
        {
            // получаем пользователя
            User user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                // получем список ролей пользователя
                var userRoles = await _userManager.GetRolesAsync(user);
                // получаем все роли
                var allRoles = _roleManager.Roles.ToList();
                // получаем список ролей, которые были добавлены
                var addedRoles = roles.Except(userRoles);
                // получаем роли, которые были удалены
                var removedRoles = userRoles.Except(roles);

                await _userManager.AddToRolesAsync(user, addedRoles);

                await _userManager.RemoveFromRolesAsync(user, removedRoles);

                return RedirectToAction("UserList");
            }

            return NotFound();
        }*/
    }
}
