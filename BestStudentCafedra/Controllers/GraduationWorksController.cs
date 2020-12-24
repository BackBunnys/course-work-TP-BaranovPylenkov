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

namespace BestStudentCafedra.Controllers
{
    public class GraduationWorksController : Controller
    {
        private readonly SubjectAreaDbContext _context;
        private readonly UserManager<User> _userManager;

        public GraduationWorksController(SubjectAreaDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: GraduationWorks
        public async Task<IActionResult> Index()
        {
            User user = await _userManager.FindByNameAsync(User.Identity.Name);
            List<GraduationWork> graduationWorks = new List<GraduationWork>();
            if (User.IsInRole("methodist")) {
                graduationWorks = await _context.GraduationWorks
                    .Include(x => x.Student.Group)
                    .Include(x => x.AssignedStaffs)
                    .OrderByDescending(x => x.ArchievedDate).ToListAsync();
            }
            else if (User.IsInRole("teacher")) {
                graduationWorks = await _context.GraduationWorks
                    .Include(x => x.Student.Group)
                    .Include(x => x.AssignedStaffs)
                    .Where(x => x.AssignedStaffs.Any(x => x.TeacherId == user.SubjectAreaId && x.Type == "Scientific Adviser"))
                    .OrderByDescending(x => x.ArchievedDate).ToListAsync();
            }

            return View(graduationWorks);
        }

        // GET: GraduationWorks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var graduationWork = await _context.GraduationWorks
                .Include(g => g.Student)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (graduationWork == null)
            {
                return NotFound();
            }

            return View(graduationWork);
        }

        // GET: GraduationWorks/Create
        public IActionResult Create()
        {
            ViewData["StudentId"] = new SelectList(_context.Students, "GradebookNumber", "FullName");
            return View();
        }

        // POST: GraduationWorks/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,StudentId,Theme,ArchievedDate,Result")] GraduationWork graduationWork)
        {
            if (ModelState.IsValid)
            {
                _context.Add(graduationWork);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["StudentId"] = new SelectList(_context.Students, "GradebookNumber", "FullName", graduationWork.StudentId);
            return View(graduationWork);
        }

        // GET: GraduationWorks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var graduationWork = await _context.GraduationWorks.FindAsync(id);
            if (graduationWork == null)
            {
                return NotFound();
            }
            ViewData["StudentId"] = new SelectList(_context.Students, "GradebookNumber", "FullName", graduationWork.StudentId);
            return View(graduationWork);
        }

        // POST: GraduationWorks/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,StudentId,Theme,ArchievedDate,Result")] GraduationWork graduationWork)
        {
            if (id != graduationWork.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(graduationWork);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GraduationWorkExists(graduationWork.Id))
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
            ViewData["StudentId"] = new SelectList(_context.Students, "GradebookNumber", "FullName", graduationWork.StudentId);
            return View(graduationWork);
        }

        // GET: GraduationWorks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var graduationWork = await _context.GraduationWorks
                .Include(g => g.Student)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (graduationWork == null)
            {
                return NotFound();
            }

            return View(graduationWork);
        }

        // POST: GraduationWorks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var graduationWork = await _context.GraduationWorks.FindAsync(id);
            _context.GraduationWorks.Remove(graduationWork);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GraduationWorkExists(int id)
        {
            return _context.GraduationWorks.Any(e => e.Id == id);
        }
    }
}
