using System;
using Energy.Core;
using Energy.Core.Interfaces;
using Energy.Net.Features.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Energy.Net.Features.Session
{
    public class SessionController : Controller
    {
        private readonly ApplicationOptions _options;
        private readonly IAdministratorRepository _adminRepo;
        private readonly IEmployeeRepository _employeeRepo;
        private readonly Ldap _authentication;

        public SessionController(IOptions<ApplicationOptions> options,
                                    IEmployeeRepository employeeRepository,
                                    IAdministratorRepository administratorRepository)
        {
            _options = options.Value;
            _employeeRepo = employeeRepository;
            _adminRepo = administratorRepository;
            _authentication = new Ldap(_options, _adminRepo.Get());
        }

        [HttpGet]
        public IActionResult Login()
        {
            ViewData["AdminEmail"] = _options.AdministratorEmail;  
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var _employee = _employeeRepo.GetByEmail(model.Email);

                    if (_employee.DistinguishedName == null)
                    {
                        TempData["WarningAlert"] = "Oh snap! Active Directory DN Error. Please contact the administrator to resolve.";
                        return View(model);
                    }

                    if (_authentication.Validate(_employee.DistinguishedName, model.Password))
                    {
                        var identity = new ClaimsIdentity(new EmployeeIdentity(_employee.DistinguishedName));
                        identity.AddClaim(new Claim(ClaimTypes.Role, _employee.Role.ToString(), ClaimValueTypes.String));
                        var principal = new ClaimsPrincipal(identity);

                        await HttpContext.SignInAsync(principal);

                        TempData["InfoAlert"] = "Congrats! Login successful.";
                        return RedirectToAction("Index", "Home");
                    }

                    ModelState.AddModelError("", "Incorrect Username and/or Password.");
                    return View(model);
                }
                catch (Exception)
                {
                    TempData["WarningAlert"] = "Oh snap! Unable to login. Please contact the administrator.";
                    return View(model);
                }
            }

            TempData["WarningAlert"] = "Oh snap! Unable to log in. Please try again.";
            return View(model);
        }

        [HttpGet]
        public IActionResult Forbidden()
        {
            ViewData["AdminEmail"] = _options.AdministratorEmail;  
            return View();
        }

        [Authorize]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await HttpContext.SignOutAsync();

                return RedirectToAction("Index", "Home");
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Oh snap! There was an error when trying to log out. Please contact the site administrator.");
                return View();
            }
        }
    }
}
