using BestStudentCafedra.Data;
using BestStudentCafedra.Models;
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
    public class AcademicGroupsController : Controller
    {
        private readonly SubjectAreaDbContext _context;
        private readonly UserManager<User> _userManager;

        public AcademicGroupsController(SubjectAreaDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: AcademicGroups
        [Authorize]
        public async Task<IActionResult> Index(int? formYear)
        {
            List<AcademicGroup> groups = null;

            if (User.IsInRole("teacher"))
            {
                User user = await _userManager.FindByNameAsync(User.Identity.Name);
                groups = _context.AcademicGroups
                    .Include(x => x.Specialty)
                    .Include(x => x.GroupDiscipline)
                        .ThenInclude(x => x.Discipline)
                            .ThenInclude(x => x.TeacherDisciplines)
                    .Where(x => x.GroupDiscipline.Any(y => y.Discipline.TeacherDisciplines.Any(z => z.TeacherId == user.SubjectAreaId)))
                    .ToList();
            }
            else
            {
                groups = await _context.AcademicGroups
                    .Include(s => s.Specialty)
                    .ToListAsync();
            }

            ViewData["formYears"] = new SelectList(groups.Select(y => y.FormationYear).Distinct(), formYear);

            if (formYear != null)
            {
                groups = groups.Where(x => x.FormationYear == formYear).ToList();
            }

            return View(groups);
        }

        // GET: AcademicGroups/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var groups = await _context.AcademicGroups
                .Include(s => s.Students)
                .Include(s => s.Specialty)
                .Include(d => d.GroupDiscipline)
                .ThenInclude(d => d.Discipline)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (groups == null)
            {
                return NotFound();
            }

            if (User.IsInRole("teacher"))
            {
                User user = await _userManager.FindByNameAsync(User.Identity.Name);
                var disciplines = groups.GroupDiscipline.Select(x => x.Discipline).AsEnumerable();
                var teacherDiscplines = _context.TeacherDisciplines.AsEnumerable()
                    .Where(x => x.TeacherId == user.SubjectAreaId && disciplines.Any(y => x.DisciplineId == y.Id))
                    .ToList();
                if (teacherDiscplines.Count() == 0) return Redirect("/Account/AccessDenied");
            }
            else if (User.IsInRole("student"))
            {
                User user = await _userManager.FindByNameAsync(User.Identity.Name);
                var students = groups.Students.Where(x => x.GroupId == id && x.GradebookNumber == user.SubjectAreaId);
                if (students.Count() == 0) return Redirect("/Account/AccessDenied");
            }

            return View(groups);
        }

        [Authorize]
        public async Task<IActionResult> Semesters(int id, int groupId, string ReturnUrl)
        {
            var semesterDisciplines = await _context.SemesterDiscipline.Where(x => x.DisciplineId == id).ToListAsync();
            ViewData["GroupId"] = groupId;
            ViewData["ReturnUrl"] = ReturnUrl;
            return PartialView("_Semesters", semesterDisciplines);
        }

        // GET: AcademicGroups/Create
        [Authorize(Roles = "methodist")]
        public ActionResult Create()
        {
            ViewData["SpecialtyId"] = new SelectList(_context.Specialties.OrderBy(c => c.Code).Select(x => new SelectListItem { Text = $"{x.Code} - {x.Name}", Value = x.Code }), "Value", "Text");
            return View();
        }

        // POST: AcademicGroups/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "methodist")]
        public async Task<IActionResult> Create([Bind("Id,SpecialtyId,Name,FormationYear")] AcademicGroup group)
        {
            if (ModelState.IsValid)
            {
                _context.Add(group);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["SpecialtyId"] = new SelectList(_context.Specialties.OrderBy(c => c.Code).Select(x => new SelectListItem { Text = $"{x.Code} - {x.Name}", Value = x.Code }), "Value", "Text");
            return View(group);
        }

        // GET: AcademicGroups/Edit/5
        [Authorize(Roles = "methodist")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var group = await _context.AcademicGroups
                .Include(s => s.Students.OrderBy(n => n.FullName))
                .Include(s => s.Specialty)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (group == null)
            {
                return NotFound();
            }

            ViewData["SpecialtyId"] = new SelectList(_context.Specialties.OrderBy(c => c.Code).Select(x => new SelectListItem { Text = $"{x.Code} - {x.Name}", Value = x.Code }), "Value", "Text");
            return View(group);
        }

        // POST: AcademicGroups/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "methodist")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SpecialtyId,Name,FormationYear")] AcademicGroup group)
        {
            if (id != group.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(group);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GroupExists(group.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["SpecialtyId"] = new SelectList(_context.Specialties.OrderBy(c => c.Code).Select(x => new SelectListItem { Text = $"{x.Code} - {x.Name}", Value = x.Code }), "Value", "Text");
            return View(group);
        }

        [Authorize(Roles = "methodist")]
        public async Task<IActionResult> AddDiscipline(int? id)
        {
            if (id == null || !GroupExists((int)id))
                return NotFound();

            List<Discipline> groupDisciplines = await _context.GroupDisciplines
                .Where(x => x.GroupId == id)
                .Select(x => x.Discipline)
                .ToListAsync();
            var disciplines = await _context.Disciplines.OrderBy(x => x.Name).ToListAsync();
            disciplines.RemoveAll(x => groupDisciplines.Any(y => y.Id == x.Id));

            ViewData["DisciplineName"] = _context.AcademicGroups.FirstOrDefault(x => x.Id == id).Name;
            ViewData["GroupId"] = id;
            return View(disciplines);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "methodist")]
        public async Task<IActionResult> AddDiscipline(int id, int disciplineId)
        {
            if (GroupExists(id))
            {
                if (_context.GroupDisciplines.Where(x => x.GroupId == id && x.DisciplineId == disciplineId).Count() > 0)
                {
                    return Conflict();
                }

                var newGroupDiscip = new GroupDiscipline();
                newGroupDiscip.GroupId = (int)id;
                newGroupDiscip.DisciplineId = disciplineId;

                _context.Add(newGroupDiscip);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Details), new { id = id });
            }
            else
            {
                return NotFound();
            } 
        }

        [Authorize(Roles = "methodist")]
        public async Task<IActionResult> DropDiscipline(int? id, int? disciplineId)
        {
            if (id == null || disciplineId == null || !GroupExists((int)id))
                return NotFound();

            var groupDiscipline = await _context.GroupDisciplines
                .Include(x => x.AcademicGroup)
                .Include(x => x.Discipline)
                .FirstOrDefaultAsync(x => x.GroupId == id && x.DisciplineId == disciplineId);
            if (groupDiscipline == null)
            {
                return NotFound();
            }

            return PartialView("_DropDiscipline", groupDiscipline);
        }

        // POST: AcademicGroups/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "methodist")]
        public async Task<IActionResult> DropDiscipline(int id)
        {
            var discipline = await _context.GroupDisciplines.FindAsync(id);
            var groupId = discipline.GroupId;
            _context.GroupDisciplines.Remove(discipline);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { id = groupId });
        }

        [Authorize(Roles = "methodist")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var group = await _context.AcademicGroups
                .FirstOrDefaultAsync(m => m.Id == id);
            if (group == null)
            {
                return NotFound();
            }

            return View(group);
        }

        // POST: AcademicGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "methodist")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var group = await _context.AcademicGroups.FindAsync(id);
            _context.AcademicGroups.Remove(group);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GroupExists(int id)
        {
            return _context.AcademicGroups.Any(e => e.Id == id);
        }
    }
}
