using BestStudentCafedra.Data;
using BestStudentCafedra.Models;
using BestStudentCafedra.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BestStudentCafedra.Controllers
{
    [Authorize(Roles = "methodist")]
    public class UserController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SubjectAreaDbContext _subjectAreaContext;

        public UserController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, 
            SubjectAreaDbContext subjectAreaContext)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _subjectAreaContext = subjectAreaContext;
        }

        public async Task<IActionResult> Index(bool onlyUnconfirmed = false)
        {
            List<UserViewModel> userViewModels = new List<UserViewModel>();

            var users = _userManager.Users;
            if (onlyUnconfirmed)
            {
                ViewData["type"] = "Неподтверждённые пользователи";
                users = users.Where(u => !u.IsConfirmed);
            }

            foreach (User user in users)
                userViewModels.Add(new UserViewModel(user, (await _userManager.GetRolesAsync(user)).ToList()));

            return View(userViewModels.OrderBy(x => x.FullName));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string email) 
        {
            User user = await GetUser(email);
            if (user == null) return NotFound();
            ViewData["AllRoles"] = _roleManager.Roles.Select(r => r.Name).ToList();

            return View(new UserViewModel(user, (await _userManager.GetRolesAsync(user)).ToList()));
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserViewModel viewModel)
        {
            User user = await GetUser(viewModel.Email);
            if (user == null) return NotFound();

            bool isSubjectExists = true;
            if (ModelState.IsValid)
            {
                if (viewModel.Roles.Contains("teacher"))
                {
                    if (!_subjectAreaContext.Teachers.Any(t => t.Id == viewModel.SubjectAreaId))
                    {
                        ModelState.AddModelError("", "Преподавателя с таким id нет в системе");
                        isSubjectExists = false;
                    }
                }
                if (viewModel.Roles.Contains("student"))
                {
                    if (!_subjectAreaContext.Students.Any(s => s.GradebookNumber == viewModel.SubjectAreaId))
                    {
                        ModelState.AddModelError("", "Студента с таким id нет в системе");
                        isSubjectExists = false;
                    }
                }
                if (isSubjectExists)
                {
                    user.SubjectAreaId = viewModel.SubjectAreaId;
                    user.IsConfirmed = true;
                    await _userManager.UpdateAsync(user);
                    await _userManager.RemoveFromRolesAsync(user, await _userManager.GetRolesAsync(user));
                    await _userManager.AddToRolesAsync(user, viewModel.Roles);
                    return RedirectToAction(nameof(Index));
                }
            }

            ViewData["AllRoles"] = _roleManager.Roles.Select(r => r.Name).ToList();
            return View(viewModel);
        }

        public async Task<IActionResult> Delete(string email)
        {
            User user = await GetUser(email);
            if (user == null) return NotFound();

            return View(new UserViewModel(user, (await _userManager.GetRolesAsync(user)).ToList()));
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string email)
        {
            User user = await GetUser(email);
            if (user == null) return NotFound();

            await _userManager.DeleteAsync(user);
            return RedirectToAction(nameof(Index));
        }

        private async Task<User> GetUser(string email)
        {
            if (email != null)
            {
                User foundedUser = await _userManager.FindByEmailAsync(email);
                if (foundedUser != null) return foundedUser;
            }
            return null;
        }

        public IActionResult TeacherSelect()
        {
            List<Teacher> teachers = _subjectAreaContext.Teachers.OrderBy(t => t.FullName).ToList();
            ViewData["Type"] = "преподавателя";
            ViewData["SubjectId"] = new SelectList(teachers, "Id", "FullName");
            return PartialView("_ChooseView");
        }
        public IActionResult StudentSelect()
        {
            List<Student> students = _subjectAreaContext.Students.OrderBy(s => s.FullName).ToList();
            ViewData["Type"] = "студента";
            ViewData["SubjectId"] = new SelectList(students, "GradebookNumber", "FullName");
            return PartialView("_ChooseView");
        }
    }
}
