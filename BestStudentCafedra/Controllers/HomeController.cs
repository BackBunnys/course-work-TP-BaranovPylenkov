using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BestStudentCafedra.Models;
using Microsoft.AspNetCore.Identity;
using BestStudentCafedra.Data;

namespace BestStudentCafedra.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly SubjectAreaDbContext _context;
        private readonly UserManager<User> _userManager;

        public HomeController(ILogger<HomeController> logger, SubjectAreaDbContext context, UserManager<User> userManager)
        {
            _logger = logger;

            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("student"))
            {
                User user = await _userManager.FindByNameAsync(User.Identity.Name);
                return RedirectToAction("Details","AcademicGroups", new { id = _context.Students.Find(user.SubjectAreaId).GroupId });
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = System.Diagnostics.Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
