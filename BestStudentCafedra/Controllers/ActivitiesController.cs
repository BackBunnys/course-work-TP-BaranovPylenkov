using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BestStudentCafedra.Data;
using BestStudentCafedra.Models;
using BestStudentCafedra.Models.ViewModels;

namespace BestStudentCafedra.Controllers
{
    public class ActivitiesController : Controller
    {
        private readonly SubjectAreaDbContext _context;

        public ActivitiesController(SubjectAreaDbContext context)
        {
            _context = context;
        }

        // GET: Activities
        public async Task<IActionResult> Index()
        {
            var subjectAreaDbContext = _context.Activities.Include(a => a.SemesterDiscipline).Include(a => a.Type);
            return View(await subjectAreaDbContext.ToListAsync());
        }

        // GET: Activities/Details/5
        public async Task<IActionResult> Protect(int? id, int? groupId)
        {
            if (id == null || !ActivityExists((int)id))
            {
                return NotFound();
            }

            var activity = await _context.Activities
                .Include(a => a.SemesterDiscipline)
                    .ThenInclude(b => b.Discipline)
                    .ThenInclude(b => b.GroupDiscipline)
                .Include(a => a.Type)
                .FirstOrDefaultAsync(m => m.Id == id);

            var groups = _context.AcademicGroups
                .Include(x => x.GroupDiscipline)
                .Where(x => x.GroupDiscipline.Any(y => y.DisciplineId == activity.SemesterDiscipline.DisciplineId))
                .AsNoTracking().ToList();

            ViewData["GroupId"] = new SelectList(groups, "Id", "Name", groupId);

            if (groupId != null)
            {
                groups = groups.Where(x => x.Id == groupId).ToList();
            }

            var students = _context.Students
                .Include(x => x.ActivityProtections.Where(y => y.ActivityId == id).OrderByDescending(y => y.ProtectionDate))
                .Include(x => x.Group)
                .OrderBy(x => x.Group.Name)
                .ThenBy(x => x.FullName)
                .Where(x => groups.Contains(x.Group))
                .AsNoTracking()
                .ToList();

            var activityProtections = new StudentActivityViewModel() { Activity = activity, Students = students };

            return View(activityProtections);
        }

        // POST: Activities/Details/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Protect(int? id,[Bind("Id,StudentId,ActivityId,Points")] ActivityProtection activityProtection, int? groupId)
        {
            if (ModelState.IsValid)
            {
                if (activityProtection.Points > _context.Activities.Find(activityProtection.ActivityId).MaxPoints || activityProtection.Points < 0)
                {
                    ModelState.AddModelError($"student-{activityProtection.StudentId}", "Оценка должна быть в диапозоне от 0 до " + _context.Activities.Find(activityProtection.ActivityId).MaxPoints);
                }

                activityProtection.ProtectionDate = DateTime.Now;
                if (_context.ActivityProtections.Any(x => x.ActivityId == activityProtection.ActivityId && x.StudentId == activityProtection.StudentId))
                {
                    try
                    {
                        _context.Update(activityProtection);
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
                }
                else
                {
                    activityProtection.Id = 0;
                    _context.Add(activityProtection);
                }
                await _context.SaveChangesAsync();
            }
            return await Protect(id, groupId);
        }

        // GET: Activities/Create
        public IActionResult Create(int SemesterDisciplineId)
        {
            ViewData["TypeId"] = new SelectList(_context.ActivityTypes, "Id", "Name");
            ViewData["SemesterDisciplineId"] = SemesterDisciplineId;
            return View();
        }

        // POST: Activities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TypeId,SemesterDisciplineId,Number,Title,MaxPoints")] Activity activity)
        {
            if (ModelState.IsValid)
            {
                _context.Add(activity);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "SemesterDisciplines", new { id = activity.SemesterDisciplineId });
            }
            ViewData["SemesterDisciplineId"] = new SelectList(_context.SemesterDiscipline, "Id", "Id", activity.SemesterDisciplineId);
            ViewData["TypeId"] = new SelectList(_context.ActivityTypes, "Id", "Name", activity.TypeId);
            return View(activity);
        }

        // GET: Activities/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activity = await _context.Activities.FindAsync(id);
            if (activity == null)
            {
                return NotFound();
            }
            ViewData["SemesterDisciplineId"] = new SelectList(_context.SemesterDiscipline, "Id", "Id", activity.SemesterDisciplineId);
            ViewData["TypeId"] = new SelectList(_context.ActivityTypes, "Id", "Name", activity.TypeId);
            return View(activity);
        }

        // POST: Activities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TypeId,SemesterDisciplineId,Number,Title,MaxPoints")] Activity activity)
        {
            if (id != activity.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(activity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActivityExists(activity.Id))
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
            ViewData["TypeId"] = new SelectList(_context.ActivityTypes, "Id", "Name", activity.TypeId);
            return View(activity);
        }

        // GET: Activities/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activity = await _context.Activities
                .Include(a => a.SemesterDiscipline)
                .Include(a => a.Type)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (activity == null)
            {
                return NotFound();
            }

            return View(activity);
        }

        // POST: Activities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var activity = await _context.Activities.FindAsync(id);
            _context.Activities.Remove(activity);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "SemesterDisciplines", new { id = activity.SemesterDisciplineId });
        }

        private bool ActivityExists(int id)
        {
            return _context.Activities.Any(e => e.Id == id);
        }

        private bool ActivityProtectionExists(int id)
        {
            return _context.ActivityProtections.Any(e => e.Id == id);
        }
    }
}
