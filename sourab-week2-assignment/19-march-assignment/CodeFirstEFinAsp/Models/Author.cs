namespace CodeFirstEFinAsp.Models
{
    public class Author
    {
        public int Id { get; set; } // it will create identity column
        public string? AuthorName { set; get; }
        public IList<Course> Courses { get; set; }
    }
}
