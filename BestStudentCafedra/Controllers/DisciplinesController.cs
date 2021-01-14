using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BestStudentCafedra.Data;
using BestStudentCafedra.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace BestStudentCafedra.Controllers
{
    public class DisciplinesController : Controller
    {
        private readonly SubjectAreaDbContext _context;
        private readonly UserManager<User> _userManager;

        public DisciplinesController(SubjectAreaDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Disciplines
        [Authorize]
        public async Task<IActionResult> Index()
        {
            List<Discipline> disciplines = null;
            User user = await _userManager.FindByNameAsync(User.Identity.Name);

            if (User.IsInRole("student"))
            {
                var group = await _context.Students
                    .Include(x => x.Group)
                        .ThenInclude(y => y.GroupDiscipline)
                            .ThenInclude(z => z.Discipline)
                    .Where(x => x.GradebookNumber == user.SubjectAreaId)
                    .Select(x => x.Group)
                    .FirstOrDefaultAsync();

                disciplines = group.GroupDiscipline.Select(x => x.Discipline).ToList();
            } 
            else if(User.IsInRole("teacher"))
            {
                disciplines = await _context.TeacherDisciplines
                    .Include(x => x.Discipline)
                    .Where(x => x.TeacherId == user.SubjectAreaId)
                    .Select(x => x.Discipline)
                    .ToListAsync();
            }
            else
            {
                disciplines = await _context.Disciplines
                    .Include(s => s.SemesterDisciplines.OrderBy(x => x.Year).ThenBy(y => y.Semester))
                    .ThenInclude(d => d.Discipline)
                    .ToListAsync();
            }
            return View(disciplines.OrderBy(x => x.Name));
        }

        // GET: Disciplines/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id, string returnUrl)
        {
            if (id == null)
            {
                return NotFound();
            }

            var discipline = await _context.Disciplines
                .Include(x => x.TeacherDisciplines)
                    .ThenInclude(x => x.Teacher)
                .Include(s => s.SemesterDisciplines.OrderBy(x => x.Year).ThenBy(y => y.Semester))
                    .ThenInclude(d => d.Discipline)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (discipline == null)
            {
                return NotFound();
            }

            ViewData["returnUrl"] = returnUrl;
            return View(discipline);
        }

        [Authorize]
        public async Task<IActionResult> Semesters(int id)
        {
            var semesterDisciplines = await _context.SemesterDiscipline.Where(x => x.DisciplineId == id).ToListAsync();
            return PartialView("_Semesters", semesterDisciplines);
        }

        // GET: Disciplines/Create
        [Authorize(Roles = "methodist")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Disciplines/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "methodist")]
        public async Task<IActionResult> Create([Bind("Id,Name")] Discipline discipline, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                _context.Add(discipline);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["returnUrl"] = returnUrl;
            return View(discipline);
        }

        // GET: Disciplines/Edit/5
        [Authorize(Roles = "methodist")]
        public async Task<IActionResult> Edit(int? id, string returnUrl)
        {
            if (id == null)
            {
                return NotFound();
            }

            var discipline = await _context.Disciplines
                .Include(s => s.SemesterDisciplines)
                    .ThenInclude(d => d.Discipline)
                .Include(x => x.TeacherDisciplines)
                    .ThenInclude(x => x.Teacher)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (discipline == null)
            {
                return NotFound();
            }

            ViewData["returnUrl"] = returnUrl;
            return View(discipline);
        }

        // POST: Disciplines/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "methodist")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Discipline discipline, string returnUrl)
        {
            if (id != discipline.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(discipline);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DisciplineExists(discipline.Id))
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

            ViewData["returnUrl"] = returnUrl;
            return View(discipline);
        }

        // GET: Disciplines/AddTeacher?groupId=5
        [Authorize(Roles = "methodist")]
        public async Task<IActionResult> AddTeacher(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacherDisciplines = _context.TeacherDisciplines
                .Include(x => x.Teacher)
                .Include(x => x.Discipline)
                .Where(x => x.DisciplineId == id)
                .AsEnumerable();

            var teachers = await _context.Teachers
                .Where(x => teacherDisciplines.Any(y => x.Id != y.TeacherId))
                .ToListAsync();

            if (teachers == null)
            {
                return NotFound();
            }

            ViewData["disсiplineId"] = id;
            ViewData["disсiplineName"] = teacherDisciplines.FirstOrDefault().Discipline.Name;
            return View(teachers);
        }

        // POST: Disciplines/AddTeacher/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "methodist")]
        public async Task<IActionResult> AddTeacher(int id, int teacherId)
        {
            if (_context.TeacherDisciplines.Where(x => x.DisciplineId == id && x.TeacherId == teacherId).Count() > 0)
            {
                return Conflict();    
            }

            var teacherDiscipline = new TeacherDiscipline();
            teacherDiscipline.DisciplineId = id;
            teacherDiscipline.TeacherId = teacherId;

            _context.Add(teacherDiscipline);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Disciplines/Delete/5
        [Authorize(Roles = "methodist")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var discipline = await _context.Disciplines
                .FirstOrDefaultAsync(m => m.Id == id);
            if (discipline == null)
            {
                return NotFound();
            }

            return View(discipline);
        }

        // POST: Disciplines/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "methodist")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var discipline = await _context.Disciplines.FindAsync(id);
            _context.Disciplines.Remove(discipline);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DisciplineExists(int id)
        {
            return _context.Disciplines.Any(e => e.Id == id);
        }
    }
}
