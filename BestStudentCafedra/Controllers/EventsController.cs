﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BestStudentCafedra.Data;
using BestStudentCafedra.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using BestStudentCafedra.Models.ViewModels;

namespace BestStudentCafedra.Controllers
{
    public class EventsController : Controller
    {
        private readonly SubjectAreaDbContext _context;
        private readonly UserManager<User> _userManager;

        public EventsController(SubjectAreaDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        // GET: Events
        [Authorize(Roles = "teacher")]
        public async Task<IActionResult> Index()
        {
            //Selecting teacher
            User user = await _userManager.FindByNameAsync(User.Identity.Name);

            var events = await GetEventsForTeacher(user.SubjectAreaId);

            return View(events);
        }

        [Authorize(Roles = "teacher")]
        public async Task<IActionResult> Mark(int? id)
        {
            if (id == null || !EventExists((int)id))
            {
                return NotFound();
            }

            return await Details(id);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "teacher")]
        public async Task<IActionResult> Mark([Bind("GraduationWorkId,EventId,Mark")]EventLog eventLog)
        {
            if (!EventExists(eventLog.EventId))
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Add(eventLog);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Mark), new { id = eventLog.EventId });
            }
            else ModelState.AddModelError($"{eventLog.GraduationWorkId}.Mark", "Не выбран вариант");
            return await Details(eventLog.EventId);
        }

        // GET: Events/Details/5
        [Authorize(Roles = "teacher")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || !EventExists((int)id))
            {
                return NotFound();
            }

            //Selecting teacher
            User user = await _userManager.FindByNameAsync(User.Identity.Name);
            Teacher teacher = await _context.Teachers.FirstOrDefaultAsync(x => x.Id == user.SubjectAreaId);

            var @event = await _context.Events
                .Include(x => x.ResponsibleTeacher)
                .Include(x => x.SchedulePlan)
                .ThenInclude(x => x.Group.Students.Where(x => x.GraduationWorks.Any(x => x.ScientificAdviserId == teacher.Id)))
                .ThenInclude(x => x.GraduationWorks)
                .ThenInclude(x => x.EventLogs.Where(x => x.EventId == id))
                .FirstOrDefaultAsync(m => m.Id == id);
            
            return View(@event);
        }

        // GET: Events/Create
        [Authorize(Roles = "methodist")]
        public IActionResult Create()
        {
            ViewData["ResponsibleTeacherId"] = new SelectList(_context.Teachers, "Id", "FullName");
            ViewData["SchedulePlanId"] = new SelectList(_context.SchedulePlans, "Id", "Id");
            return View();
        }

        // POST: Events/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "methodist")]
        public async Task<IActionResult> Create([Bind("Id,SchedulePlanId,EventDescription,Date,Class,ResponsibleTeacherId")] Event @event)
        {
            if (ModelState.IsValid)
            {
                _context.Add(@event);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ResponsibleTeacherId"] = new SelectList(_context.Teachers, "Id", "FullName", @event.ResponsibleTeacherId);
            ViewData["SchedulePlanId"] = new SelectList(_context.SchedulePlans, "Id", "Id", @event.SchedulePlanId);
            return View(@event);
        }

        // GET: Events/Edit/5
        [Authorize(Roles = "methodist")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events.FindAsync(id);
            if (@event == null)
            {
                return NotFound();
            }
            ViewData["ResponsibleTeacherId"] = new SelectList(_context.Teachers, "Id", "FullName", @event.ResponsibleTeacherId);
            ViewData["SchedulePlanId"] = new SelectList(_context.SchedulePlans, "Id", "Id", @event.SchedulePlanId);
            return View(@event);
        }

        // POST: Events/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "methodist")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SchedulePlanId,EventDescription,Date,Class,ResponsibleTeacherId")] Event @event)
        {
            if (id != @event.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(@event);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventExists(@event.Id))
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
            ViewData["ResponsibleTeacherId"] = new SelectList(_context.Teachers, "Id", "FullName", @event.ResponsibleTeacherId);
            ViewData["SchedulePlanId"] = new SelectList(_context.SchedulePlans, "Id", "Id", @event.SchedulePlanId);
            return View(@event);
        }

        // GET: Events/Delete/5
        [Authorize(Roles = "methodist")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events
                .Include(x => x.ResponsibleTeacher)
                .Include(x => x.SchedulePlan)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "methodist")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @event = await _context.Events.FindAsync(id);
            _context.Events.Remove(@event);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private async Task<ICollection<Event>> GetEventsForTeacher(int? id)
        {
            if (id == null) return new List<Event>();

            var events = await _context.Events
                .Include(x => x.ResponsibleTeacher)
                .Include(x => x.SchedulePlan.Group.Students.Where(x => x.GraduationWorks.Any(x => x.ScientificAdviserId == id)))
                    .ThenInclude(x => x.GraduationWorks)
                    .ThenInclude(x => x.ScientificAdviser)
                .Include(x => x.SchedulePlan.Group.Students)
                    .ThenInclude(x => x.GraduationWorks)
                    .ThenInclude(x => x.Reviewer)
                .Include(x => x.SchedulePlan.Group.Students)
                    .ThenInclude(x => x.GraduationWorks)
                    .ThenInclude(x => x.EventLogs)
                .Where(x => x.Date != null &&
                            x.SchedulePlan.ApprovedDate != null &&
                            x.SchedulePlan.Group.Students.Any(x => x.GraduationWorks.Any(x => x.ScientificAdviserId == id)))
                .AsNoTracking().ToListAsync();

            return events.Distinct().ToList();
        }

        private bool EventExists(int id)
        {
            return _context.Events.Any(e => e.Id == id);
        }
    }
}
