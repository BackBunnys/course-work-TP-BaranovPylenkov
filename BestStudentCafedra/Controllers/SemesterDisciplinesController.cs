using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BestStudentCafedra.Data;
using BestStudentCafedra.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace BestStudentCafedra.Controllers
{
    public class SemesterDisciplinesController : Controller
    {
        private readonly SubjectAreaDbContext _context;
        private readonly UserManager<User> _userManager;

        public SemesterDisciplinesController(SubjectAreaDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: SemesterDisciplines/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id, string returnUrl, string groupId)
        {
            if (id == null)
            {
                return NotFound();
            }

            var semesterDiscipline = await _context.SemesterDiscipline
                .Include(s => s.Discipline)
                .Include(a => a.Activities)
                .ThenInclude(t => t.Type)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (semesterDiscipline == null)
            {
                return NotFound();
            }

            if (User.IsInRole("teacher"))
            {
                User user = await _userManager.FindByNameAsync(User.Identity.Name);
                var teacherDisciplines = await _context.TeacherDisciplines
                    .Where(x => x.TeacherId == user.SubjectAreaId && x.DisciplineId == semesterDiscipline.DisciplineId)
                    .ToListAsync();
                if (teacherDisciplines.Count() == 0) return Redirect("/Account/AccessDenied");
            }
            else if (User.IsInRole("student"))
            {
                User user = await _userManager.FindByNameAsync(User.Identity.Name);
                var student = await _context.Students.FirstOrDefaultAsync(x => x.GradebookNumber == user.SubjectAreaId);
                var groupDiscipline = await _context.GroupDisciplines
                    .Where(x => x.GroupId == student.GroupId && x.DisciplineId == semesterDiscipline.DisciplineId)
                    .ToListAsync();
                if (groupDiscipline.Count() == 0) return Redirect("/Account/AccessDenied");
            }

            ViewData["returnUrl"] = returnUrl;
            ViewData["groupId"] = groupId;
            return View(semesterDiscipline);
        }

        // GET: SemesterDisciplines/Create
        [Authorize(Roles = "methodist")]
        public async Task<IActionResult> Create(int id, string ReturnUrl)
        {
            ViewData["ReturnUrl"] = ReturnUrl;
            Discipline discipline = await _context.Disciplines.FirstOrDefaultAsync(d => d.Id == id);
            SemesterDiscipline semesterDiscipline = new SemesterDiscipline() { Discipline = discipline };
            return View(semesterDiscipline);
        }

        // POST: SemesterDisciplines/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "methodist")]
        public async Task<IActionResult> Create([Bind("DisciplineId,Year,Semester,ControlType")] SemesterDiscipline semesterDiscipline, string ReturnUrl)
        {
            ViewData["ReturnUrl"] = ReturnUrl;
            semesterDiscipline.Discipline = await _context.Disciplines.FirstOrDefaultAsync(d => d.Id == semesterDiscipline.DisciplineId);

            if (_context.SemesterDiscipline
                .Where(x => x.DisciplineId == semesterDiscipline.DisciplineId && 
                x.Year == semesterDiscipline.Year &&
                x.Semester == semesterDiscipline.Semester).Count() > 0)
            {
                ModelState.AddModelError("", "Семестровая дисциплина с таким курсом и семестром уже существует!");
                return View(semesterDiscipline);
            }

            if (ModelState.IsValid)
            {
                _context.Add(semesterDiscipline);
                await _context.SaveChangesAsync();

                if (semesterDiscipline.ControlType == ControlType.Exam)
                {
                    var type = _context.ActivityTypes.FirstOrDefault(x => x.Name == RatingControlsController.EXAMTYPENAME);
                    if (type != null)
                    {
                        var exam = new Activity();
                        exam.MaxPoints = 40;
                        exam.Number = 1;
                        exam.SemesterDisciplineId = semesterDiscipline.Id;
                        exam.Title = "Экзамен";
                        exam.TypeId = type.Id;
                        _context.Add(exam);
                        await _context.SaveChangesAsync();
                    }
                }

                if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
                {
                    return Redirect(ReturnUrl);
                }
                else
                {
                    return RedirectToAction("Edit", "Disciplines", new { id = semesterDiscipline.DisciplineId });
                }
            }
            return View(semesterDiscipline);
        }

        // GET: SemesterDisciplines/Edit/5
        [Authorize(Roles = "methodist")]
        public async Task<IActionResult> Edit(int? id, int disciplineId, string returnUrl)
        {
            if (id == null)
            {
                return NotFound();
            }

            var semesterDiscipline = await _context.SemesterDiscipline
                .Include(s => s.Discipline)
                .Include(a => a.Activities)
                .ThenInclude(t => t.Type)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (semesterDiscipline == null)
            {
                return NotFound();
            }

            ViewData["returnUrl"] = returnUrl;
            return View(semesterDiscipline);
        }

        // POST: SemesterDisciplines/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "methodist")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DisciplineId,Year,Semester,ControlType")] SemesterDiscipline semesterDiscipline, string returnUrl)
        {
            if (id != semesterDiscipline.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(semesterDiscipline);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SemesterDisciplineExists(semesterDiscipline.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    return RedirectToAction("Details", "Disciplines", new { id = semesterDiscipline.DisciplineId });
                }
            }
            ViewData["returnUrl"] = returnUrl;
            ViewData["DisciplineId"] = new SelectList(_context.Disciplines, "Id", "Name", semesterDiscipline.DisciplineId);
            return View(semesterDiscipline);
        }

        // GET: SemesterDisciplines/Delete/5
        [Authorize(Roles = "methodist")]
        public async Task<IActionResult> Delete(int? id, string returnUrl)
        {
            if (id == null)
            {
                return NotFound();
            }

            var semesterDiscipline = await _context.SemesterDiscipline
                .Include(s => s.Discipline)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (semesterDiscipline == null)
            {
                return NotFound();
            }

            ViewData["returnUrl"] = returnUrl;
            return View(semesterDiscipline);
        }

        // POST: SemesterDisciplines/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "methodist")]
        public async Task<IActionResult> DeleteConfirmed(int id, string returnUrl)
        {
            var semesterDiscipline = await _context.SemesterDiscipline.FindAsync(id);
            _context.SemesterDiscipline.Remove(semesterDiscipline);
            await _context.SaveChangesAsync();
            return Redirect(returnUrl);
        }

        private bool SemesterDisciplineExists(int id)
        {
            return _context.SemesterDiscipline.Any(e => e.Id == id);
        }
    }
}
