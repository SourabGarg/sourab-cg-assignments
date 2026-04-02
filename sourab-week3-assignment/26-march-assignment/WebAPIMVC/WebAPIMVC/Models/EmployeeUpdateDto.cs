using System.ComponentModel.DataAnnotations;

namespace WebAPIMVC.Models
{
    public class EmployeeUpdateDto
    {
        [Required(ErrorMessage = "Please enter your firstname")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "Please enter your lastname")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Please enter email id")]
        [EmailAddress(ErrorMessage = "Please enter valid email id")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Please enter your age")]
        [Range(0, 100, ErrorMessage = "Please enter your age betwen 1 to 100 only ")]
        public int Age { get; set; }

        public string? ImagePath { get; set; }
    }
}
