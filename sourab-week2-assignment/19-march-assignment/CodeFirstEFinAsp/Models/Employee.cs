using System.ComponentModel.DataAnnotations;

namespace CodeFirstEFinAsp.Models
{
    public class Employee
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please Enter your First Name")]
        public string FirstName { get; set; }
        
        [Required(ErrorMessage = "Please Enter your Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please Enter Email")]
        [EmailAddress(ErrorMessage = "Please Enter a valid Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "enter your age ")]
        [Range(0, 100, ErrorMessage = "please enter age between 1 to 100 only ")]
        public int Age { set; get; }



    }
}
