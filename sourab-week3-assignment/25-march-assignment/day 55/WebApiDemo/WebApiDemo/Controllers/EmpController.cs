using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiDemo.Models;

namespace WebApiDemo.Controllers
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

        public async Task<ActionResult<Employee>> GetById(int id)
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(id);
            if (employee == null)
            {
                return NotFound("Employee not found");
            }
            return Ok(employee);
        }
        public async Task<ActionResult<Employee>> Create([FromForm]Employee employee,IFormFile image)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var added=await _employeeService.AddEmployeeAsync(employee,image);
            return Ok(added);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<Employee>> Update(int Id,[FromForm]Employee employee,IFormFile? image)
        {
            if (Id != employee.Id)
            {
                return BadRequest("ID MISMATCH");

            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var updated=await _employeeService.UpdateEmployeeAsync(employee,image);
            if (updated == null)
            {
                return NotFound("Employee not Found to update");
            }
            return Ok(updated);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<Employee>> Delete(int id)
        {
            var deleted=await _employeeService.DeleteEmployeeAsync(id); 
            if(deleted == null)
            {
                return NotFound("Employee not found for deletion");
            }
            return Ok(deleted);
        }
    }
}
