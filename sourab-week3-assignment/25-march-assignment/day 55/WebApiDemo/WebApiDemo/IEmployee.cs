using WebApiDemo.Models;

namespace WebApiDemo
{
    public interface IEmployee
    {
        Task<List<Employee>> GetAllEmployeesAsync(int pageNumber,int pageSize);
        Task<Employee?> GetEmployeeByIdAsync(int id);
        Task<Employee> AddEmployeeAsync(Employee employee,IFormFile image);
        Task<Employee?> UpdateEmployeeAsync(Employee employee,IFormFile? image);
        Task<Employee?> DeleteEmployeeAsync(int id);
    }
}
