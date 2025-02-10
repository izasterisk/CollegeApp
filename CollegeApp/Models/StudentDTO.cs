using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace CollegeApp.Models
{
    public class StudentDTO
    {
        [ValidateNever]
        public int Id { get; set; }
        [Required(ErrorMessage = "Student name ngu vai ca lon")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Student name ngan vai ca lon")]
        public string studentName { get; set; }
        [EmailAddress(ErrorMessage = "Day la cai email ngu nhat tao tung thay")]
        public string Email { get; set; }
        [Required]
        public string Address { get; set; }
        public String DOB { get; set; }
        //[Range(18, 60, ErrorMessage = "Tuoi phai tu 18 den 60")]
        //public int Age { get; set; }
        //public string Password { get; set; }
        //[Compare(nameof(Password), ErrorMessage = "Password khong trung khop")]
        //public string ConfirmPassword { get; set; }
        //public DateTime AdmissionDate { get; set; }
    }
}
