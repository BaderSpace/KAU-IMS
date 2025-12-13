using System.ComponentModel.DataAnnotations;

namespace KAU_IMS.Models
{
    public class Company
    {
        public int Id { get; set; }

        [Required]
        public string CompanyName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        public string? Industry { get; set; }

        public string? Location { get; set; }

        public string? Website { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? LastLoginAt { get; set; }
    }
}
