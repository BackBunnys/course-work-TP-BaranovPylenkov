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
    public class RatingControlsController : Controller
    {
        private readonly SubjectAreaDbContext _context;

        public RatingControlsController(SubjectAreaDbContext context)
        {
            _context = context;
        }

        // GET: RatingControls
        public async Task<IActionResult> Index()
        {
            var subjectAreaDbContext = _context.RatingControls.Include(r => r.SemesterDiscipline);
            return View(await subjectAreaDbContext.ToListAsync());
        }

        // GET: RatingControls/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ratingControl = await _context.RatingControls
                .Include(r => r.SemesterDiscipline)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ratingControl == null)
            {
                return NotFound();
            }

            return View(ratingControl);
        }

        // GET: RatingControls/Create
        public IActionResult Create()
        {
            ViewData["SemesterDisciplineId"] = new SelectList(_context.SemesterDiscipline, "Id", "Id");
            return View();
        }

        // POST: RatingControls/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SemesterDisciplineId,Number,CompletionDate")] RatingControl ratingControl)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ratingControl);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SemesterDisciplineId"] = new SelectList(_context.SemesterDiscipline, "Id", "Id", ratingControl.SemesterDisciplineId);
            return View(ratingControl);
        }

        // GET: RatingControls/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ratingControl = await _context.RatingControls.FindAsync(id);
            if (ratingControl == null)
            {
                return NotFound();
            }
            ViewData["SemesterDisciplineId"] = new SelectList(_context.SemesterDiscipline, "Id", "Id", ratingControl.SemesterDisciplineId);
            return View(ratingControl);
        }

        // POST: RatingControls/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SemesterDisciplineId,Number,CompletionDate")] RatingControl ratingControl)
        {
            if (id != ratingControl.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ratingControl);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RatingControlExists(ratingControl.Id))
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
            ViewData["SemesterDisciplineId"] = new SelectList(_context.SemesterDiscipline, "Id", "Id", ratingControl.SemesterDisciplineId);
            return View(ratingControl);
        }

        // GET: RatingControls/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ratingControl = await _context.RatingControls
                .Include(r => r.SemesterDiscipline)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ratingControl == null)
            {
                return NotFound();
            }

            return View(ratingControl);
        }

        // POST: RatingControls/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ratingControl = await _context.RatingControls.FindAsync(id);
            _context.RatingControls.Remove(ratingControl);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RatingControlExists(int id)
        {
            return _context.RatingControls.Any(e => e.Id == id);
        }
    }
}
