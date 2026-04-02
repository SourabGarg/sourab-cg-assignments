using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPIMVC.Models;

namespace WebAPIMVC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmpController : ControllerBase
    {
        private readonly IEmployee _employeeService;
        public EmpController(IEmployee employeeService)
        {
            _employeeService = employeeService;
        }
        [HttpGet]
        public async Task<ActionResult<List<Employee>>> GetAll(int page=1,int pageSize=5)
        { 
            
            return Ok(await _employeeService.GetAllEmployeesAsync(page,pageSize));
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetById(int id)
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(id);
            if (employee == null)
                return NotFound("Employee not found");
            return Ok(employee);
        }

        [HttpPost]
        public async Task<ActionResult<Employee>> Create([FromForm]Employee employee,IFormFile image)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var added = await _employeeService.AddEmployeeAsync(employee,image);
            return Ok(added);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<Employee>> Update(
        int id, [FromForm] EmployeeUpdateDto employeeDto, IFormFile? image)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // map dto to entity
            var employee = new Employee
            {
                Id = id, // take from route only
                FirstName = employeeDto.FirstName,
                LastName = employeeDto.LastName,
                Email = employeeDto.Email,
                Age = employeeDto.Age,
                ImagePath = employeeDto.ImagePath
            };

            var updated = await _employeeService.UpdateEmployeeAsync(employee, image);
            if (updated == null)
                return NotFound("Employee not found to update");

            return Ok(updated);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<Employee>> Delete(int id)
        {
            var deleted = await _employeeService.DeleteEmployeeAsync(id);
            if (deleted == null)
                return NotFound("Employee not foudn to delete");
            return Ok(deleted);

        }
    }
}
