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
    public class SchedulePlanController : Controller
    {
        private readonly SubjectAreaDbContext _context;

        public SchedulePlanController(SubjectAreaDbContext context)
        {
            _context = context;
        }

        // GET: SchedulePlan
        public async Task<IActionResult> Index()
        {
            var subjectAreaDbContext = _context.SchedulePlan.Include(s => s.Group);
            return View(await subjectAreaDbContext.ToListAsync());
        }

        // GET: SchedulePlan/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var schedulePlan = await _context.SchedulePlan
                .Include(s => s.Group)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (schedulePlan == null)
            {
                return NotFound();
            }

            return View(schedulePlan);
        }

        // GET: SchedulePlan/Create
        public IActionResult Create()
        {
            ViewData["GroupId"] = new SelectList(_context.AcademicGroups, "Id", "Name");
            return View();
        }

        // POST: SchedulePlan/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,GroupId,ApprovedDate,LastChangedDate,ApprovingOfficerName")] SchedulePlan schedulePlan)
        {
            if (ModelState.IsValid)
            {
                _context.Add(schedulePlan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GroupId"] = new SelectList(_context.AcademicGroups, "Id", "Name", schedulePlan.GroupId);
            return View(schedulePlan);
        }

        // GET: SchedulePlan/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var schedulePlan = await _context.SchedulePlan.FindAsync(id);
            if (schedulePlan == null)
            {
                return NotFound();
            }
            ViewData["GroupId"] = new SelectList(_context.AcademicGroups, "Id", "Name", schedulePlan.GroupId);
            return View(schedulePlan);
        }

        // POST: SchedulePlan/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,GroupId,ApprovedDate,LastChangedDate,ApprovingOfficerName")] SchedulePlan schedulePlan)
        {
            if (id != schedulePlan.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(schedulePlan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SchedulePlanExists(schedulePlan.Id))
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
            ViewData["GroupId"] = new SelectList(_context.AcademicGroups, "Id", "Name", schedulePlan.GroupId);
            return View(schedulePlan);
        }

        // GET: SchedulePlan/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var schedulePlan = await _context.SchedulePlan
                .Include(s => s.Group)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (schedulePlan == null)
            {
                return NotFound();
            }

            return View(schedulePlan);
        }

        // POST: SchedulePlan/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var schedulePlan = await _context.SchedulePlan.FindAsync(id);
            _context.SchedulePlan.Remove(schedulePlan);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SchedulePlanExists(int id)
        {
            return _context.SchedulePlan.Any(e => e.Id == id);
        }
    }
}
