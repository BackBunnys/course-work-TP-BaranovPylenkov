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
    public class ActivityProtectionsController : Controller
    {
        private readonly SubjectAreaDbContext _context;

        public ActivityProtectionsController(SubjectAreaDbContext context)
        {
            _context = context;
        }

        // GET: ActivityProtections
        public async Task<IActionResult> Index()
        {
            var subjectAreaDbContext = _context.ActivityProtections.Include(a => a.Activity).Include(a => a.Student);
            return View(await subjectAreaDbContext.ToListAsync());
        }

        // GET: ActivityProtections/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activityProtection = await _context.ActivityProtections
                .Include(a => a.Activity)
                .Include(a => a.Student)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (activityProtection == null)
            {
                return NotFound();
            }

            return View(activityProtection);
        }

        // GET: ActivityProtections/Create
        public IActionResult Create()
        {
            ViewData["ActivityId"] = new SelectList(_context.Activities, "Id", "Title");
            ViewData["StudentId"] = new SelectList(_context.Students, "GradebookNumber", "FullName");
            return View();
        }

        // POST: ActivityProtections/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,StudentId,ActivityId,ProtectionDate,Points")] ActivityProtection activityProtection)
        {
            if (ModelState.IsValid)
            {
                _context.Add(activityProtection);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ActivityId"] = new SelectList(_context.Activities, "Id", "Title", activityProtection.ActivityId);
            ViewData["StudentId"] = new SelectList(_context.Students, "GradebookNumber", "FullName", activityProtection.StudentId);
            return View(activityProtection);
        }

        // GET: ActivityProtections/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activityProtection = await _context.ActivityProtections.FindAsync(id);
            if (activityProtection == null)
            {
                return NotFound();
            }
            ViewData["ActivityId"] = new SelectList(_context.Activities, "Id", "Title", activityProtection.ActivityId);
            ViewData["StudentId"] = new SelectList(_context.Students, "GradebookNumber", "FullName", activityProtection.StudentId);
            return View(activityProtection);
        }

        // POST: ActivityProtections/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,StudentId,ActivityId,ProtectionDate,Points")] ActivityProtection activityProtection)
        {
            if (id != activityProtection.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(activityProtection);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActivityProtectionExists(activityProtection.Id))
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
            ViewData["ActivityId"] = new SelectList(_context.Activities, "Id", "Title", activityProtection.ActivityId);
            ViewData["StudentId"] = new SelectList(_context.Students, "GradebookNumber", "FullName", activityProtection.StudentId);
            return View(activityProtection);
        }

        // GET: ActivityProtections/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activityProtection = await _context.ActivityProtections
                .Include(a => a.Activity)
                .Include(a => a.Student)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (activityProtection == null)
            {
                return NotFound();
            }

            return View(activityProtection);
        }

        // POST: ActivityProtections/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var activityProtection = await _context.ActivityProtections.FindAsync(id);
            _context.ActivityProtections.Remove(activityProtection);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ActivityProtectionExists(int id)
        {
            return _context.ActivityProtections.Any(e => e.Id == id);
        }
    }
}
