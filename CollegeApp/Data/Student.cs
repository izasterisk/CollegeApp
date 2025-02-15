using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CollegeApp.Data
{
    public class Student
    {
        //[Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //Sau khi thêm StudentConfig
        //Không cần phải sử dụng [Key] và [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string studentName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public DateTime DOB { get; set; }
        public int? DepartmentId { get; set; }
        public virtual Department? Department { get; set; }
    }
}
