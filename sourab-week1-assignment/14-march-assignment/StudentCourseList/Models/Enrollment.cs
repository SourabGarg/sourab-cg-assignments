namespace MVC_Demo_Project.Models
{
    public class Enrollment
    {
        public int EnrollmentId { get; set; }
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public string Grade { get; set; } = string.Empty;
        public int AttemptNumber { get; set; }
        public Student? Student { get; set; }
        public Course? Course { get; set; }
    }
}
