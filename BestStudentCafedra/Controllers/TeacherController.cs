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
    public class TeacherController : Controller
    {
        private readonly SubjectAreaDbContext _context;
        private readonly UserManager<User> _userManager;

        public TeacherController(SubjectAreaDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Teachers
        [Authorize]
        public async Task<IActionResult> Index()
        {
            List<Teacher> teachers = null;
            User user = await _userManager.FindByNameAsync(User.Identity.Name);

            if (User.IsInRole("student"))
            {
                var disciplines = await _context.AcademicGroups
                    .Include(x => x.GroupDiscipline)
                        .ThenInclude(y => y.Discipline)
                            .ThenInclude(z => z.TeacherDisciplines)
                                .ThenInclude(h => h.Teacher)
                    .SelectMany(x => x.GroupDiscipline)
                    .ToListAsync();

                var teacherDisciplines = disciplines.SelectMany(x => x.Discipline.TeacherDisciplines);
                teachers = teacherDisciplines.Select(x => x.Teacher).Distinct().ToList();
            }
            else if (User.IsInRole("teacher"))
            {
                var teacherDisciplines = await _context.Disciplines
                    .Include(x => x.TeacherDisciplines)
                        .ThenInclude(x => x.Teacher)
                    .Where(x => x.TeacherDisciplines.Any(y => y.TeacherId == user.SubjectAreaId))
                    .SelectMany(x => x.TeacherDisciplines)
                    .ToListAsync();
                teachers = teacherDisciplines.Select(x => x.Teacher).Distinct().ToList();
            }
            else
            {
                teachers = await _context.Teachers.ToListAsync();
            }

            return View(teachers);
        }

        // GET: Teachers/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id, string ReturnUrl)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacher = await _context.Teachers
                .Include(e => e.TeacherDisciplines)
                .ThenInclude(sc => sc.Discipline)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (teacher == null)
            {
                return NotFound();
            }
            ViewData["ReturnUrl"] = ReturnUrl;
            return View(teacher);
        }

        [HttpGet]
        [Authorize(Roles = "methodist")]
        public async Task<IActionResult> AddDiscipline(int? id, string ReturnUrl)
        {
            if (id == null)
            {
                return NotFound();
            }

            ViewData["TeacherId"] = id;
            List<Discipline> teacherDisciplines = await _context.TeacherDisciplines
                .Where(x => x.TeacherId == id)
                .Select(x => x.Discipline)
                .ToListAsync();

            List<Discipline> disciplines = await _context.Disciplines.ToListAsync();
            disciplines.RemoveAll(x => teacherDisciplines.Any(y => y.Id == x.Id));

            ViewData["ReturnUrl"] = ReturnUrl;
            return View(disciplines);
        }

        [HttpPost]
        [Authorize(Roles = "methodist")]
        public async Task<IActionResult> AddDiscipline(int id, int DisciplineId, string ReturnUrl)
        {
            if(_context.TeacherDisciplines.Where(x => x.TeacherId == id && x.DisciplineId == DisciplineId).Count() > 0)
            {
                return Conflict();
            }

            var newTeacherDisp = new TeacherDiscipline();
            newTeacherDisp.TeacherId = (int)id;
            newTeacherDisp.DisciplineId = DisciplineId;

            _context.Add(newTeacherDisp);
            await _context.SaveChangesAsync();
            ViewData["ReturnUrl"] = ReturnUrl;
            return RedirectToAction(nameof(Edit), new { id = id, ReturnUrl = ReturnUrl });
        }

        [Authorize(Roles = "methodist")]
        public async Task<IActionResult> DropDiscipline(int? id, string ReturnUrl)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacherDiscipline = await _context.TeacherDisciplines
                .Include(x => x.Teacher)
                .Include(x => x.Discipline)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (teacherDiscipline == null)
            {
                return NotFound();
            }

            ViewData["ReturnUrl"] = ReturnUrl;
            return PartialView("_DropDiscipline", teacherDiscipline);
        }

        // POST: AcademicGroups/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "methodist")]
        public async Task<IActionResult> DropDiscipline(int id, string ReturnUrl)
        {
            var teacherDiscipline = await _context.TeacherDisciplines.FirstOrDefaultAsync(x => x.Id == id);
            var teacherId = teacherDiscipline.TeacherId;
            _context.TeacherDisciplines.Remove(teacherDiscipline);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Edit), new { id = teacherId, ReturnUrl = ReturnUrl });
        }

        // GET: Teachers/Create
        [Authorize(Roles = "methodist")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Teachers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "methodist")]
        public async Task<IActionResult> Create([Bind("Id,FullName,Post,AcademicDegree")] Teacher teacher)
        {
            if (ModelState.IsValid)
            {
                _context.Add(teacher);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(teacher);
        }

        // GET: Teachers/Edit/5
        [Authorize(Roles = "methodist")]
        public async Task<IActionResult> Edit(int? id, string ReturnUrl)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacher = await _context.Teachers
                .Include(e => e.TeacherDisciplines)
                .ThenInclude(sc => sc.Discipline)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (teacher == null)
            {
                return NotFound();
            }
            ViewData["ReturnUrl"] = ReturnUrl;
            return View(teacher);
        }

        // POST: Teachers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "methodist")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FullName,Post,AcademicDegree")] Teacher teacher, string ReturnUrl)
        {
            if (id != teacher.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(teacher);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeacherExists(teacher.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Details), new { id = id, ReturnUrl = ReturnUrl });
            }
            ViewData["ReturnUrl"] = ReturnUrl;
            return View(teacher);
        }

        // GET: Teachers/Delete/5
        [Authorize(Roles = "methodist")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacher = await _context.Teachers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (teacher == null)
            {
                return NotFound();
            }

            return View(teacher);
        }

        // POST: Teachers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "methodist")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var teacher = await _context.Teachers.FindAsync(id);
            _context.Teachers.Remove(teacher);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TeacherExists(int id)
        {
            return _context.Teachers.Any(e => e.Id == id);
        }
    }
}
