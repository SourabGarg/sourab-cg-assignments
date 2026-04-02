using System.ComponentModel.DataAnnotations;

namespace WebAPIMVC.Models
{
    public class Employee
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter your firstname")]
        public string? FirstName { set; get; }

        [Required(ErrorMessage = "Please enter your lastname")]
        public string? LastName { set; get; }

        [Required(ErrorMessage = "Please enter email id")]
        [EmailAddress(ErrorMessage = "Please enter valid email id")]
        public string? Email { set; get; }

        [Required(ErrorMessage = "Please enter your age")]
        [Range(0, 100, ErrorMessage = "Please enter your age betwen 1 to 100 only ")]

        public int Age { set; get; }

        public string? ImagePath { set; get; }
    }
}
