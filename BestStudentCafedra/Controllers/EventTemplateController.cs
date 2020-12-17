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
    public class EventTemplateController : Controller
    {
        private readonly SubjectAreaDbContext _context;

        public EventTemplateController(SubjectAreaDbContext context)
        {
            _context = context;
        }

        // GET: EventTemplate
        public async Task<IActionResult> Index(EventTemplate eventTemplate = null)
        {
            ViewData["Events"] = await _context.EventTemplates.OrderBy(x => x.SequentialNumber).ToListAsync();
            return View("Index", eventTemplate == null? new EventTemplate() : eventTemplate);
        }

        // POST: EventTemplate/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SequentialNumber,Description")] EventTemplate eventTemplate)
        {
            if (ModelState.IsValid)
            {
                var eventTemplates = await _context.EventTemplates.ToListAsync();
                eventTemplates.ForEach(x => x.SequentialNumber += x.SequentialNumber >= eventTemplate.SequentialNumber ? +1 : 0);
                _context.UpdateRange(eventTemplates);
                _context.Add(eventTemplate);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ModelState.AddModelError("", "Описание мероприятия должно содержать от 5 до 150 символов");
            return await Index(eventTemplate);
        }

        // POST: EventTemplate/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Up(int id)
        {
            if (!EventTemplateExists(id)) return NotFound();

            var eventTemplate = await _context.EventTemplates.FindAsync(id);
            int min = await _context.EventTemplates.MinAsync(x => x.SequentialNumber);
            if(eventTemplate.SequentialNumber != min)
            {
                var eventBefore = await _context.EventTemplates.Where(x => x.SequentialNumber == eventTemplate.SequentialNumber - 1).FirstOrDefaultAsync();
                ++eventBefore.SequentialNumber;
                --eventTemplate.SequentialNumber;
                _context.UpdateRange(eventTemplate, eventBefore);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ModelState.AddModelError("", "Выбранное мероприятия и так является самым первым в списке.");
            return await Index();
        }

        // POST: EventTemplate/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Down(int id)
        {
            if (!EventTemplateExists(id)) return NotFound();

            var eventTemplate = await _context.EventTemplates.FindAsync(id);
            int max = await _context.EventTemplates.MaxAsync(x => x.SequentialNumber);
            if (eventTemplate.SequentialNumber != max)
            {
                var eventAfter = await _context.EventTemplates.Where(x => x.SequentialNumber == eventTemplate.SequentialNumber + 1).FirstOrDefaultAsync();
                --eventAfter.SequentialNumber;
                ++eventTemplate.SequentialNumber;
                _context.UpdateRange(eventTemplate, eventAfter);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ModelState.AddModelError("", "Выбранное мероприятия и так является самым последним в списке.");
            return await Index();
        }

        // POST: EventTemplate/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (!EventTemplateExists(id)) return NotFound();

            var eventTemplates = await _context.EventTemplates.ToListAsync();
            var eventTemplate = eventTemplates.FirstOrDefault(x => x.Id == id);
            eventTemplates.ForEach(x => x.SequentialNumber += x.SequentialNumber > eventTemplate.SequentialNumber ? -1 : 0);
            _context.EventTemplates.UpdateRange(eventTemplates);
            _context.EventTemplates.Remove(eventTemplate);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventTemplateExists(int id)
        {
            return _context.EventTemplates.Any(e => e.Id == id);
        }
    }
}
