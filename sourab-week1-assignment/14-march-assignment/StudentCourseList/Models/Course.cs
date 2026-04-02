namespace MVC_Demo_Project.Models
{
    public class Course
    {
        public int CourseId { get; set; }
        public string Title { get; set; } = string.Empty;
        public int Credits { get; set; }
        public string Department { get; set; } = string.Empty;
        public List<Enrollment> Enrollments { get; set; } = new();
    }
}
