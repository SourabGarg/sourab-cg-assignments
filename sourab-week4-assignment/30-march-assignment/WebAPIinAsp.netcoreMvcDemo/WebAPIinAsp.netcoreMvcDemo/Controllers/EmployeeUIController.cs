using Microsoft.AspNetCore.Mvc;

namespace WebAPIinAsp.netcoreMvcDemo.Controllers
{
    public class EmployeeUIController : Controller
    {
        public IActionResult Index()
        {

            return View();
        }

        public IActionResult Create()
        {

            return View();
        }

        public IActionResult Edit()
        {

            return View();
        }

        public IActionResult Details(int id)
        {
            return View();
        }

      

        public IActionResult Delete(int id)
        {
            return View();
        }

        public IActionResult Export()
        {
            return View();
        }

       

    }
}
