using Microsoft.EntityFrameworkCore;
using WebApilnAsp.Models;

namespace WebApilnAsp
{
    public class EmployeeService : IEmployee
    {
        private const string DefaultImageRelativePath = "/uploads/default.jpg";
        private readonly EmpContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public EmployeeService(EmpContext context, IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _env = env;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Employee> AddEmployeeAsync(Employee employee, IFormFile? image)
        {
            employee.ImagePath = await SaveImageAsync(image) ?? DefaultImageRelativePath;

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
            employee.ImagePath = BuildAbsoluteImagePath(employee.ImagePath);
            return employee;
        }

        public async Task<List<Employee>> GetAllEmployeesAsync(int pageNumber, int pageSize)
        {
            pageNumber = Math.Max(pageNumber, 1);
            pageSize = Math.Max(pageSize, 1);

            var employees = await _context.Employees
                .AsNoTracking()
                .OrderBy(employee => employee.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            string baseUrl = GetBaseUrl();
            foreach (var employee in employees)
            {
                employee.ImagePath = string.IsNullOrEmpty(employee.ImagePath)
                    ? baseUrl + "/uploads/default.jpg"
                    : baseUrl + NormalizeStoredImagePath(employee.ImagePath);
            }

            return employees;
        }

        public async Task<List<EmployeeBasicDto>> GetAllEmployeeBasicInfoAsync(int pageNumber, int pageSize, string? searchTerm)
        {
            pageNumber = Math.Max(pageNumber, 1);
            pageSize = Math.Max(pageSize, 1);

            var query = _context.Employees.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var trimmedSearchTerm = searchTerm.Trim();
                query = query.Where(employee =>
                    (employee.FirstName != null && employee.FirstName.Contains(trimmedSearchTerm)) ||
                    (employee.LastName != null && employee.LastName.Contains(trimmedSearchTerm)) ||
                    (employee.Email != null && employee.Email.Contains(trimmedSearchTerm)));
            }

            var employees = await query
                .OrderBy(employee => employee.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return employees.Select(employee => new EmployeeBasicDto
            {
                Id = employee.Id,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Email = employee.Email,
                ImageUrl = BuildAbsoluteImagePath(employee.ImagePath)
            }).ToList();
        }

        public async Task<Employee?> GetEmployeeByIdAsync(int id)
        {
            var emp = await _context.Employees
                .AsNoTracking()
                .FirstOrDefaultAsync(employee => employee.Id == id);

            if (emp != null)
            {
                emp.ImagePath = string.IsNullOrEmpty(emp.ImagePath)
                    ? GetBaseUrl() + "/uploads/default.jpg"
                    : GetBaseUrl() + NormalizeStoredImagePath(emp.ImagePath);
            }

            return emp;
        }

        public async Task<Employee?> UpdateEmployeeAsync(Employee employee, IFormFile? image)
        {
            var existing = await _context.Employees.FindAsync(employee.Id);
            if (existing == null)
            {
                return null;
            }

            existing.FirstName = employee.FirstName;
            existing.LastName = employee.LastName;
            existing.Email = employee.Email;
            existing.Age = employee.Age;

            if (image != null && image.Length > 0)
            {
                DeleteImageFile(existing.ImagePath);
                existing.ImagePath = await SaveImageAsync(image) ?? DefaultImageRelativePath;
            }
            else if (string.IsNullOrWhiteSpace(existing.ImagePath))
            {
                existing.ImagePath = DefaultImageRelativePath;
            }

            await _context.SaveChangesAsync();
            existing.ImagePath = BuildAbsoluteImagePath(existing.ImagePath);
            return existing;
        }

        //public async Task<Employee?> DeleteEmployeeAsync(int id)
        //{
        //    var emp = await _context.Employees.FindAsync(id);
        //    if (emp == null) return null;

        //    _context.Employees.Remove(emp);
        //    await _context.SaveChangesAsync();
        //    return emp;
        //}
        public async Task<Employee?> DeleteEmployeeAsync(int id)
        {
            var emp = await _context.Employees.FindAsync(id);
            if (emp == null) return null;

            DeleteImageFile(emp.ImagePath);

            _context.Employees.Remove(emp);
            await _context.SaveChangesAsync();

            emp.ImagePath = BuildAbsoluteImagePath(emp.ImagePath);
            return emp;
        }

        private async Task<string?> SaveImageAsync(IFormFile? image)
        {
            if (image == null || image.Length == 0)
            {
                return null;
            }

            var imageName = Guid.NewGuid() + Path.GetExtension(image.FileName);
            var folderPath = Path.Combine(_env.WebRootPath, "uploads");
            var imagePath = Path.Combine(folderPath, imageName);

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            using var stream = new FileStream(imagePath, FileMode.Create);
            await image.CopyToAsync(stream);

            return "/uploads/" + imageName;
        }

        private void DeleteImageFile(string? imagePath)
        {
            var relativePath = NormalizeStoredImagePath(imagePath);
            if (string.IsNullOrEmpty(relativePath) || relativePath.Contains("default.jpg", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            var fullPath = Path.Combine(_env.WebRootPath, relativePath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }

        public string GetBaseUrl()
        {
           var httpContext = _httpContextAccessor.HttpContext;
           if (httpContext == null) throw new InvalidOperationException("No HttpContext");
           var request = httpContext.Request;
           return $"{request.Scheme}://{request.Host}";
        }

        private string BuildAbsoluteImagePath(string? imagePath)
        {
            var relativePath = NormalizeStoredImagePath(imagePath) ?? DefaultImageRelativePath;
            if (_httpContextAccessor.HttpContext == null)
            {
                return relativePath;
            }

            return $"{GetBaseUrl()}{relativePath}";
        }

        private static string? NormalizeStoredImagePath(string? imagePath)
        {
            if (string.IsNullOrWhiteSpace(imagePath))
            {
                return null;
            }

            if (Uri.TryCreate(imagePath, UriKind.Absolute, out var absoluteUri))
            {
                return absoluteUri.AbsolutePath;
            }

            return imagePath.StartsWith('/') ? imagePath : "/" + imagePath;
        }
    }
}
