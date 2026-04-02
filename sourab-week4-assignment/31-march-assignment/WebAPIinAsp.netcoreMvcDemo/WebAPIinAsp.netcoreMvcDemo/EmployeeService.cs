using Microsoft.EntityFrameworkCore;
using WebAPIinAsp.netcoreMvcDemo.Models;
using System.IO;



namespace WebAPIinAsp.netcoreMvcDemo
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

        public async Task<Employee> AddEmployeeAsync(Employee employee, IFormFile? image)
        {
            if (image != null && image.Length > 0)
                employee.ImagePath = await SaveImageAsync(image);
            else
                employee.ImagePath = "/uploads/default.jpeg";

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

            return employee;
        }

        public async Task<List<Employee>> GetAllEmployeesAsync(int pageNumber, int pageSize)
        {
            return await _context.employees
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<Employee?> GetEmployeeByIdAsync(int id)
        {
            return await _context.employees.FindAsync(id);
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
                existing.ImagePath = await SaveImageAsync(image);
            }

            await _context.SaveChangesAsync();
            return existing;
        }

        private void DeleteImageFile(string? imagePath)
        {
            if (string.IsNullOrEmpty(imagePath) || imagePath.Contains("default.jpeg"))
                return;

            var fullPath = Path.Combine(_env.WebRootPath ?? "wwwroot",
                imagePath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));

            if (File.Exists(fullPath))
                File.Delete(fullPath);
        }

        private async Task<string> SaveImageAsync(IFormFile image)
        {
            var folder = Path.Combine(_env.WebRootPath ?? "wwwroot", "uploads");

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            var name = Guid.NewGuid() + Path.GetExtension(image.FileName);
            var path = Path.Combine(folder, name);

            using var stream = new FileStream(path, FileMode.Create);
            await image.CopyToAsync(stream);

            return "/uploads/" + name;
        }

        public async Task<List<EmployeeBasicDto>> GetAllEmployeeBasicInfoAsync(int pageNumber, int pageSize, string? searchTerm)
        {
            var query = _context.employees.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(e =>
                    e.FirstName!.Contains(searchTerm) ||
                    e.LastName!.Contains(searchTerm) ||
                    e.Email!.Contains(searchTerm));
            }

            var employees = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return employees.Select(e => new EmployeeBasicDto
            {
                Id = e.Id,
                FirstName = e.FirstName,
                LastName = e.LastName,
                Email = e.Email,
                ImageUrl = string.IsNullOrEmpty(e.ImagePath) ? "/uploads/default.jpeg" : e.ImagePath
            }).ToList();
        }
    }
}