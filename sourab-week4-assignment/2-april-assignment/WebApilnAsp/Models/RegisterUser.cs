using System.ComponentModel.DataAnnotations;

namespace WebApilnAsp.Models
{
    public class RegisterUser
    {
        [Required(ErrorMessage = "User Name is required")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Format")]
        
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "At least one role is required")]
        [MinLength(1, ErrorMessage = "At least one role is required")]
        public List<string> Roles { get; set; } = new();
    }
}
