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
    public class ProposedTopicController : Controller
    {
        private readonly SubjectAreaDbContext _context;

        public ProposedTopicController(SubjectAreaDbContext context)
        {
            _context = context;
        }

        // GET: ProposedTopic
        [Authorize]
        public async Task<IActionResult> Index(ProposedTopic topic)
        {
            ViewData["topics"] = await _context.ProposedTopics.ToListAsync();
            return View(topic == null? new ProposedTopic(): topic);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "methodist")]
        public async Task<IActionResult> Create([Bind("Id,Name")] ProposedTopic proposedTopic)
        {
            if (ModelState.IsValid)
            {
                _context.Add(proposedTopic);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ModelState.AddModelError("", "Тема должна быть длиной от 10 до 100 символов");
            ViewData["topics"] = await _context.ProposedTopics.ToListAsync();
            return View("Index", proposedTopic);
        }

        // POST: ProposedTopic/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "methodist")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var proposedTopic = await _context.ProposedTopics
                .FirstOrDefaultAsync(m => m.Id == id);
            if (proposedTopic == null)
            {
                return NotFound();
            }
            _context.ProposedTopics.Remove(proposedTopic);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
