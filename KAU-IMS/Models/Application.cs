using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KAU_IMS.Models
{
    public class Application
    {
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int InternshipId { get; set; }

        public DateTime AppliedDate { get; set; } = DateTime.Now;

        public string Status { get; set; } = "Pending";

        public string? CoverLetter { get; set; }

        [ForeignKey("UserId")]
        public virtual User? User { get; set; }

        [ForeignKey("InternshipId")]
        public virtual Internship? Internship { get; set; }
    }
}
