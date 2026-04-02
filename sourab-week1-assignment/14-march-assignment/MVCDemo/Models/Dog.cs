using System.ComponentModel.DataAnnotations;

namespace MVCDemo.Models
{
    public class Dog
    {
        [Required(ErrorMessage = "ID is required")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required"), MaxLength(222)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Age is required"), Range(0,20,ErrorMessage = "Age should be between 0 to 15")]
        public int Age { get; set; }

        [MaxLength(500)]
        public string? ImagePath { get; set; }

        public IFormFile? ImageFile { get; set; }
    }
}
