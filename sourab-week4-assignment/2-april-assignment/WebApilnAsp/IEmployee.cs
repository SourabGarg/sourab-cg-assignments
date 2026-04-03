using Microsoft.AspNetCore.Mvc;
using WebApilnAsp.Models;

namespace WebApilnAsp
{
    public interface IEmployee
    {
        Task<List<Employee>> GetAllEmployeesAsync(int pageNumber, int pageSize);
        Task<List<EmployeeBasicDto>> GetAllEmployeeBasicInfoAsync(int pageNumber, int pageSize, string? searchTerm);
        Task<Employee?> GetEmployeeByIdAsync(int id);
        Task<Employee> AddEmployeeAsync(Employee employee, IFormFile? image);
        Task<Employee?> UpdateEmployeeAsync(Employee employee, IFormFile? image);
        Task<Employee?> DeleteEmployeeAsync(int id);
    }
}
