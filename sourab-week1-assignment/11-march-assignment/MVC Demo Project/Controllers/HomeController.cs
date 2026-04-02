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
        Employee obj = new Employee()
        {
            EmployeeId = 101,
            EmpName = "Anand",
            Salary = 45000
        };
        List<Employee> employeesList = new List<Employee>()
        {
            new Employee(){EmployeeId=101,EmpName="Anand",Salary=45000, ImageUrl= "/images/Library-HackerRank-03-02-2026_12_39_PM.png"},
            new Employee(){EmployeeId=102,EmpName="Sita",Salary=55000, ImageUrl= "/images/Programming-problems-and-Competitions-HackerRank-03-02-2026_12_40_PM.png"},
            new Employee(){EmployeeId=103,EmpName="Gita",Salary=65000, ImageUrl= "/images/Programming-problems-and-Competitions-HackerRank-03-02-2026_12_40_PM.png"},
            new Employee(){EmployeeId=104,EmpName="Rita",Salary=75000, ImageUrl= "/images/Programming-problems-and-Competitions-HackerRank-03-02-2026_12_40_PM.png"},
        };

        public IActionResult listObjectPassing()
        {
            return View(employeesList);
        }
        public IActionResult display()
        {
            return View();
        }
        public IActionResult singleObjectPassing()
        {
            return View(obj);
        }

        public IActionResult Index()
        {
            return View();
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
