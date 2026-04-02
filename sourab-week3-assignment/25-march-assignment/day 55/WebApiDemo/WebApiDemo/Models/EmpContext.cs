using Microsoft.EntityFrameworkCore;

namespace WebApiDemo.Models
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
