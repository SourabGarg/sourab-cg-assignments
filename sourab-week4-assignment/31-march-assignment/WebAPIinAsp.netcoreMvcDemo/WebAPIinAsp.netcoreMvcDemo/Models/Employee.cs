using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;


namespace WebAPIinAsp.netcoreMvcDemo.Models
{
    public class Employee
    {
        public int Id { get; set; }

        [Required]
        public string? FirstName { get; set; }

        [Required]
        public string? LastName { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        [Range(1, 100)]
        public int Age { get; set; }

        public string? ImagePath { get; set; }
    }
}
