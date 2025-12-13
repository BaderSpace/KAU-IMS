using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KAU_IMS.Models
{
    public class Internship
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Company { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        public string Location { get; set; } = string.Empty;

        [Required]
        public string RequiredSkills { get; set; } = string.Empty;

        public DateTime PostedDate { get; set; } = DateTime.Now;

        public DateTime? Deadline { get; set; }

        public bool IsActive { get; set; } = true;

        public int? CompanyId { get; set; }

        [ForeignKey("CompanyId")]
        public virtual Company? CompanyOwner { get; set; }
    }
}
