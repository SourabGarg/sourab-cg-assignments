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
        private readonly EmpContext _context;
        public EmpController(EmpContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult <List<Employee>>>getemployees()
        {
            return Ok(await _context.employees.ToListAsync());
        }
        [HttpGet]
        [Route("emp2")]
        public List<Employee> getemployees2()
        {
            return _context.employees.ToList();
        }
        [HttpPost]
        public async Task<ActionResult<List<Employee>>> AddEmployee(Employee emp)
        {
            await _context.employees.AddAsync(emp);
            await _context.SaveChangesAsync();
            return Ok(await _context.employees.ToListAsync());
        }
        [HttpPost]
        [Route("emp_post2")]
        public async Task<ActionResult<Employee>> AddEmployee2(Employee emp)
        {
            await _context.employees.AddAsync(emp);
            await _context.SaveChangesAsync();
            return Ok(emp);
        }
        [HttpPut]
        public async Task<ActionResult<List<Employee>>> UpdateEmployee(Employee emp)
        {
            var employee = await _context.employees.FindAsync(emp.Id);
            if (employee == null)
            {
                return BadRequest("Employee not found ");
            }
            employee.FirstName = emp.FirstName;
            employee.LastName = emp.LastName;
            employee.Email = emp.Email;
            employee.Age = emp.Age;
            await _context.SaveChangesAsync();
            return Ok(await _context.employees.ToListAsync());
        }
        [HttpPut]
        [Route("put2")]
        public async Task<ActionResult<Employee>> UpdateEmployee2(Employee emp)
        {
            var employee = await _context.employees.FindAsync(emp.Id);
            if (employee == null)
            {
                return BadRequest("Employee not found ");
            }
            employee.FirstName = emp.FirstName;
            employee.LastName = emp.LastName;
            employee.Email = emp.Email;
            employee.Age = emp.Age;
            await _context.SaveChangesAsync();
            return Ok(employee);
        }
        [HttpPut]
        [Route("put3")]
        public async Task<ActionResult<Employee>> UpdateEmployee3(Employee emp, int id)
        {
            var employee = await _context.employees.FindAsync(id);

            if (employee == null)
            {
                return BadRequest("Employee not found");
            }

            employee.FirstName = emp.FirstName;
            employee.LastName = emp.LastName;
            employee.Email = emp.Email;
            employee.Age = emp.Age;

            await _context.SaveChangesAsync();

            return Ok(employee);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<List<Employee>>> DeleteEmployee(int id)
        {
            var employee = await _context.employees.FindAsync(id);
            if (employee == null)
            {
                return BadRequest("Emloyee not foud");

            }
            _context.employees.Remove(employee);
            await _context.SaveChangesAsync();
            return Ok(await _context.employees.ToListAsync());
        }
        [HttpDelete("del2/{id}")]
        public async Task<ActionResult<Employee>> DeleteEmployee2(int id)
        {
            var employee = await _context.employees.FindAsync(id);
            if (employee == null)
            {
                return BadRequest("Emloyee not foud");

            }
            _context.employees.Remove(employee);
            await _context.SaveChangesAsync();
            return Ok(employee);
        }

       

                [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployeesById(int id)
        {
            var employee = await _context.employees.FindAsync(id);
            if (employee == null)
            {
                return BadRequest("Emloyee not foud");

            }
            return Ok(employee);
        }
    }
}
