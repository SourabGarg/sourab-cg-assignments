using Microsoft.EntityFrameworkCore;
using WebApiDemo.Models;

namespace WebApiDemo
{
    public class EmployeeService : IEmployee
    {
        private readonly EmpContext _context;
        private readonly IWebHostEnvironment _env;
        public EmployeeService(EmpContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        // FileStream fs;
        public async Task<Employee> AddEmployeeAsync(Employee employee, IFormFile image)
        {
            if (image != null && image.Length > 0)
            {
                var imageName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                var imagePath = Path.Combine(_env.WebRootPath, "uploads", imageName);
                Directory.CreateDirectory(Path.GetDirectoryName(imagePath)!);
                using var stream = new FileStream(imagePath, FileMode.Create);
                await image.CopyToAsync(stream);
                employee.ImagePath = "/uploads/" + imageName;


            }
            await _context.employees.AddAsync(employee);
            await _context.SaveChangesAsync();
            return employee;
        }

        public async Task<Employee?> DeleteEmployeeAsync(int id)
        {
            var employee = await _context.employees.FindAsync(id);
            if (employee == null) return null;
            DeleteImageFile(employee.ImagePath);
            _context.employees.Remove(employee);
            await _context.SaveChangesAsync();
            employee.ImagePath = null; // optional to avoid exposing deleted image URL
            return employee;


        }

        public async Task<List<Employee>> GetAllEmployeesAsync(int pageNumber, int pageSize)
        {

            return await _context.employees.
                Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        }

        public async Task<Employee?> GetEmployeeByIdAsync(int id)
        {
            return await _context.employees.FindAsync(id);
        }
        private void DeleteImageFile(string? imagePath)
        {
            if (string.IsNullOrEmpty(imagePath))
                return;

            var fullPath = Path.Combine
                (_env.WebRootPath, imagePath.TrimStart('/').Replace
                ('/', Path.DirectorySeparatorChar));
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }

        private string SaveImageToUploads(IFormFile image)
        {
            var imageName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
            var uploadPath = Path.Combine(_env.WebRootPath, "uploads");

            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            var fullPath = Path.Combine(uploadPath, imageName);
            using var stream = new FileStream(fullPath, FileMode.Create);
            image.CopyTo(stream);

            return "/uploads/" + imageName;
        }
        public async Task<Employee?> UpdateEmployeeAsync(Employee employee, IFormFile? image)
        {
            var existing = await _context.employees.FindAsync(employee.Id);
            if (existing == null) return null;

            existing.FirstName = employee.FirstName;
            existing.LastName = employee.LastName;
            existing.Email = employee.Email;
            existing.Age = employee.Age;

            if (image != null && image.Length > 0)
            {
                DeleteImageFile(existing.ImagePath);
                existing.ImagePath = SaveImageToUploads(image);
            }

            await _context.SaveChangesAsync();



            return existing;

        }
    }
}

