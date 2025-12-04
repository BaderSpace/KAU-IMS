using System.ComponentModel.DataAnnotations;

namespace KAU_IMS.Models
{
    public class Company
    {
        [Key]
        public int company_Id { get; set; }
        public string company_name { get; set; }
        public string company_description { get; set; }
        public string company_phone { get; set; }
        public string company_email { get; set; }
        public string company_url { get; set; }
        public string company_internships { get; set; }

        public Company()
        {
            
        }
    }
}
