using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers;

public class EmployeesController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<EmployeesController> _logger;

    public EmployeesController(ApplicationDbContext context, ILogger<EmployeesController> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        try
        {
            var employees = await _context.Employees.AsNoTracking().ToListAsync();
            return View(employees);
        }
        catch (Exception ex) when (IsAzureSqlConnectionError(ex))
        {
            _logger.LogError(ex, "Failed to load employees due to Azure SQL connectivity issue.");
            ViewData["ErrorMessage"] = "Database is temporarily unavailable. Please try again in a moment.";
            return View(Enumerable.Empty<Employee>());
        }
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        try
        {
            var employee = await _context.Employees
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);

            if (employee is null)
            {
                return NotFound();
            }

            return View(employee);
        }
        catch (Exception ex) when (IsAzureSqlConnectionError(ex))
        {
            return HandleDatabaseException(ex);
        }
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Name,Department,Salary,JoiningDate")] Employee employee)
    {
        if (!ModelState.IsValid)
        {
            return View(employee);
        }

        try
        {
            _context.Add(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex) when (IsAzureSqlConnectionError(ex))
        {
            _logger.LogError(ex, "Failed to create employee due to Azure SQL connectivity issue.");
            ModelState.AddModelError(string.Empty, "Unable to save changes right now. Please try again.");
            return View(employee);
        }
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        try
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee is null)
            {
                return NotFound();
            }

            return View(employee);
        }
        catch (Exception ex) when (IsAzureSqlConnectionError(ex))
        {
            return HandleDatabaseException(ex);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Department,Salary,JoiningDate")] Employee employee)
    {
        if (id != employee.Id)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return View(employee);
        }

        try
        {
            _context.Update(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await EmployeeExists(employee.Id))
            {
                return NotFound();
            }

            throw;
        }
        catch (Exception ex) when (IsAzureSqlConnectionError(ex))
        {
            _logger.LogError(ex, "Failed to update employee due to Azure SQL connectivity issue.");
            ModelState.AddModelError(string.Empty, "Unable to save changes right now. Please try again.");
            return View(employee);
        }
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        try
        {
            var employee = await _context.Employees
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employee is null)
            {
                return NotFound();
            }

            return View(employee);
        }
        catch (Exception ex) when (IsAzureSqlConnectionError(ex))
        {
            return HandleDatabaseException(ex);
        }
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee is not null)
            {
                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex) when (IsAzureSqlConnectionError(ex))
        {
            return HandleDatabaseException(ex);
        }
    }

    private async Task<bool> EmployeeExists(int id)
    {
        return await _context.Employees.AnyAsync(e => e.Id == id);
    }

    private IActionResult HandleDatabaseException(Exception ex)
    {
        _logger.LogError(ex, "Azure SQL connectivity issue in EmployeesController.");
        TempData["ErrorMessage"] = "Database is temporarily unavailable. Please try again in a moment.";
        return RedirectToAction(nameof(Index));
    }

    private static bool IsAzureSqlConnectionError(Exception exception)
    {
        if (exception is SqlException sqlException)
        {
            return sqlException.Number is -2 or 4060 or 10928 or 10929 or 40197 or 40501 or 40613 or 49918 or 49919 or 49920;
        }

        return exception.InnerException is not null && IsAzureSqlConnectionError(exception.InnerException);
    }
}
