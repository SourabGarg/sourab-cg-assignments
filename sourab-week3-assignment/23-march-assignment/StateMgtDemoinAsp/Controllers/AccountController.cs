using Microsoft.AspNetCore.Mvc;
using StateMgtDemoinAsp.Models;

namespace StateMgtDemoinAsp.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                //var cookieOptions = new CookieOptions
                //{
                //    Expires = DateTime.Now.AddMinutes(1)// set cookie
                //};
                //Response.Cookies.Append("UserName", model.Username, cookieOptions);
                HttpContext.Session.SetString("UserName", model.Username);
                return RedirectToAction("Welcome");
            }
            return View(model);
        }

        public IActionResult Welcome()
        {
            var username = HttpContext.Session.GetString("UserName");
            //if(Request.Cookies.ContainsKey("UserName"))
            //{
            //    string username = Request.Cookies["UserName"];
            //    ViewBag.UserName = username;
            //}
            if (!String.IsNullOrEmpty(username))
            {
                ViewBag.UserName = username;
            }
            else
            {
                return RedirectToAction("Login");
            }
            return View();
        }

        [HttpPost]
        public IActionResult Logout()
        {
            //  Response.Cookies.Delete("UserName");
            HttpContext.Session.Remove("UserName");
            return RedirectToAction("Login");
        }

    }
}
