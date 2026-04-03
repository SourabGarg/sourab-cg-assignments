using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApilnAsp.Models;

namespace WebApilnAsp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // ? Apply authentication globally
    public class EmpController : ControllerBase
    {
        private readonly IEmployee _employeeService;

        public EmpController(IEmployee employeeService)
        {
            _employeeService = employeeService;
        }

        // ? GET ALL
        [HttpGet]
        [Authorize(Roles = "Admin,HR,User")]
        public async Task<IActionResult> GetAll(int pageNumber = 1, int pageSize = 5)
        {
            var data = await _employeeService.GetAllEmployeesAsync(pageNumber, pageSize);
            return Ok(data);
        }

        // ? BASIC LIST WITH SEARCH (secured now)
        [HttpGet("basic")]
        [Authorize(Roles = "Admin,HR,User")]
        public async Task<ActionResult<List<EmployeeBasicDto>>> GetBasicEmployeeList(
            int page = 1, int pageSize = 5, string? search = null)
        {
            var result = await _employeeService.GetAllEmployeeBasicInfoAsync(page, pageSize, search);
            return Ok(result);
        }

        // ? GET BY ID
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,HR,User")]
        public async Task<ActionResult<Employee>> GetById(int id)
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(id);

            if (employee == null)
                return NotFound("Employee not found");

            return Ok(employee);
        }

        // ? CREATE
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromForm] Employee emp, IFormFile? image)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = await _employeeService.AddEmployeeAsync(emp, image);
            return Ok(created);
        }

        // ? UPDATE (Already good, just cleaned)
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Employee>> Update(int id, [FromForm] EmployeeUpdateDto dto, IFormFile? image)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var employee = new Employee
            {
                Id = id,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Age = dto.Age,
                ImagePath = dto.ImagePath
            };

            var updated = await _employeeService.UpdateEmployeeAsync(employee, image);

            if (updated == null)
                return NotFound("Employee not found to update");

            return Ok(updated);
        }

        // ? DELETE (improved message)
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _employeeService.DeleteEmployeeAsync(id);

            if (deleted == null)
                return NotFound("Employee not found");

            return Ok("Employee deleted successfully");
        }

        // ? EXPORT TO EXCEL (improved)
        [HttpGet("export/excel")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ExportToExcel(string? search = null)
        {
            var employees = await _employeeService.GetAllEmployeeBasicInfoAsync(1, int.MaxValue, search);

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Employees");

            // Header
            worksheet.Cell(1, 1).Value = "First Name";
            worksheet.Cell(1, 2).Value = "Last Name";
            worksheet.Cell(1, 3).Value = "Email";
            worksheet.Cell(1, 4).Value = "Image URL";

            // Bold header
            worksheet.Row(1).Style.Font.Bold = true;

            int row = 2;
            foreach (var emp in employees)
            {
                worksheet.Cell(row, 1).Value = emp.FirstName;
                worksheet.Cell(row, 2).Value = emp.LastName;
                worksheet.Cell(row, 3).Value = emp.Email;
                worksheet.Cell(row, 4).Value = emp.ImageUrl;
                row++;
            }

            worksheet.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Seek(0, SeekOrigin.Begin);

            return File(
                stream.ToArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "Employees.xlsx");
        }
    }
}
