using AzureFxnExample.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;

namespace AzureFxnExample.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;

        public HomeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(SalesRequest request, IFormFile? file)
        {
            request.Id = Guid.NewGuid().ToString();
            request.Status = file != null ? "ResumeUploaded" : "NoResume";

            var functionUrl = _configuration["FunctionApp:SalesUploadUrl"];

            if (!string.IsNullOrEmpty(functionUrl))
            {
                var client = new HttpClient();
                var response = await client.PostAsJsonAsync(functionUrl, request);

                if (response.IsSuccessStatusCode)
                {
                    TempData["Message"] = "Application submitted successfully!";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["Message"] = "Error submitting application.";
                    return View(request);
                }
            }

            return View(request);
        }
    }
}
