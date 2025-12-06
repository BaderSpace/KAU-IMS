using System.ComponentModel.DataAnnotations;

namespace KAU_IMS.Models
{
    public class Student
    {
        [Key]
        public int stu_Id { get; set; }
        public string stu_name { get; set; }
        public string stu_email { get; set; }
        public string stu_phone { get; set; }
        public float stu_GDP { get; set; }
        public string stu_specialty { get; set; }
        public int stu_gender { get; set; }

        public Student()
        {
            
        }
    }
}
