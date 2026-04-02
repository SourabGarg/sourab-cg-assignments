using DBFirstEFInASP.Data;
using DBFirstEFInASP.Models;
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
        public ActionResult ProductsInCategory(string categoryname)
        {
            using var cnt = new NorthwindContext();
            
            var productsInCategory = cnt.Products
                .Where(x => x.Category!.CategoryName == categoryname)
                .Select(x => new ProdCat
                {
                    prodname = x.ProductName,
                    catname = x.Category.CategoryName
                })
                .ToList();
            
            return View(productsInCategory);  // Pass the list to view
        }

        public ActionResult OrderRange(string range)
        {
            NorthwindContext cnt = new NorthwindContext();
            var orderRange = cnt.Orders
                            .Where(x => x.OrderDetails.Sum(y => y.Quantity) > int.Parse(range))
                            .Select(x => new OrderQ
                            {
                                Id = x.OrderId,
                                OrderDate = x.OrderDate,
                                TotalQuantity = x.OrderDetails.Sum(y => y.Quantity)
                            }).ToList();
            return View(orderRange);
        }
    }
}
