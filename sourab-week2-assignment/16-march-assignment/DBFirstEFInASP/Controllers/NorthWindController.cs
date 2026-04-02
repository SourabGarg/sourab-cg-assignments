using DBFirstEFInASP.Data;
using DBFirstEFInASP.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace DBFirstEFInASP.Controllers
{
    public class NorthWindController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SpainCustomers()
        {
            using var cnt = new NorthwindContext();

            var spainCustomers = cnt.Customers
                .Where(x => x.Country == "Spain")
                .Select(x => new SpainCustomerViewModel
                {
                    Cid = x.CustomerId,
                    Cname = x.ContactName,
                    Conname = x.CompanyName
                })
                .ToList();

            return View(spainCustomers);
        }

        public IActionResult SearchCustomer(string? contactname)
        {
            using var cnt = new NorthwindContext();

            var customers = cnt.Customers
                .Where(x => string.IsNullOrWhiteSpace(contactname) || x.ContactName!.Contains(contactname))
                .Select(x => new SpainCustomerViewModel
                {
                    Cid = x.CustomerId,
                    Cname = x.ContactName,
                    Conname = x.CompanyName
                })
                .ToList();

            return View(customers); // looks for Views/NorthWind/SearchCustomer.cshtml
        }
    }
}
