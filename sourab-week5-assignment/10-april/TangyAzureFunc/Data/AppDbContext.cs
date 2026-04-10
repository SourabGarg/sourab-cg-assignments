using Microsoft.EntityFrameworkCore;
using TangyAzureFunc.Models;

namespace TangyAzureFunc.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<SalesRequest> SalesRequests => Set<SalesRequest>();
}
