using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models;

public class Employee
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string Department { get; set; } = string.Empty;

    [Range(typeof(decimal), "0.01", "79228162514264337593543950335")]
    public decimal Salary { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime JoiningDate { get; set; }
}
