using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BestStudentCafedra.Data;
using BestStudentCafedra.Models;

namespace BestStudentCafedra.Controllers
{
    public class TeacherController : Controller
    {
        private readonly SubjectAreaDbContext _context;

        public TeacherController(SubjectAreaDbContext context)
        {
            _context = context;
        }

        // GET: Teachers
        public async Task<IActionResult> Index()
        {
            return View(await _context.Teachers.ToListAsync());
        }

        // GET: Teachers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacher = await _context.Teachers
                .Include(e => e.TeacherDisciplines)
                .ThenInclude(sc => sc.Discipline)
                .FirstOrDefaultAsync(m => m.Id == id); ;

            if (teacher == null)
            {
                return NotFound();
            }

            return View(teacher);
        }

        [HttpGet]
        public async Task<IActionResult> AddDiscipline(int? id)
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

            ViewData["DisciplinesId"] = new SelectList(disciplines, "Id", "Name");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddDiscipline(int id, int DisciplineId)
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
            return RedirectToAction(nameof(Details), new { id = id });
        }

        [HttpGet]
        public async Task<IActionResult> DeleteDiscipline(int id)
        {
            var teacherDiscipline = await _context.TeacherDisciplines.FirstOrDefaultAsync(x => x.Id == id);
            var teacherId = teacherDiscipline.TeacherId;
            _context.TeacherDisciplines.Remove(teacherDiscipline);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { id = teacherId });
        }
        
        // GET: Teachers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Teachers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
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
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacher = await _context.Teachers.FindAsync(id);
            if (teacher == null)
            {
                return NotFound();
            }
            return View(teacher);
        }

        // POST: Teachers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FullName,Post,AcademicDegree")] Teacher teacher)
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
                return RedirectToAction(nameof(Index));
            }
            return View(teacher);
        }

        // GET: Teachers/Delete/5
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
