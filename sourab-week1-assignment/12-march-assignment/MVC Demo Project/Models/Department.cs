namespace MVC_Demo_Project.Models
{
    public class Department
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string? Description { get; set; }

        // Master-side collection
        public List<Employee> Employees { get; set; } = new List<Employee>();
    }
}