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
using Microsoft.AspNetCore.Authorization;

namespace BestStudentCafedra.Controllers
{
    public class TeacherRequestsController : Controller
    {
        private readonly SubjectAreaDbContext _context;
        private readonly UserManager<User> _userManager;

        public TeacherRequestsController(SubjectAreaDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: TeacherRequests
        [Authorize("teacher")]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            var requests = _context.TeacherRequests
                .Include(t => t.GraduationWork)
                    .ThenInclude(x => x.Student.Group)
                .Include(t => t.Teacher)
                .Where(t => t.Teacher.Id == user.SubjectAreaId && t.Status == null);

            return View(await requests.OrderByDescending(x => x.CreatingDate).ToListAsync());
        }

        // GET: TeacherRequests/Approve
        [Authorize("teacher")]
        public async Task<IActionResult> Approve(int? id, string returnUrl)
        {
            if (id == null || !TeacherRequestExists((int)id))
                return NotFound();

            ViewData["returnUrl"] = returnUrl;

            return PartialView("_Approve", await _context.TeacherRequests.FindAsync(id));
        }

        // POST: TeacherRequests/Approve
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize("teacher")]
        public async Task<IActionResult> Approve(int id, [Bind("Id")]TeacherRequest teacherRequest, string returnUrl)
        {
            if (id != teacherRequest.Id || !TeacherRequestExists(id))
                return NotFound();

            var tR = await _context.TeacherRequests.Include(x => x.GraduationWork).Include(x => x.Teacher).FirstOrDefaultAsync(x => x.Id == id);
            tR.Approve(tR.Teacher);

            _context.Update(tR);
            await _context.SaveChangesAsync();

            return RedirectToUrl(returnUrl);
        }

        // GET: TeacherRequests/Reject
        [Authorize("teacher")]
        public async Task<IActionResult> Reject(int? id, string returnUrl)
        {
            if (id == null || !TeacherRequestExists((int)id))
                return NotFound();

            ViewData["returnUrl"] = returnUrl;

            return PartialView("_Reject", await _context.TeacherRequests.FindAsync(id));
        }

        // POST: TeacherRequests/Reject
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize("teacher")]
        public async Task<IActionResult> Reject(int id, [Bind("Id,RejectReason")] TeacherRequest teacherRequest, string returnUrl)
        {
            if (id != teacherRequest.Id || !TeacherRequestExists(id))
                return NotFound();

            var tR = await _context.TeacherRequests.Include(x => x.GraduationWork).Include(x => x.Teacher).FirstOrDefaultAsync(x => x.Id == id);
            tR.Reject(tR.Teacher, teacherRequest.RejectReason);
            
            _context.Update(tR);
            await _context.SaveChangesAsync();

            return RedirectToUrl(returnUrl);
        }

        // GET: TeacherRequests/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id, string returnUrl)
        {
            if (id == null || !TeacherRequestExists((int)id))
            {
                return NotFound();
            }

            ViewData["returnUrl"] = returnUrl;

            var teacherRequest = await _context.TeacherRequests
                .Include(t => t.GraduationWork)
                    .ThenInclude(t => t.Student)
                .Include(t => t.Teacher)
                .FirstOrDefaultAsync(m => m.Id == id);

            return PartialView("_Details", teacherRequest);
        }

        // GET: TeacherRequests/Create
        [Authorize("student")]
        public async Task<IActionResult> Create(RequestType requestType, string returnUrl)
        {
            TeacherRequest teacherRequest = new TeacherRequest();
            teacherRequest.RequestType = requestType;
            
            User user = await _userManager.FindByNameAsync(User.Identity.Name);
            var graduationWork = await _context.GraduationWorks.FirstOrDefaultAsync(x => x.StudentId == user.SubjectAreaId);
            teacherRequest.GraduationWorkId = graduationWork.Id;

            ViewData["TeacherId"] = new SelectList(_context.Teachers, "Id", "FullName");
            ViewData["returnUrl"] = returnUrl;
            return PartialView("_Create", teacherRequest);
        }

        // POST: TeacherRequests/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize("student")]
        public async Task<IActionResult> Create([Bind("GraduationWorkId,TeacherId,Motivation,RequestType")] TeacherRequest teacherRequest, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (!_context.TeacherRequests.Any(x => x.GraduationWorkId == teacherRequest.GraduationWorkId && x.Status == null && x.RequestType == teacherRequest.RequestType ))
                {
                    teacherRequest.CreatingDate = DateTime.Now;
                    _context.Add(teacherRequest);
                    await _context.SaveChangesAsync();
                }

                return RedirectToUrl(returnUrl);
            }
            ViewData["GraduationWorkId"] = new SelectList(_context.GraduationWorks, "Id", "Id", teacherRequest.GraduationWorkId);
            ViewData["TeacherId"] = new SelectList(_context.Teachers, "Id", "FullName", teacherRequest.TeacherId);
            return View(teacherRequest);
        }

        // GET: TeacherRequests/Delete/5
        [Authorize("student")]
        public async Task<IActionResult> Delete(int? id, string returnUrl)
        {
            if (id == null || !TeacherRequestExists((int)id))
            {
                return NotFound();
            }

            ViewData["returnUrl"] = returnUrl;

            var teacherRequest = await _context.TeacherRequests
                .Include(t => t.GraduationWork)
                .Include(t => t.Teacher)
                .FirstOrDefaultAsync(m => m.Id == id);

            return PartialView("_Delete", teacherRequest);
        }

        // POST: TeacherRequests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize("student")]
        public async Task<IActionResult> DeleteConfirmed(int id, string returnUrl)
        {
            var teacherRequest = await _context.TeacherRequests.Include(x => x.GraduationWork).FirstOrDefaultAsync(x => x.Id == id);
            _context.TeacherRequests.Remove(teacherRequest);
            await _context.SaveChangesAsync();

            return RedirectToUrl(returnUrl);
        }

        private bool TeacherRequestExists(int id)
        {
            return _context.TeacherRequests.Any(e => e.Id == id);
        }

        private IActionResult RedirectToUrl(string url)
        {
            if (!string.IsNullOrEmpty(url) && Url.IsLocalUrl(url))
                return Redirect(url);
            else
                return RedirectToAction(nameof(Index));
        }
    }
}
