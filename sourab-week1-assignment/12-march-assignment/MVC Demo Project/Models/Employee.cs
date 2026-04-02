namespace MVC_Demo_Project.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }
        public string EmpName { get; set; }
        public int Salary { get; set; }
        public string? ImageUrl { get; set; }
        public string? Description { get; set; }

        // Foreign Key
        public int DepartmentId { get; set; }

        // Navigation Property
        public Department? Department { get; set; }
    }
}
