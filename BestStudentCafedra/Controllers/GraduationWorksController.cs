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
using BestStudentCafedra.Models.ViewModels;

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

            if (User.IsInRole("student"))
            {
                GraduationWork graduationWork = await _context.GraduationWorks.FirstOrDefaultAsync(x => x.StudentId == user.SubjectAreaId);
                return await Details(graduationWork?.Id);
            }

            IQueryable<GraduationWork> graduationWorks = _context.GraduationWorks
                    .Include(x => x.Student.Group.SchedulePlans.Where(x => x.ApprovedDate != null))
                    .ThenInclude(x => x.Events.Where(x => x.Date <= DateTime.Now))
                    .Include(x => x.EventLogs)
                    .Include(x => x.Reviewer)
                    .Include(x => x.ScientificAdviser);

            if (User.IsInRole("teacher")) 
                graduationWorks = graduationWorks.Where(x => x.ScientificAdviserId == user.SubjectAreaId);

            return View(await graduationWorks.OrderByDescending(x => x.ArchievedDate).ThenBy(x => x.Student.FullName).ToListAsync());
        }

        // GET: GraduationWorks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || !GraduationWorkExists((int)id))
            {
                return NotFound();
            }

            var graduationWork = await _context.GraduationWorks
                .Include(g => g.Student)
                .ThenInclude(g => g.Group.SchedulePlans)
                .ThenInclude(g => g.Events.Where(e => e.Date != null))
                .ThenInclude(g => g.EventLogs.Where(e => e.GraduationWorkId == id))
                .Include(x => x.Reviewer)
                .Include(x => x.ScientificAdviser)
                .FirstOrDefaultAsync(m => m.Id == id);

            return View("Details", graduationWork);
        }

        // GET: GraduationWorks/Create
        public IActionResult Create()
        {
            ViewData["StudentId"] = new SelectList(_context.Students.Include(x => x.GraduationWorks).Where(x => x.GraduationWorks.Count == 0), "GradebookNumber", "FullName");
            ViewData["ScientificAdviserId"] = new SelectList(_context.Teachers, "Id", "FullName");
            ViewData["ReviewerId"] = new SelectList(_context.Teachers, "Id", "FullName");
            return View();
        }

        // POST: GraduationWorks/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StudentId, Theme, ScientificAdviserId, ReviewerId")]GraduationWork graduationWork)
        {
            if (ModelState.IsValid)
            {
                _context.Add(graduationWork);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["StudentId"] = new SelectList(_context.Students, "GradebookNumber", "FullName", graduationWork.StudentId);
            ViewData["ScientificAdviserId"] = new SelectList(_context.Teachers, "Id", "FullName", graduationWork.ScientificAdviserId);
            ViewData["ReviewerId"] = new SelectList(_context.Teachers, "Id", "FullName", graduationWork.ReviewerId);
            return View(graduationWork);
        }

        // GET: GraduationWorks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || !GraduationWorkExists((int)id))
            {
                return NotFound();
            }

            var graduationWork = await _context.GraduationWorks.Include(x => x.Student).FirstOrDefaultAsync(x => x.Id == id);

            ViewData["StudentId"] = new SelectList(_context.Students, "GradebookNumber", "FullName", graduationWork.StudentId);
            ViewData["ScientificAdviserId"] = new SelectList(_context.Teachers, "Id", "FullName", graduationWork.ScientificAdviserId);
            ViewData["ReviewerId"] = new SelectList(_context.Teachers, "Id", "FullName", graduationWork.ReviewerId);
            return View(graduationWork);
        }

        // POST: GraduationWorks/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,StudentId,Theme,ScientificAdviserId,ReviewerId")]GraduationWork graduationWork)
        {
            if (id != graduationWork.Id || !GraduationWorkExists(id))
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                 _context.Update(graduationWork);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            ViewData["StudentId"] = new SelectList(_context.Students, "GradebookNumber", "FullName", graduationWork.StudentId);
            ViewData["ScientificAdviserId"] = new SelectList(_context.Teachers, "Id", "FullName", graduationWork.ScientificAdviserId);
            ViewData["ReviewerId"] = new SelectList(_context.Teachers, "Id", "FullName", graduationWork.ReviewerId);
            return View(graduationWork);
        }

        // GET: GraduationWorks/Archive/5
        public async Task<IActionResult> Archive(int? id)
        {
            if (id == null || !GraduationWorkExists((int)id))
            {
                return NotFound();
            }

            var graduationWork = await _context.GraduationWorks.Include(x => x.Student).FirstOrDefaultAsync(x => x.Id == id);

            return PartialView("_Archive", new ArchiveWorkViewModel { GraduationWorkId = graduationWork.Id, GraduationWork = graduationWork, ArchievedDate = DateTime.Now });
        }

        // POST: GraduationWorks/Archive/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Archive(int id, [Bind("GraduationWorkId,ArchievedDate,Result")]ArchiveWorkViewModel archiveViewModel)
        {
            if (id != archiveViewModel.GraduationWorkId || !GraduationWorkExists(id))
            {
                return NotFound();
            }

            var gw = await _context.GraduationWorks.Include(x => x.EventLogs)
                .Include(x => x.Student.Group.SchedulePlans.Where(x => x.ApprovedDate != null))
                .ThenInclude(x => x.Events)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (gw.EventLogs.Count < gw.Student.Group.SchedulePlans.FirstOrDefault()?.Events.Count)
                ModelState.AddModelError("", "Нельзя отправить в архив работу, не прошедшую все мероприятия.");

            if (ModelState.IsValid)
            {
                gw.Archive(archiveViewModel.Result, archiveViewModel.ArchievedDate);

                _context.Update(gw);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Details), new { id = id });
            }

            return await Details(id);
        }

        // GET: GraduationWorks/Unarchive/5
        public async Task<IActionResult> Unarchive(int? id)
        {
            if (id == null || !GraduationWorkExists((int)id))
            {
                return NotFound();
            }

            var graduationWork = await _context.GraduationWorks.FindAsync(id);

            return PartialView("_Unarchive", new ArchiveWorkViewModel
            {
                GraduationWorkId = graduationWork.Id,
                ArchievedDate = (DateTime)graduationWork.ArchievedDate,
                Result = (int)graduationWork.Result
            });
        }

        // POST: GraduationWorks/Unarchive/5
        [HttpPost, ActionName("Unarchive")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UnarchiveConfirmed(int id)
        {
            if (!GraduationWorkExists(id))
            {
                return NotFound();
            }

            var gw = await _context.GraduationWorks.Include(x => x.EventLogs)
                .Include(x => x.Student)
                .FirstOrDefaultAsync(x => x.Id == id);

            gw.Unarchive();

            _context.Update(gw);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = id });
        }

        // GET: GraduationWorks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || !GraduationWorkExists((int)id))
            {
                return NotFound();
            }

            var graduationWork = await _context.GraduationWorks
                .Include(g => g.Student)
                .Include(g => g.ScientificAdviser)
                .Include(g => g.Reviewer)
                .FirstOrDefaultAsync(m => m.Id == id);

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
