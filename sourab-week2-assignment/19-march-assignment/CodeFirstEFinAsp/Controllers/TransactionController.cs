using CodeFirstEFinAsp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CodeFirstEFinAsp.Controllers
{
    public class TransactionController : Controller
    {
        private readonly EventContext _context;

        public TransactionController(EventContext context)
        {
            _context = context;
        }

        public IActionResult CreateCustomer()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateCustomer(Customer customer)
        {
            ModelState.Clear();
            ModelState.Remove(nameof(Customer.CustomerID));
            if (ModelState.IsValid)
            {
                _context.customer.Add(customer);
                _context.SaveChanges();
                //return Content("Customer created successfully!");
                return RedirectToAction("CreateProduct", new {CustomerId= customer.CustomerID });
            }
            return View(customer);
        }
        public IActionResult CreateProduct(int? customerId = null)
        {
            var cid = customerId ?? 0;

            ViewBag.CustomerId = cid;
            ViewBag.CustomerList = new SelectList(
                _context.customer,
                "CustomerID",
                "CustomerName",
                cid
            );

            return View();
        }
    }
}

