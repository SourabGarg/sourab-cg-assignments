namespace MVC_Demo_Project.Models
{
    public class CourseDetailVM
    {
        public string Title { get; set; } = string.Empty;
        public int Credits { get; set; }
        public string Department { get; set; } = string.Empty;
        public string LatestGrade { get; set; } = string.Empty;
    }
}
