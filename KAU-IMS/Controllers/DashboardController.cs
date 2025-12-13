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

        public IActionResult CompanyDashboard()
        {
            var companyIdString = HttpContext.Session.GetString("CompanyId");
            if (string.IsNullOrEmpty(companyIdString))
            {
                TempData["ErrorMessage"] = "Please login to access dashboard.";
                return RedirectToAction("Login", "Account");
            }

            int companyId = int.Parse(companyIdString);
            var company = _context.Companies.Find(companyId);

            if (company == null)
            {
                HttpContext.Session.Clear();
                return RedirectToAction("Login", "Account");
            }

            var internships = _context.Internships
                .Where(i => i.CompanyId == companyId)
                .OrderByDescending(i => i.PostedDate)
                .ToList();

            ViewBag.Company = company;
            ViewBag.InternshipCount = internships.Count;
            ViewBag.LastLogin = Request.Cookies["LastLogin"];

            return View(internships);
        }

        public IActionResult ViewApplications(int internshipId)
        {
            var companyIdString = HttpContext.Session.GetString("CompanyId");
            if (string.IsNullOrEmpty(companyIdString))
            {
                TempData["ErrorMessage"] = "Please login to access this page.";
                return RedirectToAction("Login", "Account");
            }

            int companyId = int.Parse(companyIdString);
            var internship = _context.Internships.Find(internshipId);

            if (internship == null || internship.CompanyId != companyId)
            {
                TempData["ErrorMessage"] = "Internship not found or access denied.";
                return RedirectToAction("CompanyDashboard");
            }

            var applications = _context.Applications
                .Include(a => a.User)
                .Include(a => a.Internship)
                .Where(a => a.InternshipId == internshipId)
                .OrderByDescending(a => a.AppliedDate)
                .ToList();

            ViewBag.Internship = internship;
            ViewBag.ApplicationCount = applications.Count;

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
