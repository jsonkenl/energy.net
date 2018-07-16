using Energy.Core;
using Energy.Core.Interfaces;
using Energy.Net.Features.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Energy.Net.Features.Employees
{
    public class EmployeesController : ApplicationController 
    {
        private IEmployeeRepository _employeeRepo;

        public EmployeesController(IOptions<ApplicationOptions> options,
                                    IEmployeeRepository employeeRepository)
            : base(options)
        {
            _employeeRepo = employeeRepository;
        }

        [HttpGet("EmployeeDirectory")]
        public IActionResult Index()
        {
            var employee = _employeeRepo.GetByEmail("EnergyAdmin@EnergyCo.net");

            ViewData["AdminDn"] = Options.AdministratorDistinguishedName;
            ViewData["Employee"] = employee;

            return View();
        }
    }
}
