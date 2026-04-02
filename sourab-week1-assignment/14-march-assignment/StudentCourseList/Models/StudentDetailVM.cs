namespace MVC_Demo_Project.Models
{
    public class StudentDetailVM
    {
        public int StudentId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Branch { get; set; } = string.Empty;
        public List<CourseDetailVM> Courses { get; set; } = new();
    }
}
