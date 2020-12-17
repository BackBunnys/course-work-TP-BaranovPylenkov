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
        public async Task<IActionResult> Index()
        {
            return View(await _context.EventTemplates.ToListAsync());
        }

        // POST: EventTemplate/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SequentialNumber,Description")] EventTemplate eventTemplate)
        {
            if (ModelState.IsValid)
            {
                _context.Add(eventTemplate);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(eventTemplate);
        }

        // POST: EventTemplate/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SequentialNumber,Description")] EventTemplate eventTemplate)
        {
            if (id != eventTemplate.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(eventTemplate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventTemplateExists(eventTemplate.Id))
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
            return View(eventTemplate);
        }

        // POST: EventTemplate/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var eventTemplate = await _context.EventTemplates.FindAsync(id);
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
