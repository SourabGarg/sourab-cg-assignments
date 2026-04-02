using CodeFirstEFinAsp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace CodeFirstEFinAsp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly EventContext _eventContext;

        public HomeController(ILogger<HomeController> logger, EventContext eventContext)
        {
            _logger = logger;
            _eventContext = eventContext;
        }
        public IActionResult displayEmp()
        {
           var emp = _eventContext.employees.ToList();
            return View(emp);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Employee employee)
        {
            if(ModelState.IsValid)
            {
                _eventContext.employees.Add(employee);
                _eventContext.SaveChanges();
                return RedirectToAction("displayEmp");
            }
            return View(employee);
        }

        public IActionResult Details(int id)
        {
            var employee = _eventContext.employees.FirstOrDefault(e => e.Id == id);

            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        public IActionResult Edit(int id)
        {
            var employee = _eventContext.employees.Find(id);

            if (employee == null)
            {
                return BadRequest();
            }

            return View(employee);
        }
        [HttpPost]
        public IActionResult Edit(int id, Employee employee)
        {
            if (id != employee.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                _eventContext.Update(employee);
                _eventContext.SaveChanges();

                return RedirectToAction("displayEmp");
            }

            return View(employee);
        }

        public IActionResult Delete(int id)
        {
            var employee = _eventContext.employees.Find(id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }
        // POST: Employee/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var employee = _eventContext.employees.Find(id);
            if (employee != null)
            {
                _eventContext.employees.Remove(employee);
                _eventContext.SaveChanges();
            }
            return RedirectToAction("displayEmp");
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
