using Microsoft.AspNetCore.Mvc;
using MVCDemo.Models;
using System.Diagnostics;

namespace MVCDemo.Controllers
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
            return "Sye";
        }
        public string sampleDemo2(string name, int age)
        {
            return $"The name {name} and age: {age}";
        }
        public IActionResult sampleDemo3()
        {
            ViewBag.Name = "Sye";
            ViewBag.Age = 34;
            ViewData["Message"] = "Welcome to Asp.Net Core Learning";
            ViewData["Year"] = DateTime.Now.Year;
            return View();
        }
        employee obj = new employee()
        {
            empId = 101,
            empName = "Sye",
            salary = 2323
        };
        List<employee> empList = new List<employee>()
        {
            new employee{ empId = 101, empName = "John Smith", salary = 300434, imageurl = "/images/emp1.jpg", deptId = 1001},
            new employee{ empId = 102, empName = "Sarah Johnson", salary = 343234, imageurl = "/images/emp2.jpg", deptId = 1001},
            new employee{ empId = 103, empName = "Michael Brown", salary = 323434, imageurl = "/images/emp3.jpg", deptId = 1002},
            new employee{ empId = 104, empName = "Emily Davis", salary = 342234, imageurl = "/images/emp4.jpg", deptId = 1002},
            new employee{ empId = 105, empName = "Robert Wilson", salary = 320000, imageurl = "/images/emp5.jpg", deptId = 1003},
            new employee{ empId = 106, empName = "Jessica Martinez", salary = 335000, imageurl = "/images/emp6.jpg", deptId = 1003},
            new employee{ empId = 107, empName = "David Anderson", salary = 310000, imageurl = "/images/emp7.jpg", deptId = 1004},
            new employee{ empId = 108, empName = "Lisa Taylor", salary = 330000, imageurl = "/images/emp8.jpg", deptId = 1004}
        };
        List<Dept> deptList = new List<Dept>()
        {
            new Dept{ DeptId = 1001, DeptName = "Software Development" },
            new Dept{ DeptId = 1002, DeptName = "Sales and Marketing"},
            new Dept{ DeptId = 1003, DeptName = "Human Resources"},
            new Dept{ DeptId = 1004, DeptName = "Finance and Accounting"}
        };

        public IActionResult ViewDepartments()
        {
            return View(deptList);
        }

        public IActionResult ViewEmployeesByDepartment(int deptid)
        {
            var dept = deptList.FirstOrDefault(d => d.DeptId == deptid);
            if (dept == null)
            {
                return NotFound();
            }
            var employees = empList.Where(e => e.deptId == deptid).ToList();
            ViewBag.DepartmentName = dept.DeptName;
            ViewBag.DepartmentId = deptid;
            return View(employees);
        }

        public IActionResult mxedobjectcollection(int empid)
        {
            var query1 = deptList.ToList();
            employee emp = empList.Where(e => e.empId == empid).FirstOrDefault();
            var query2 = emp;
            EmpViewModel obj = new EmpViewModel()
            {
                deptList = query1,
                emp = query2,
                date = DateTime.Now
            };
            return View(obj);
        }
        public IActionResult Details(int id) 
        { 
            var emp = empList.Where(e => e.empId == id).FirstOrDefault();
            if(emp == null)
            {
                return NotFound();
            }
            return View(emp);
        }
        public IActionResult searchemp(int empid)
        {
            var emp = empList.Where(e => e.empId == empid).FirstOrDefault();
            if (emp == null)
            {
                return NotFound();
            }
            return View(emp);
        }
        public IActionResult collectionofobjectpassing()
        {
            return View(empList);
        }

        public IActionResult display()
        {
            return View();
        }

        public IActionResult singleobjectpassing()
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
