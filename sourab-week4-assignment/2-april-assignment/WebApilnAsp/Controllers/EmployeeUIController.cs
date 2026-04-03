using Microsoft.AspNetCore.Mvc;

namespace WebApilnAsp.Controllers
{
    public class EmployeeUIController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Details(int id)
        {
            return View(model: id);
        }

        public IActionResult Edit(int id)
        {
            return View(model: id);
        }

        public IActionResult Delete(int id)
        {
            return View(model: id);
        }

        public IActionResult Export()
        {
            return View();
        }
    }
}
