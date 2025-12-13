using Microsoft.EntityFrameworkCore;
using KAU_IMS.Models;

namespace KAU_IMS.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Internship> Internships { get; set; }
        public DbSet<Application> Applications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Internship>().HasData(
                new Internship
                {
                    Id = 1,
                    Title = "Software Development Intern",
                    Company = "Tech Solutions Inc.",
                    Description = "Work on web applications using ASP.NET and C#",
                    Location = "Jeddah, Saudi Arabia",
                    RequiredSkills = "C#, ASP.NET, SQL, JavaScript",
                    PostedDate = DateTime.Now,
                    Deadline = DateTime.Now.AddMonths(1),
                    IsActive = true
                },
                new Internship
                {
                    Id = 2,
                    Title = "Data Analyst Intern",
                    Company = "Analytics Pro",
                    Description = "Analyze data and create reports using Python and SQL",
                    Location = "Riyadh, Saudi Arabia",
                    RequiredSkills = "Python, SQL, Excel, Data Visualization",
                    PostedDate = DateTime.Now,
                    Deadline = DateTime.Now.AddMonths(2),
                    IsActive = true
                },
                new Internship
                {
                    Id = 3,
                    Title = "UI/UX Design Intern",
                    Company = "Creative Designs Co.",
                    Description = "Design user interfaces and improve user experience",
                    Location = "Remote",
                    RequiredSkills = "Figma, Adobe XD, HTML, CSS",
                    PostedDate = DateTime.Now,
                    Deadline = DateTime.Now.AddMonths(1),
                    IsActive = true
                }
            );
        }
    }
}
