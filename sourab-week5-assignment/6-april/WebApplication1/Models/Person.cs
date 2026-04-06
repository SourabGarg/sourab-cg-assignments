using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Person
    {
        public int Id { get; set; }   
        public string Name { get; set; } = null!;
        public int Age { get; set; }
        public string City { get; set; } = null!;
    }
}
