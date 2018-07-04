using Energy.Core;
using Energy.Core.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Energy.Infrastructure
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private EnergyDotNetDbContext _context;

        public EmployeeRepository(EnergyDotNetDbContext context)
        {
            _context = context;
        }

        public Employee Add(Employee newEmployee)
        {
            _context.Add(newEmployee);
            return newEmployee;
        }

        public Employee GetById(int id)
        {
            return _context.Employees.FirstOrDefault(e => e.EmployeeId == id);
        }

        public Employee GetByEmail(string email)
        {
            return _context.Employees.FirstOrDefault(e => e.Email == email);
        }

        public void Remove(int id)
        {
            var employee = GetById(id);
            if (employee != null)
            {
                _context.Remove(employee);
            }
        }

        public void Commit()
        {
            _context.SaveChanges();
        }

        public IEnumerable<Employee> GetAll()
        {
            return _context.Employees;
        }
    }
}
