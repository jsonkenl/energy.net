using Energy.Core;
using System.Linq;
using System.Threading.Tasks;

namespace Energy.Infrastructure.SeedData
{
    public class AdminEmployeeSeedData
    {
        private EnergyDotNetDbContext _context;

        public AdminEmployeeSeedData(EnergyDotNetDbContext context)
        {
            _context = context;
        }

        public async Task EnsureAdminEmployee(string adminEmail, string adminDn)
        {
            if (!_context.Employees.Any())
            {
                var admin = new Employee()
                {
                    FirstName = "EnergyCo",
                    LastName = "Administrator",
                    Email = adminEmail,
                    DistinguishedName = adminDn,
                    Role = Role.Administrator
                };

                _context.Employees.Add(admin);
                await _context.SaveChangesAsync();
            }
        }
    }
}
