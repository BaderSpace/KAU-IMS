using Microsoft.AspNetCore.Mvc;
using KAU_IMS.Data;
using KAU_IMS.Models;
using Microsoft.EntityFrameworkCore;

namespace KAU_IMS.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var userIdString = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdString))
            {
                TempData["ErrorMessage"] = "Please login to access dashboard.";
                return RedirectToAction("Login", "Account");
            }

            int userId = int.Parse(userIdString);
            var user = _context.Users.Find(userId);

            if (user == null)
            {
                HttpContext.Session.Clear();
                return RedirectToAction("Login", "Account");
            }

            var applications = _context.Applications
                .Include(a => a.Internship)
                .Where(a => a.UserId == userId)
                .OrderByDescending(a => a.AppliedDate)
                .ToList();

            ViewBag.User = user;
            ViewBag.ApplicationCount = applications.Count;
            ViewBag.LastLogin = Request.Cookies["LastLogin"];

            return View(applications);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteApplication(int id)
        {
            var application = _context.Applications.Find(id);
            if (application != null)
            {
                _context.Applications.Remove(application);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Application deleted successfully.";
            }

            return RedirectToAction("Index");
        }
    }
}
