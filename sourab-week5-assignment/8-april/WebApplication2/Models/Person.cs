namespace WebApplication2.Models
{
    public class Person
    {
        public int Id { get; set; }        // PK
        public string Name { get; set; } = null!;
        public int Age { get; set; }
        public string City { get; set; } = null!;
    }
}