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
    public class ProposedTopicController : Controller
    {
        private readonly SubjectAreaDbContext _context;

        public ProposedTopicController(SubjectAreaDbContext context)
        {
            _context = context;
        }

        // GET: ProposedTopic
        public async Task<IActionResult> Index()
        {
            return View(await _context.ProposedTopics.ToListAsync());
        }

        // GET: ProposedTopic/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var proposedTopic = await _context.ProposedTopics
                .FirstOrDefaultAsync(m => m.Id == id);
            if (proposedTopic == null)
            {
                return NotFound();
            }

            return View(proposedTopic);
        }

        // GET: ProposedTopic/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ProposedTopic/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] ProposedTopic proposedTopic)
        {
            if (ModelState.IsValid)
            {
                _context.Add(proposedTopic);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(proposedTopic);
        }

        // GET: ProposedTopic/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var proposedTopic = await _context.ProposedTopics.FindAsync(id);
            if (proposedTopic == null)
            {
                return NotFound();
            }
            return View(proposedTopic);
        }

        // POST: ProposedTopic/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] ProposedTopic proposedTopic)
        {
            if (id != proposedTopic.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(proposedTopic);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProposedTopicExists(proposedTopic.Id))
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
            return View(proposedTopic);
        }

        // GET: ProposedTopic/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var proposedTopic = await _context.ProposedTopics
                .FirstOrDefaultAsync(m => m.Id == id);
            if (proposedTopic == null)
            {
                return NotFound();
            }

            return View(proposedTopic);
        }

        // POST: ProposedTopic/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var proposedTopic = await _context.ProposedTopics.FindAsync(id);
            _context.ProposedTopics.Remove(proposedTopic);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProposedTopicExists(int id)
        {
            return _context.ProposedTopics.Any(e => e.Id == id);
        }
    }
}
