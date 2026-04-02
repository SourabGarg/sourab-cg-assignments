namespace MVC_Demo_Project.Models
{
    public class StudentCoursesVM
    {
        public int StudentId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Branch { get; set; } = string.Empty;
        public List<string> CourseTitles { get; set; } = new();
    }
}
