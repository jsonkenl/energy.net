using Energy.Core;
using Energy.Core.Interfaces;
using System.Linq;

namespace Energy.Infrastructure
{
    public class AdministratorRepository : IAdministratorRepository
    {
        private EnergyDotNetDbContext _context;

        public AdministratorRepository(EnergyDotNetDbContext context)
        {
            _context = context;
        }
        public void Add(Administrator newAdministrator)
        {
            _context.Add(newAdministrator);
        }

        public void Commit()
        {
            _context.SaveChanges();
        }

        public Administrator Get()
        {
            return _context.Administrators.First();
        }
    }
}
