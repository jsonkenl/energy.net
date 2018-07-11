using Energy.Core;
using Microsoft.EntityFrameworkCore;

namespace Energy.Infrastructure
{
    public class EnergyDotNetDbContext : DbContext
    {
        public EnergyDotNetDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Administrator> Administrators { get; set; }
        public DbSet<Employee> Employees { get; set; }
    }
}
