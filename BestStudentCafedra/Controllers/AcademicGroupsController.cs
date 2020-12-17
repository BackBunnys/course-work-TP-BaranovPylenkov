using BestStudentCafedra.Data;
using BestStudentCafedra.Models;
using Microsoft.AspNetCore.Http;
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

        public AcademicGroupsController(SubjectAreaDbContext context)
        {
            _context = context;
        }

        // GET: AcademicGroups
        public async Task<IActionResult> Index()
        {
            var groups = _context.AcademicGroups
                .Include(s => s.Students)
                .Include(s => s.Specialty);

            return View(await groups.ToListAsync());
        }

        // GET: AcademicGroups/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var groups = await _context.AcademicGroups
                .Include(s => s.Students)
                .Include(s => s.Specialty)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (groups == null)
            {
                return NotFound();
            }

            return View(groups);
        }

        // GET: AcademicGroups/Create
        public ActionResult Create()
        {
            ViewData["SpecialtyId"] = new SelectList(_context.Specialties.OrderBy(c => c.Code), "Code", "Code");
            return View();
        }

        // POST: AcademicGroups/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SpecialtyId,Name,FormationYear")] AcademicGroup group)
        {
            if (ModelState.IsValid)
            {
                _context.Add(group);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["SpecialtyId"] = new SelectList(_context.Specialties.OrderBy(c => c.Code), "Code", "Code");
            return View(group);
        }

        // GET: AcademicGroups/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var group = await _context.AcademicGroups.FindAsync(id);
            if (group == null)
            {
                return NotFound();
            }

            ViewData["SpecialtyId"] = new SelectList(_context.Specialties.OrderBy(c => c.Code), "Code", "Code", group.SpecialtyId);
            return View(group);
        }

        // POST: AcademicGroups/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
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

            ViewData["SpecialtyId"] = new SelectList(_context.Specialties.OrderBy(c => c.Code), "Code", "Code", group.SpecialtyId);
            return View(group);
        }

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
