using Microsoft.EntityFrameworkCore;

namespace WebAPIMVC.Models
{
    public class EmpContext : DbContext
    {
        public EmpContext(DbContextOptions dbContextOptions) :
            base(dbContextOptions)
        {

        }

        public DbSet<Employee> employees { set; get; }
    }
}
