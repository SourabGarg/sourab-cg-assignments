using Microsoft.AspNetCore.Mvc;

namespace WebApilnAsp.Controllers
{
    public class AuthenticationUIController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); // Clear session/token
            return RedirectToAction("Login");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SaveSession(string username)
        {
            HttpContext.Session.SetString("username", username);
            return Ok();
        }
    }
}
