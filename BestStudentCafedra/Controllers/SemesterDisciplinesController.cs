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
    public class SemesterDisciplinesController : Controller
    {
        private readonly SubjectAreaDbContext _context;

        public SemesterDisciplinesController(SubjectAreaDbContext context)
        {
            _context = context;
        }

        // GET: SemesterDisciplines/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var semesterDiscipline = await _context.SemesterDiscipline
                .Include(s => s.Discipline)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (semesterDiscipline == null)
            {
                return NotFound();
            }

            return View(semesterDiscipline);
        }

        // GET: SemesterDisciplines/Create
        public async Task<IActionResult> Create(int id, string returnUrl)
        {
            ViewData["returnUrl"] = returnUrl;
            Discipline discipline = await _context.Disciplines.FirstOrDefaultAsync(d => d.Id == id);
            SemesterDiscipline semesterDiscipline = new SemesterDiscipline() { Discipline = discipline };
            return View(semesterDiscipline);
        }

        // POST: SemesterDisciplines/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DisciplineId,Year,Semester,ControlType")] SemesterDiscipline semesterDiscipline, string returnUrl)
        {
            ViewData["returnUrl"] = returnUrl;
            semesterDiscipline.Discipline = await _context.Disciplines.FirstOrDefaultAsync(d => d.Id == semesterDiscipline.DisciplineId);

            if (_context.SemesterDiscipline
                .Where(x => x.DisciplineId == semesterDiscipline.DisciplineId && 
                x.Year == semesterDiscipline.Year &&
                x.Semester == semesterDiscipline.Semester).Count() > 0)
            {
                ModelState.AddModelError("", "Семестровая дисциплина с таким курсом и семестром уже существует!");
                return View(semesterDiscipline);
            }

            if (ModelState.IsValid)
            {
                _context.Add(semesterDiscipline);
                await _context.SaveChangesAsync();
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(semesterDiscipline);
        }

        // GET: SemesterDisciplines/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var semesterDiscipline = await _context.SemesterDiscipline.FindAsync(id);
            if (semesterDiscipline == null)
            {
                return NotFound();
            }
            ViewData["DisciplineId"] = new SelectList(_context.Disciplines, "Id", "Name", semesterDiscipline.DisciplineId);
            return View(semesterDiscipline);
        }

        // POST: SemesterDisciplines/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DisciplineId,Year,Semester,ControlType")] SemesterDiscipline semesterDiscipline)
        {
            if (id != semesterDiscipline.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(semesterDiscipline);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SemesterDisciplineExists(semesterDiscipline.Id))
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
            ViewData["DisciplineId"] = new SelectList(_context.Disciplines, "Id", "Name", semesterDiscipline.DisciplineId);
            return View(semesterDiscipline);
        }

        // GET: SemesterDisciplines/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var semesterDiscipline = await _context.SemesterDiscipline
                .Include(s => s.Discipline)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (semesterDiscipline == null)
            {
                return NotFound();
            }

            return View(semesterDiscipline);
        }

        // POST: SemesterDisciplines/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var semesterDiscipline = await _context.SemesterDiscipline.FindAsync(id);
            _context.SemesterDiscipline.Remove(semesterDiscipline);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SemesterDisciplineExists(int id)
        {
            return _context.SemesterDiscipline.Any(e => e.Id == id);
        }
    }
}
