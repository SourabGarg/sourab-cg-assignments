namespace CodeFirstEFinAsp.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? CourseDescription { get; set; }
        public float? fullprice { get; set; }
        public Author? Author { get; set; }
        public List<Student> Students { get; set; }

    }
}
