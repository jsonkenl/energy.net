using Energy.Core;
using Energy.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Energy.Net.Features.Admin
{
    public class AdminController : Controller
    {
        private ApplicationOptions _options;
        private IAdministratorRepository _adminRepo;

        public AdminController(IOptions<ApplicationOptions> options,
                                IAdministratorRepository administratorRepository)
        {
            _options = options.Value;
            _adminRepo = administratorRepository;
        }

        [HttpGet]
        public IActionResult CreatePassword()
        {
            ViewData["AdminEmail"] = _options.AdministratorEmail;
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult CreatePassword(AdminViewModel model)
        {
            if (ModelState.IsValid)
            {
                var newAdmin = new Administrator()
                {
                   HashedPassword = BCrypt.Net.BCrypt.HashPassword(model.Password) 
                };

                _adminRepo.Add(newAdmin);
                _adminRepo.Commit();

                TempData["InfoAlert"] = "Congrats! Administrator account successfully created.";

                return RedirectToAction("Index", "Home");
            }
            ViewData["AdminEmail"] = _options.AdministratorEmail;
            TempData["WarningAlert"] = "Oh snap! Unable to save password."; 
            return View();
        }
    }
}
