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
        public async Task<IActionResult> Approve(int id, [Bind("Id")]TeacherRequest teacherRequest, string returnUrl)
        {
            if (id != teacherRequest.Id || !TeacherRequestExists(id))
                return NotFound();

            var tR = await _context.TeacherRequests.Include(x => x.GraduationWork).Include(x => x.Teacher).FirstOrDefaultAsync(x => x.Id == id);
            tR.ResponseDate = DateTime.Now;
            tR.ResponsePersonName = tR.Teacher.FullName;
            tR.Status = Status.APPROVED;
            if (tR.RequestType == RequestType.ADVISER)
                tR.GraduationWork.ScientificAdviserId = tR.TeacherId;
            else if(tR.RequestType == RequestType.REVIEWER)
                tR.GraduationWork.ReviewerId = tR.TeacherId;
            _context.Update(tR);

            await _context.SaveChangesAsync();

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            else
                return RedirectToAction(nameof(Index));
        }

        // GET: TeacherRequests/Reject
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
        public async Task<IActionResult> Reject(int id, [Bind("Id,RejectReason")] TeacherRequest teacherRequest, string returnUrl)
        {
            if (id != teacherRequest.Id || !TeacherRequestExists(id))
                return NotFound();

            var tR = await _context.TeacherRequests.Include(x => x.GraduationWork).Include(x => x.Teacher).FirstOrDefaultAsync(x => x.Id == id);
            tR.ResponseDate = DateTime.Now;
            tR.ResponsePersonName = tR.Teacher.FullName;
            tR.Status = Status.REJECTED;
            tR.RejectReason = teacherRequest.RejectReason;
            
            _context.Update(tR);

            await _context.SaveChangesAsync();

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            else
                return RedirectToAction(nameof(Index));
        }

        // GET: TeacherRequests/Details/5
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

                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    return Redirect(returnUrl);
                else
                    return RedirectToAction(nameof(Index));
            }
            ViewData["GraduationWorkId"] = new SelectList(_context.GraduationWorks, "Id", "Id", teacherRequest.GraduationWorkId);
            ViewData["TeacherId"] = new SelectList(_context.Teachers, "Id", "FullName", teacherRequest.TeacherId);
            return View(teacherRequest);
        }

        // GET: TeacherRequests/Delete/5
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
        public async Task<IActionResult> DeleteConfirmed(int id, string returnUrl)
        {
            var teacherRequest = await _context.TeacherRequests.Include(x => x.GraduationWork).FirstOrDefaultAsync(x => x.Id == id);
            _context.TeacherRequests.Remove(teacherRequest);
            await _context.SaveChangesAsync();

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            else
                return RedirectToAction(nameof(Index));
        }

        private bool TeacherRequestExists(int id)
        {
            return _context.TeacherRequests.Any(e => e.Id == id);
        }
    }
}
