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

namespace BestStudentCafedra.Controllers
{
    public class StudentController : Controller
    {
        private readonly SubjectAreaDbContext _context;

        public StudentController(SubjectAreaDbContext context)
        {
            _context = context;
        }

        // GET: Students
        [Authorize]
        public async Task<IActionResult> Index(string ReturnUrl)
        {
            var subjectAreaDbContext = _context.Students.Include(s => s.Group);
            ViewData["ReturnUrl"] = ReturnUrl;
            return View(await subjectAreaDbContext.ToListAsync());
        }

        // GET: Students/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id, string ReturnUrl, string From)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .Include(s => s.Group)
                .FirstOrDefaultAsync(m => m.GradebookNumber == id);
            if (student == null)
            {
                return NotFound();
            }
            ViewData["ReturnUrl"] = ReturnUrl;
            ViewData["From"] = From;
            return View(student);
        }

        // GET: Students/Create
        [Authorize(Roles = "methodist")]
        public IActionResult Create(string ReturnUrl, int? ForGroup = null)
        {
            ViewData["GroupId"] = new SelectList(_context.AcademicGroups, "Id", "Name", ForGroup);
            ViewData["ReturnUrl"] = ReturnUrl;
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "methodist")]
        public async Task<IActionResult> Create([Bind("GradebookNumber,GroupId,FullName,PhoneNumber")] Student student, string ReturnUrl)
        {
            if (ModelState.IsValid)
            {
                _context.Add(student);
                await _context.SaveChangesAsync();
                if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
                    return Redirect(ReturnUrl);
                else
                    return RedirectToAction("Details", "AcademicGroups", new { id = student.GroupId });
            }
            ViewData["GroupId"] = new SelectList(_context.AcademicGroups, "Id", "Name", student.GroupId);
            ViewData["ReturnUrl"] = ReturnUrl;
            return View(student);
        }

        // GET: Students/Edit/5
        [Authorize(Roles = "methodist")]
        public async Task<IActionResult> Edit(int? id, string ReturnUrl)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            ViewData["GroupId"] = new SelectList(_context.AcademicGroups, "Id", "Name", student.GroupId);
            ViewData["ReturnUrl"] = ReturnUrl;
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "methodist")]
        public async Task<IActionResult> Edit(int id, [Bind("GradebookNumber,GroupId,FullName,PhoneNumber")] Student student, string ReturnUrl)
        {
            if (id != student.GradebookNumber)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.GradebookNumber))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
                    return Redirect(ReturnUrl);
                else
                    return RedirectToAction(nameof(Index));
            }
            ViewData["GroupId"] = new SelectList(_context.AcademicGroups, "Id", "Name", student.GroupId);
            ViewData["ReturnUrl"] = ReturnUrl;
            return View(student);
        }

        // GET: Students/Delete/5
        [Authorize(Roles = "methodist")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .Include(s => s.Group)
                .FirstOrDefaultAsync(m => m.GradebookNumber == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "methodist")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Students.FindAsync(id);
            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.GradebookNumber == id);
        }
    }
}
