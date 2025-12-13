using Microsoft.AspNetCore.Mvc;
using KAU_IMS.Data;
using KAU_IMS.Models;
using System.Security.Cryptography;
using System.Text;

namespace KAU_IMS.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string email, string password, string userType = "student")
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                TempData["ErrorMessage"] = "Email and password are required.";
                return View();
            }

            string hashedPassword = HashPassword(password);

            if (userType == "company")
            {
                var company = _context.Companies.FirstOrDefault(c => c.Email == email && c.Password == hashedPassword);

                if (company == null)
                {
                    TempData["ErrorMessage"] = "Invalid email or password.";
                    return View();
                }

                HttpContext.Session.SetString("CompanyId", company.Id.ToString());
                HttpContext.Session.SetString("CompanyEmail", company.Email);
                HttpContext.Session.SetString("CompanyName", company.CompanyName);
                HttpContext.Session.SetString("UserType", "company");

                CookieOptions option = new CookieOptions
                {
                    Expires = DateTime.Now.AddDays(30),
                    HttpOnly = true,
                    Secure = true
                };
                Response.Cookies.Append("CompanyEmail", company.Email, option);
                Response.Cookies.Append("LastLogin", DateTime.Now.ToString(), option);

                company.LastLoginAt = DateTime.Now;
                _context.SaveChanges();

                TempData["SuccessMessage"] = $"Welcome back, {company.CompanyName}!";
                return RedirectToAction("CompanyDashboard", "Dashboard");
            }
            else
            {
                var user = _context.Users.FirstOrDefault(u => u.Email == email && u.Password == hashedPassword);

                if (user == null)
                {
                    TempData["ErrorMessage"] = "Invalid email or password.";
                    return View();
                }

                HttpContext.Session.SetString("UserId", user.Id.ToString());
                HttpContext.Session.SetString("UserEmail", user.Email);
                HttpContext.Session.SetString("UserName", $"{user.FirstName} {user.LastName}");
                HttpContext.Session.SetString("UserType", "student");

                CookieOptions option = new CookieOptions
                {
                    Expires = DateTime.Now.AddDays(30),
                    HttpOnly = true,
                    Secure = true
                };
                Response.Cookies.Append("UserEmail", user.Email, option);
                Response.Cookies.Append("LastLogin", DateTime.Now.ToString(), option);

                user.LastLoginAt = DateTime.Now;
                _context.SaveChanges();

                TempData["SuccessMessage"] = $"Welcome back, {user.FirstName}!";
                return RedirectToAction("Index", "Dashboard");
            }
        }

        public IActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Signup(User user)
        {
            if (string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Password))
            {
                TempData["ErrorMessage"] = "All fields are required.";
                return View();
            }

            var existingUser = _context.Users.FirstOrDefault(u => u.Email == user.Email);
            if (existingUser != null)
            {
                TempData["ErrorMessage"] = "Email already registered. Please login.";
                return View();
            }

            user.Password = HashPassword(user.Password);
            user.CreatedAt = DateTime.Now;

            _context.Users.Add(user);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Account created successfully! Please login.";
            return RedirectToAction("Login");
        }

        public IActionResult CompanySignup()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CompanySignup(Company company)
        {
            if (string.IsNullOrEmpty(company.Email) || string.IsNullOrEmpty(company.Password))
            {
                TempData["ErrorMessage"] = "All fields are required.";
                return View();
            }

            var existingCompany = _context.Companies.FirstOrDefault(c => c.Email == company.Email);
            if (existingCompany != null)
            {
                TempData["ErrorMessage"] = "Email already registered. Please login.";
                return View();
            }

            company.Password = HashPassword(company.Password);
            company.CreatedAt = DateTime.Now;

            _context.Companies.Add(company);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Company account created successfully! Please login.";
            return RedirectToAction("Login");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            Response.Cookies.Delete("UserEmail");
            Response.Cookies.Delete("CompanyEmail");
            Response.Cookies.Delete("LastLogin");

            TempData["InfoMessage"] = "You have been logged out successfully.";
            return RedirectToAction("Index", "Home");
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
