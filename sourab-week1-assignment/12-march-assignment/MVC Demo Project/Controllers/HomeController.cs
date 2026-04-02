using Microsoft.AspNetCore.Mvc;
using MVC_Demo_Project.Models;
using System.Diagnostics;

namespace MVC_Demo_Project.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public string sampleDemo1()
        {
            return "This is a sample demo method";
        }

        public string sampleDemo2()
        {
            return "This is a sample demo method 2";
        }

        public IActionResult sampleDemo3()
        {
            int age = 27;
            string name = "Anand";
            ViewBag.Age = age;
            ViewBag.Name = name;
            ViewData["Message"] = "This is a sample demo method 3";
            ViewData["Year"] = DateTime.Now.Year;
            return View();
        }

        // Initialize employee list with department assignments
        List<Employee> employeesList = new List<Employee>()
        {
            new Employee(){
                EmployeeId = 101, EmpName = "Anand", Salary = 45000, DepartmentId = 1,
                ImageUrl = "https://ui-avatars.com/api/?name=Anand&background=4a90d9&color=fff&size=200&rounded=true&bold=true",
                Description = "Anand is a Senior Software Engineer with 5+ years of experience in .NET and cloud technologies. He leads the backend development team."
            },
            new Employee(){
                EmployeeId = 102, EmpName = "Sita", Salary = 55000, DepartmentId = 1,
                ImageUrl = "https://ui-avatars.com/api/?name=Sita&background=e07b54&color=fff&size=200&rounded=true&bold=true",
                Description = "Sita is a Full Stack Developer specializing in React and ASP.NET Core. She has delivered multiple client-facing web applications."
            },
            new Employee(){
                EmployeeId = 103, EmpName = "Gita", Salary = 65000, DepartmentId = 2,
                ImageUrl = "https://ui-avatars.com/api/?name=Gita&background=4caf7d&color=fff&size=200&rounded=true&bold=true",
                Description = "Gita is a Project Manager with expertise in Agile methodologies. She oversees project timelines and coordinates cross-functional teams."
            },
            new Employee(){
                EmployeeId = 104, EmpName = "Rita", Salary = 75000, DepartmentId = 3,
                ImageUrl = "https://ui-avatars.com/api/?name=Rita&background=9b59b6&color=fff&size=200&rounded=true&bold=true",
                Description = "Rita is a Data Analyst with strong skills in SQL and Power BI. She drives data-informed decisions across the organization."
            },
        };

        List<Department> departmentsList = new List<Department>()
        {
            new Department(){
                DepartmentId = 1, DepartmentName = "IT", 
                Description = "Information Technology Department - Develops and maintains all technical infrastructure and applications.",
                Employees = new List<Employee>()
            },
            new Department(){
                DepartmentId = 2, DepartmentName = "HR", 
                Description = "Human Resources Department - Manages recruitment, payroll, and employee development programs.",
                Employees = new List<Employee>()
            },
            new Department(){
                DepartmentId = 3, DepartmentName = "Finance", 
                Description = "Finance and Accounts Department - Oversees budgeting, financial planning, and reporting.",
                Employees = new List<Employee>()
            },
            new Department(){
                DepartmentId = 4, DepartmentName = "Operations", 
                Description = "Operations Management Department - Ensures smooth daily operations and resource optimization.",
                Employees = new List<Employee>()
            },
        };

        // Helper method to populate department-employee relationships
        private void PopulateDepartmentEmployees()
        {
            // Clear existing employees
            foreach (var dept in departmentsList)
            {
                dept.Employees.Clear();
            }

            // Assign employees to departments
            foreach (var emp in employeesList)
            {
                var department = departmentsList.FirstOrDefault(d => d.DepartmentId == emp.DepartmentId);
                if (department != null)
                {
                    department.Employees.Add(emp);
                }
            }
        }

        // EMPLOYEE ACTIONS
        public IActionResult ListEmployees()
        {
            return View(employeesList);
        }

        public IActionResult EmployeeDetail(int id)
        {
            var employee = employeesList.FirstOrDefault(e => e.EmployeeId == id);
            if (employee == null) return NotFound();
            
            // Populate department info
            employee.Department = departmentsList.FirstOrDefault(d => d.DepartmentId == employee.DepartmentId);
            return View(employee);
        }

        public IActionResult SearchEmployee(int empId)
        {
            Employee emp = (from e in employeesList
                            where e.EmployeeId == empId
                            select e).FirstOrDefault();

            if (emp == null)
            {
                ViewBag.Message = $"No employee found with ID {empId}.";
                return View(new Employee());
            }

            emp.Department = departmentsList.FirstOrDefault(d => d.DepartmentId == emp.DepartmentId);
            return View(emp);
        }

        // DEPARTMENT ACTIONS
        public IActionResult ListDepartments()
        {
            PopulateDepartmentEmployees();
            return View(departmentsList);
        }

        public IActionResult DepartmentDetail(int id)
        {
            PopulateDepartmentEmployees();
            var department = departmentsList.FirstOrDefault(d => d.DepartmentId == id);
            if (department == null) return NotFound();
            return View(department);
        }

        public IActionResult DepartmentEmployees(int id)
        {
            PopulateDepartmentEmployees();
            var department = departmentsList.FirstOrDefault(d => d.DepartmentId == id);
            if (department == null) return NotFound();
            return View(department);
        }

        public IActionResult SearchDepartment(int deptId)
        {
            PopulateDepartmentEmployees();
            Department dept = (from d in departmentsList
                               where d.DepartmentId == deptId
                               select d).FirstOrDefault();

            if (dept == null)
            {
                ViewBag.Message = $"No department found with ID {deptId}.";
                return View(new Department());
            }

            return View(dept);
        }

        // GENERAL PAGES
        public IActionResult Display()
        {
            return View();
        }

        public IActionResult Index()
        {
            PopulateDepartmentEmployees();
            return View(departmentsList);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}