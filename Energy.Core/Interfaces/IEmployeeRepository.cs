using System.Collections.Generic;

namespace Energy.Core.Interfaces
{
    public interface IEmployeeRepository
    {
        Employee Add(Employee newEmployee);
        Employee GetById(int id);
        Employee GetByEmail(string email);
        void Remove(int id);
        void Commit();
        IEnumerable<Employee> GetAll();
    }
}
