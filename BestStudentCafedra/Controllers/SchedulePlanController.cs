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
            List<AcademicGroup> academicGroups = await _context.AcademicGroups.Where(x => x.SchedulePlans.Count == 0).ToListAsync();
            academicGroups.Insert(0, new AcademicGroup { Id = int.MinValue, Name = "Выберите группу..." });
            ViewData["GroupId"] = new SelectList(academicGroups, "Id", "Name");
            ViewData["SchedulePlanes"] = await _context.SchedulePlans.Include(x => x.Group).OrderBy(x => x.ApprovedDate).ToListAsync();
            return View("Index");
        }

        // GET: SchedulePlan/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || !SchedulePlanExists((int)id))
                return NotFound();

            var schedulePlan = await _context.SchedulePlans
                .Include(s => s.Group)
                .Include(s => s.Events)
                .FirstOrDefaultAsync(m => m.Id == id);

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
                return RedirectToAction(nameof(Edit), new { id = schedulePlan.Id });
            }
            return await Index();
        }

        public async Task<IActionResult> Fill(int? id)
        {
            if (id == null || !SchedulePlanExists((int)id)) 
                return NotFound();

            var events = await _context.EventTemplates.OrderBy(x => x.SequentialNumber).ToListAsync();
            ViewData["schedulePlanId"] = id;
            return PartialView("_Fill", events);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Fill(int id)
        {
            if (SchedulePlanExists(id))
            {
                ICollection<Event> events = new List<Event>();
                foreach (var eventTemplate in _context.EventTemplates.OrderBy(x => x.SequentialNumber))
                    events.Add(new Event { EventDescription = eventTemplate.Description, SchedulePlanId = id });
                
                await _context.AddRangeAsync(events);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Edit), new { id = id });
            }
            return NotFound();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(int id)
        {
            if (SchedulePlanExists((int)id))
            {
                SchedulePlan schedulePlan = await _context.SchedulePlans.FindAsync(id);
                String name = User.Identity.Name;
                User user = await _userManager.FindByNameAsync(name);
                schedulePlan.Approve(user.SecondName + " " + user.FirstName[0] + "." + user.MiddleName?[0] + ".", DateTime.Now);
                _context.Update(schedulePlan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return NotFound();
        }

        // GET: SchedulePlan/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || !SchedulePlanExists((int)id))
            {
                return NotFound();
            }

            var schedulePlan = await _context.SchedulePlans
                .Include(x => x.Group)
                .Include(x => x.Events)
                .ThenInclude(x => x.ResponsibleTeacher)
                .Include(x => x.Events)
                .ThenInclude(x => x.EventLogs)
                .FirstOrDefaultAsync(x => x.Id == id);

            var teachers = await _context.Teachers.OrderBy(x => x.FullName).ToListAsync();
            teachers.Insert(0, new Teacher { Id = int.MinValue, FullName = "Выберите преподавателя..." });
            ViewData["Teachers"] = teachers;
            schedulePlan.Events = schedulePlan.Events.Where(x => x.Date != null).OrderBy(x => x.Date).Union(schedulePlan.Events.Where(x => x.Date == null)).ToList();
            ViewData["SchedulePlan"] = schedulePlan;
            return View("Edit", new Event() { SchedulePlanId = (int)id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(List<Event> events, int id)
        {
            if (SchedulePlanExists(id))
            {
                var schedulePlan = _context.SchedulePlans.FirstOrDefault(x => x.Id == id);
                schedulePlan.LastChangedDate = DateTime.Now;
                events.ForEach(x => x.ResponsibleTeacherId = x.ResponsibleTeacherId == int.MinValue ? null : x.ResponsibleTeacherId);
                _context.UpdateRange(events);
                _context.Update(schedulePlan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Edit), new { id = id });
            }
            return await Edit(id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEvent([Bind("SchedulePlanId,EventDescription")] Event e)
        {
            if (ModelState.IsValid)
            {
                _context.Add(e);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Edit), new { id = e.SchedulePlanId });
            }
            return await Edit(e.SchedulePlanId);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteEvent(int eventId)
        {
            var ev = await _context.Events.FindAsync(eventId);
            if (ev == null) return NotFound();
            if (!_context.EventLogs.Any(x => x.EventId == ev.Id))
            {
                _context.Remove(ev);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Edit), new { id = ev.SchedulePlanId });
            }
            ModelState.AddModelError("EventLogs", "Невозможно удалить мероприятиe, по которому проставлены оценки.");
            return await Edit(ev.SchedulePlanId);
        }


        // GET: SchedulePlan/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || !SchedulePlanExists((int)id))
            {
                return NotFound();
            }

            var schedulePlan = await _context.SchedulePlans
                .Include(s => s.Group)
                .FirstOrDefaultAsync(m => m.Id == id);

            return View(schedulePlan);
        }

        // POST: SchedulePlan/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var schedulePlan = await _context.SchedulePlans.FindAsync(id);
            if (!_context.Events.Include(x => x.EventLogs).Where(x => x.SchedulePlanId == id).Any(x => x.EventLogs.Count > 0))
            {
                _context.SchedulePlans.Remove(schedulePlan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ModelState.AddModelError("", "Невозможно удалить план-график, мероприятия для которого были оценены.");
            return await Edit(id);
        }

        private bool SchedulePlanExists(int id)
        {
            return _context.SchedulePlans.Any(e => e.Id == id);
        }
    }
}
