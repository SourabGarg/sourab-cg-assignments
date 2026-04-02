using System.ComponentModel.DataAnnotations;

namespace DogApp.Models
{
    public class Dog
    {
        [Required(ErrorMessage = "ID is required")]
        public int ID { get; set; }

        [Required(ErrorMessage = "Name is required"), MaxLength(222)]

        public string? Name { get; set; }

        [Required(ErrorMessage = "Age is required"), Range(0, 20, ErrorMessage = "Age should be between 0 to 20 only")]

        public int Age { get; set; }

        //[Required(ErrorMessage = "Description is required"), MaxLength(500)]

        public string? Description { get; set; }

        public string? ImagePath { get; set; } // Path to saved image

    }
}
