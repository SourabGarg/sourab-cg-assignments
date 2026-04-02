using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeFirstEFinAsp.Models
{
    public class Product
    {
        public int ProductID { get; set; }
        
        [Required]
        public string ProductName { get; set; }
        
        [Display(Name ="who buyed")]
        [ForeignKey("Customer")]
        public int CustomerID { get; set; }
       
        public Customer Customer { get; set; }

    }
}
