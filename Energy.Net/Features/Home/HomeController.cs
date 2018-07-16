using Energy.Core;
using Energy.Infrastructure;
using Energy.Net.Features.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Linq;

namespace Energy.Net.Features.Home
{
    public class HomeController : ApplicationController 
    {
        private EnergyDotNetDbContext _context;

        public HomeController(IOptions<ApplicationOptions> options,
                                EnergyDotNetDbContext context)
            : base(options)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            if (!_context.Administrators.Any())
            {
                return RedirectToAction("CreatePassword", "Admin");
            }
            else
            {
                return View();
            }
        }
    }
}
