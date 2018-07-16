using Energy.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;

namespace Energy.Net.Features.Shared
{
    public abstract class ApplicationController : Controller
    {
        public ApplicationOptions Options { get; }

        public ApplicationController(IOptions<ApplicationOptions> options)
        {
            Options = options.Value;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            ViewData["Options"] = Options;
        }
    }
}
