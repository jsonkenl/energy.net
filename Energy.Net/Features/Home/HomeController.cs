using Energy.Core;
using Energy.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Linq;

namespace Energy.Net.Features.Home
{
    public class HomeController : Controller
    {
        private ApplicationOptions _options;
        private EnergyDotNetDbContext _context;

        public HomeController(IOptions<ApplicationOptions> options,
                                EnergyDotNetDbContext context)
        {
            _options = options.Value;
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
