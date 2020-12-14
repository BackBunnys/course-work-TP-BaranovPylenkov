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
using Microsoft.AspNetCore.Identity;

namespace BestStudentCafedra.Controllers
{
    public class SchedulePlanController : Controller
    {
        private readonly SubjectAreaDbContext _context;
        private readonly UserManager<User> _userManager;

        public SchedulePlanController(SubjectAreaDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
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
                return RedirectToAction(nameof(Form), new { id = schedulePlan.Id });
            }
            List<AcademicGroup> academicGroups = await _context.AcademicGroups.ToListAsync();
            academicGroups.Insert(0, new AcademicGroup { Id = int.MinValue, Name = "Выберите группу..." });
            ViewData["GroupId"] = new SelectList(academicGroups, "Id", "Name");
            ViewData["SchedulePlanes"] = await _context.SchedulePlans.OrderBy(x => x.ApprovedDate).ToListAsync();
            return View(nameof(Index), schedulePlan);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Fill(int schedulePlanId)
        {
            if (SchedulePlanExists((int)schedulePlanId))
            {
                ICollection<Event> events = new List<Event>();
                foreach (var eventTemplate in _context.EventTemplates.OrderBy(x => x.SequentialNumber))
                    events.Add(new Event { EventDescription = eventTemplate.Description, SchedulePlanId = schedulePlanId });
                
                await _context.AddRangeAsync(events);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Form), new { id = schedulePlanId });
            }
            return NotFound();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(int schedulePlanId)
        {
            if (SchedulePlanExists((int)schedulePlanId))
            {
                SchedulePlan schedulePlan = await _context.SchedulePlans.FindAsync(schedulePlanId);
                String name = User.Identity.Name;
                User user = await _userManager.FindByNameAsync(name);
                schedulePlan.Approve(user.SecondName + " " + user.FirstName[0] + "." + user.MiddleName?[0] + ".");
                _context.Update(schedulePlan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(List<Event> events, int schedulePlanId)
        {
            if (schedulePlanId != null)
            {
                events.ForEach(x => x.ResponsibleTeacherId = x.ResponsibleTeacherId == int.MinValue ? null : x.ResponsibleTeacherId );
                _context.UpdateRange(events);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Form), new { id = schedulePlanId });
            }
            var schedulePlan = await _context.SchedulePlans
                .Include(x => x.Group)
                .Include(x => x.Events)
                .ThenInclude(x => x.ResponsibleTeacher)
                .FirstOrDefaultAsync(x => x.Id == schedulePlanId);
            var teachers = await _context.Teachers.OrderBy(x => x.FullName).ToListAsync();
            teachers.Insert(0, new Teacher { Id = int.MinValue, FullName = "Выберите преподавателя..." });
            ViewData["teachers"] = teachers;
            ViewData["events"] = schedulePlan.Events.Where(x => x.Date != null).OrderBy(x => x.Date).Union(schedulePlan.Events.Where(x => x.Date == null)).ToList();
            ViewData["groupName"] = schedulePlan.Group.Name;
            return View("Form", new Event() { SchedulePlanId = (int)schedulePlanId });
        }

        // GET: SchedulePlan/Edit/5
        public async Task<IActionResult> Form(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var schedulePlan = await _context.SchedulePlans
                .Include(x => x.Group)
                .Include(x => x.Events)
                .ThenInclude(x => x.ResponsibleTeacher)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (schedulePlan == null)
            {
                return NotFound();
            }
            var teachers = await _context.Teachers.OrderBy(x => x.FullName).ToListAsync();
            teachers.Insert(0, new Teacher { Id = int.MinValue, FullName = "Выберите преподавателя..." });
            ViewData["teachers"] = teachers;
            ViewData["events"] = schedulePlan.Events.Where(x => x.Date != null).OrderBy(x => x.Date).Union(schedulePlan.Events.Where(x => x.Date == null)).ToList();
            ViewData["groupName"] = schedulePlan.Group.Name;
            return View(new Event() { SchedulePlanId = (int)id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Form([Bind("SchedulePlanId,EventDescription")] Event e)
        {
            if (ModelState.IsValid)
            {
                _context.Add(e);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Form), e.SchedulePlanId);
            }

            var schedulePlan = await _context.SchedulePlans
                .Include(x => x.Group)
                .Include(x => x.Events)
                .ThenInclude(x => x.ResponsibleTeacher)
                .FirstOrDefaultAsync(x => x.Id == e.SchedulePlanId);
            var teachers = await _context.Teachers.OrderBy(x => x.FullName).ToListAsync();
            teachers.Insert(0, new Teacher { Id = int.MinValue, FullName = "Выберите преподавателя" });
            ViewData["teachers"] = teachers;
            ViewData["events"] = schedulePlan.Events.Where(x => x.Date != null).OrderBy(x => x.Date).Union(schedulePlan.Events.Where(x => x.Date == null)).ToList();
            ViewData["groupName"] = schedulePlan.Group.Name;
            return View(nameof(Form), e);
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
        public async Task<IActionResult> DeleteConfirmed(int schedulePlanId)
        {
            var schedulePlan = await _context.SchedulePlans.FindAsync(schedulePlanId);
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
