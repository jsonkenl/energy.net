using Energy.Core;
using Energy.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Energy.Net.Features.Employees
{
    public class EmployeesController : Controller
    {
        private ApplicationOptions _options;
        private IEmployeeRepository _employeeRepo;

        public EmployeesController(IOptions<ApplicationOptions> options,
                                    IEmployeeRepository employeeRepository)
        {
            _options = options.Value;
            _employeeRepo = employeeRepository;
        }

        [HttpGet("EmployeeDirectory")]
        public IActionResult Index()
        {
            var employee = _employeeRepo.GetByEmail("EnergyAdmin@EnergyCo.net");

            ViewData["AdminDn"] = _options.AdministratorDistinguishedName;
            ViewData["Employee"] = employee;

            return View();
        }
    }
}
