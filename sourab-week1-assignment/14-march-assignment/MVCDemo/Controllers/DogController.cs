using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MVCDemo.Models;

namespace MVCDemo.Controllers
{
    public class DogController : Controller
    {
        private static readonly List<Dog> dogs = new List<Dog>();
        private readonly IWebHostEnvironment _webHostEnvironment;

        public DogController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        private async Task<string?> SaveImageAsync(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
                return null;

            try
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "dogs");
                Directory.CreateDirectory(uploadsFolder);

                string fileName = $"{Guid.NewGuid()}{Path.GetExtension(imageFile.FileName)}";
                string filePath = Path.Combine(uploadsFolder, fileName);

                await using var fileStream = new FileStream(filePath, FileMode.Create);
                await imageFile.CopyToAsync(fileStream);

                return Path.Combine("/uploads/dogs", fileName).Replace("\\", "/");
            }
            catch
            {
                return null;
            }
        }

        private void DeleteImageIfExists(string? relativeImagePath)
        {
            if (string.IsNullOrWhiteSpace(relativeImagePath))
                return;

            try
            {
                string imagePath = Path.Combine(
                    _webHostEnvironment.WebRootPath,
                    relativeImagePath.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString()));

                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }
            catch
            {
                // Keep operation non-blocking if file deletion fails.
            }
        }

        // GET: DogController
        public ActionResult Index(string? search)
        {
            var filteredDogs = string.IsNullOrWhiteSpace(search)
                ? dogs
                : dogs
                .Where(d => d.Name.Contains(search, StringComparison.OrdinalIgnoreCase))
                .ToList();

            return View(filteredDogs);
        }

        // GET: DogController/Details/5
        public ActionResult Details(int id)
        {
            var dog = dogs.FirstOrDefault(d => d.Id == id);
            if (dog == null)
            {
                return View("DogNotFound", id);
            }

            return View(dog);
        }

        // GET: DogController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DogController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Dog d)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(d);
                }

                if (dogs.Any(x => x.Id == d.Id))
                {
                    ModelState.AddModelError(nameof(Dog.Id), "A dog with this ID already exists.");
                    return View(d);
                }

                if (d.ImageFile != null && d.ImageFile.Length > 0)
                {
                    d.ImagePath = await SaveImageAsync(d.ImageFile);
                }

                dogs.Add(d);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(d);
            }
        }

        // GET: DogController/Edit/5
        public ActionResult Edit(int id)
        {
            var dog = dogs.FirstOrDefault(d => d.Id == id);
            if (dog == null)
            {
                return NotFound();
            }

            return View(dog);
        }

        // POST: DogController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Dog d)
        {
            try
            {
                var dog = dogs.FirstOrDefault(x => x.Id == id);
                if (dog == null)
                {
                    return NotFound();
                }

                if (!ModelState.IsValid)
                {
                    d.Id = id; // Ensure route id remains consistent in the view model
                    return View(d);
                }

                dog.Name = d.Name;
                dog.Age = d.Age;

                if (d.ImageFile != null && d.ImageFile.Length > 0)
                {
                    var oldImagePath = dog.ImagePath;
                    dog.ImagePath = await SaveImageAsync(d.ImageFile);
                    DeleteImageIfExists(oldImagePath);
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                d.Id = id;
                return View(d);
            }
        }

        // GET: DogController/Delete/5
        public ActionResult Delete(int id)
        {
            var dog = dogs.FirstOrDefault(d => d.Id == id);
            if (dog == null)
            {
                return NotFound();
            }

            return View(dog);
        }

        // POST: DogController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                var dog = dogs.FirstOrDefault(d => d.Id == id);
                if (dog != null)
                {
                    DeleteImageIfExists(dog.ImagePath);
                    dogs.Remove(dog);
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
