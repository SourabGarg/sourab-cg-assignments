using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApilnAsp.Models;

namespace WebApilnAsp.Models
{
    public class EmpContext : IdentityDbContext<IdentityUser>
    {
        public EmpContext(DbContextOptions<EmpContext> options)
            : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            SeedRoles(modelBuilder);
        }

        private static void SeedRoles(ModelBuilder builder)
        {
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                    ConcurrencyStamp = "1"
                },
                new IdentityRole
                {
                    Name = "User",
                    NormalizedName = "USER",
                    ConcurrencyStamp = "2"
                },
                new IdentityRole
                {
                    Name = "HR",
                    NormalizedName = "HR",
                    ConcurrencyStamp = "3"
                }
            );
        }
    }
}