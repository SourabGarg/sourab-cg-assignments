using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPIinAsp.netcoreMvcDemo;
using WebAPIinAsp.netcoreMvcDemo.Models;




namespace WebApiInAsp.netcoreMvcDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmpController : ControllerBase
    {
        private readonly IEmployee _service;

        public EmpController(IEmployee service)
        {
            _service = service;
        }

        private string GetFullUrl(string? path)
        {
            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            if (string.IsNullOrEmpty(path)) return baseUrl + "/uploads/default.jpeg";
            if (path.StartsWith("http")) return path;
            return baseUrl + path;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(int page = 1, int pageSize = 5)
        {
            var data = await _service.GetAllEmployeesAsync(page, pageSize);

            var result = data.Select(e => new
            {
                id = e.Id,
                firstName = e.FirstName,
                lastName = e.LastName,
                email = e.Email,
                age = e.Age,
                imageUrl = GetFullUrl(e.ImagePath)
            });

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var e = await _service.GetEmployeeByIdAsync(id);
            if (e == null) return NotFound();

            return Ok(new
            {
                id = e.Id,
                firstName = e.FirstName,
                lastName = e.LastName,
                email = e.Email,
                age = e.Age,
                imageUrl = GetFullUrl(e.ImagePath)
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] Employee emp, IFormFile? image)
        {
            var added = await _service.AddEmployeeAsync(emp, image);
            return Ok(added);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] EmployeeUpdateDto dto, IFormFile? image)
        {
            var emp = new Employee
            {
                Id = id,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Age = dto.Age,
                ImagePath = dto.ImagePath
            };

            var updated = await _service.UpdateEmployeeAsync(emp, image);
            if (updated == null) return NotFound();

            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteEmployeeAsync(id);
            if (deleted == null) return NotFound();

            return Ok(deleted);
        }

        [HttpGet("basic")]
        public async Task<IActionResult> Basic(int page = 1, int pageSize = 5, string? search = null)
        {
            var data = await _service.GetAllEmployeeBasicInfoAsync(page, pageSize, search);
            return Ok(data);
        }

        [HttpGet("export/excel")]
        public async Task<IActionResult> Export(string? search = null)
        {
            var list = await _service.GetAllEmployeeBasicInfoAsync(1, int.MaxValue, search);

            using var wb = new XLWorkbook();
            var ws = wb.Worksheets.Add("Employees");

            ws.Cell(1, 1).Value = "First Name";
            ws.Cell(1, 2).Value = "Last Name";
            ws.Cell(1, 3).Value = "Email";
            ws.Cell(1, 4).Value = "Image URL";

            int r = 2;
            foreach (var e in list)
            {
                ws.Cell(r, 1).Value = e.FirstName;
                ws.Cell(r, 2).Value = e.LastName;
                ws.Cell(r, 3).Value = e.Email;
                ws.Cell(r, 4).Value = e.ImageUrl;
                r++;
            }

            using var stream = new MemoryStream();
            wb.SaveAs(stream);
            stream.Position = 0;

            return File(stream.ToArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "Employees.xlsx");
        }
    }
}