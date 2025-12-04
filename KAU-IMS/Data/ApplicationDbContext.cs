using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using KAU_IMS.Models;

namespace KAU_IMS.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<KAU_IMS.Models.InternShip> InternShip { get; set; } = default!;
        public DbSet<KAU_IMS.Models.Company> Company { get; set; } = default!;
        public DbSet<KAU_IMS.Models.Student> Student { get; set; } = default!;
    }
}
