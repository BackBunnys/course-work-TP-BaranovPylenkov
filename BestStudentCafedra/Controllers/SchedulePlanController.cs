﻿using System;
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
using BestStudentCafedra.Services.Messages.Email;

namespace BestStudentCafedra.Controllers
{
    public class SchedulePlanController : Controller
    {
        private readonly SubjectAreaDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly EmailSender _emailService;

        public SchedulePlanController(SubjectAreaDbContext context, UserManager<User> userManager, EmailSender emailService)
        {
            _context = context;
            _userManager = userManager;
            _emailService = emailService;
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
                .ThenInclude(s => s.ResponsibleTeacher)
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
        public async Task<IActionResult> Approve(int id, bool students, bool teachers, bool advisers)
        {
            if (SchedulePlanExists(id))
            {
                SchedulePlan schedulePlan = await _context.SchedulePlans.FindAsync(id);
                String name = User.Identity.Name;
                User user = await _userManager.FindByNameAsync(name);
                schedulePlan.Approve(user.SecondName + " " + user.FirstName[0] + "." + user.MiddleName?[0] + ".", DateTime.Now);
                _context.Update(schedulePlan);
                await _context.SaveChangesAsync();

                await MailingAsync(id, students, teachers, advisers);

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
        public async Task<IActionResult> Edit(List<Event> events, int id, bool students, bool teachers, bool advisers)
        {
            if (SchedulePlanExists(id))
            {
                var schedulePlan = _context.SchedulePlans.FirstOrDefault(x => x.Id == id);
                schedulePlan.LastChangedDate = DateTime.Now;
                events.ForEach(x => x.ResponsibleTeacherId = x.ResponsibleTeacherId == int.MinValue ? null : x.ResponsibleTeacherId);
                _context.UpdateRange(events);
                _context.Update(schedulePlan);
                await _context.SaveChangesAsync();

                await MailingAsync(id, students, teachers, advisers);

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

        private async Task MailingAsync(int schedulePlanId, bool sendToStudents, bool sendToResponsibleTeachers, bool sendToScientificAdvisers)
        {
            if (sendToStudents) {
                var studentsId = (await _context.SchedulePlans
                    .Include(x => x.Group.Students)
                    .FirstOrDefaultAsync(x => x.Id == schedulePlanId))
                    .Group.Students.Select(x => x.GradebookNumber)
                    .Distinct();

                var studentsEmail = (await _userManager.GetUsersInRoleAsync("student"))
                    .Where(x => x.SubjectAreaId != null && studentsId.Contains((int)x.SubjectAreaId))
                    .Select(x => x.Email)
                    .ToList();

                EmailMessage emailMessage = new EmailMessage { Subject = "ВКР - план-график" };
                if (studentsEmail.Count > 0)
                {
                    emailMessage.Content = "План-график на вашу группу был изменён. Для подробностей пройдите по ссылке: " + Url.ActionLink("Details", "SchedulePlan", new { id = schedulePlanId });
                    await _emailService.SendAsync(studentsEmail, emailMessage);
                }
            }
            if(sendToResponsibleTeachers)
            {
                var teachersId = (await _context.SchedulePlans
                    .Include(x => x.Events.Where(x => x.ResponsibleTeacherId != null))
                    .FirstOrDefaultAsync(x => x.Id == schedulePlanId)).Events.Select(x => x.ResponsibleTeacherId)
                    .Distinct();

                var teachersEmail = (await _userManager.GetUsersInRoleAsync("teacher"))
                    .Where(x => x.SubjectAreaId != null && teachersId.Contains((int)x.SubjectAreaId))
                    .Select(x => x.Email)
                    .ToList();

                EmailMessage emailMessage = new EmailMessage { Subject = "ВКР - план-график" };
                if (teachersEmail.Count > 0)
                {
                    emailMessage.Content = "План-график, в одном или нескольких мероприятиях которого вы являетесь ответственным преподавателем, был изменён. Для подробностей пройдите по ссылке: " + Url.ActionLink("Details", "SchedulePlan", new { id = schedulePlanId });
                    await _emailService.SendAsync(teachersEmail, emailMessage);
                }
            }
            if(sendToScientificAdvisers)
            {
                var teachersId = (await _context.SchedulePlans
                    .Include(x => x.Group.Students)
                        .ThenInclude(x => x.GraduationWorks)
                    .FirstOrDefaultAsync(x => x.Id == schedulePlanId))
                    .Group.Students.Where(x => x.GraduationWorks.FirstOrDefault()?.ScientificAdviserId != null)
                    .Select(x => x.GraduationWorks.FirstOrDefault().ScientificAdviserId)
                    .Distinct();

                var teachersEmail = (await _userManager.GetUsersInRoleAsync("teacher"))
                    .Where(x => x.SubjectAreaId != null && teachersId.Contains((int)x.SubjectAreaId))
                    .Select(x => x.Email)
                    .ToList();

                EmailMessage emailMessage = new EmailMessage { Subject = "ВКР - план-график" };
                if (teachersEmail.Count > 0)
                {
                    emailMessage.Content = "План-график одной из работ, которыми вы руководите, был изменён. Для подробностей пройдите по ссылке: " + Url.ActionLink("Details", "SchedulePlan", new { id = schedulePlanId });
                    await _emailService.SendAsync(teachersEmail, emailMessage);
                }
            }

        }
    }
}
