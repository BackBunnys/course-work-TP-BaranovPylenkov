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
            List<AcademicGroup> academicGroups = await _context.AcademicGroups.ToListAsync();
            academicGroups.Insert(0, new AcademicGroup { Id = int.MinValue, Name = "Выберите группу..." });
            ViewData["GroupId"] = new SelectList(academicGroups, "Id", "Name");
            ViewData["SchedulePlanes"] = await _context.SchedulePlans.OrderBy(x => x.ApprovedDate).ToListAsync();
            return View();
        }

        // GET: SchedulePlan/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var schedulePlan = await _context.SchedulePlans
                .Include(s => s.Group)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (schedulePlan == null)
            {
                return NotFound();
            }

            return View(schedulePlan);
        }

        // POST: SchedulePlan/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GroupId")] SchedulePlan schedulePlan)
        {
            if (ModelState.IsValid)
            {
                _context.Add(schedulePlan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            List<AcademicGroup> academicGroups = await _context.AcademicGroups.ToListAsync();
            academicGroups.Insert(0, new AcademicGroup { Id = int.MinValue, Name = "Выберите группу..." });
            ViewData["GroupId"] = new SelectList(academicGroups, "Id", "Name");
            ViewData["SchedulePlanes"] = await _context.SchedulePlans.OrderBy(x => x.ApprovedDate).ToListAsync();
            return View(nameof(Index), schedulePlan);
        }

        // GET: SchedulePlan/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var schedulePlan = await _context.SchedulePlans.FindAsync(id);
            if (schedulePlan == null)
            {
                return NotFound();
            }
            ViewData["GroupId"] = new SelectList(_context.AcademicGroups.ToList(), "Id", "Name");
            return View(schedulePlan);
        }

        // POST: SchedulePlan/Edit/5
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
            List<AcademicGroup> academicGroups = await _context.AcademicGroups.ToListAsync();
            academicGroups.Insert(0, new AcademicGroup { Id = int.MinValue, Name = "Выберите группу..." });
            ViewData["GroupId"] = new SelectList(academicGroups, "Id", "Name");
            return View(schedulePlan);
        }

        // GET: SchedulePlan/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var schedulePlan = await _context.SchedulePlans
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
            var schedulePlan = await _context.SchedulePlans.FindAsync(id);
            _context.SchedulePlans.Remove(schedulePlan);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SchedulePlanExists(int id)
        {
            return _context.SchedulePlans.Any(e => e.Id == id);
        }
    }
}
