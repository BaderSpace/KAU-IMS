using Microsoft.AspNetCore.Mvc;
using KAU_IMS.Data;
using KAU_IMS.Models;
using Microsoft.EntityFrameworkCore;

namespace KAU_IMS.Controllers
{
    public class InternshipController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InternshipController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(string search)
        {
            var internships = _context.Internships.Where(i => i.IsActive).AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                internships = internships.Where(i => 
                    i.Title.Contains(search) || 
                    i.Company.Contains(search) || 
                    i.RequiredSkills.Contains(search) ||
                    i.Location.Contains(search));
                
                ViewBag.SearchTerm = search;
            }

            var internshipList = internships.OrderByDescending(i => i.PostedDate).ToList();
            return View(internshipList);
        }

        public IActionResult Details(int id)
        {
            var internship = _context.Internships.Find(id);
            if (internship == null)
            {
                TempData["ErrorMessage"] = "Internship not found.";
                return RedirectToAction("Index");
            }

            var userIdString = HttpContext.Session.GetString("UserId");
            if (!string.IsNullOrEmpty(userIdString))
            {
                int userId = int.Parse(userIdString);
                var existingApplication = _context.Applications
                    .FirstOrDefault(a => a.UserId == userId && a.InternshipId == id);
                ViewBag.AlreadyApplied = existingApplication != null;
            }

            return View(internship);
        }

        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("UserId") == null)
            {
                TempData["ErrorMessage"] = "Please login to post internships.";
                return RedirectToAction("Login", "Account");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Internship internship)
        {
            if (ModelState.IsValid)
            {
                internship.PostedDate = DateTime.Now;
                internship.IsActive = true;

                _context.Internships.Add(internship);
                _context.SaveChanges();

                TempData["SuccessMessage"] = "Internship posted successfully!";
                return RedirectToAction("Index");
            }

            return View(internship);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Apply(int internshipId, string coverLetter)
        {
            var userIdString = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdString))
            {
                TempData["ErrorMessage"] = "Please login to apply.";
                return RedirectToAction("Login", "Account");
            }

            int userId = int.Parse(userIdString);

            var existingApplication = _context.Applications
                .FirstOrDefault(a => a.UserId == userId && a.InternshipId == internshipId);

            if (existingApplication != null)
            {
                TempData["ErrorMessage"] = "You have already applied to this internship.";
                return RedirectToAction("Details", new { id = internshipId });
            }

            var application = new Application
            {
                UserId = userId,
                InternshipId = internshipId,
                AppliedDate = DateTime.Now,
                Status = "Pending",
                CoverLetter = coverLetter
            };

            _context.Applications.Add(application);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Application submitted successfully!";
            return RedirectToAction("Index", "Dashboard");
        }
    }
}
