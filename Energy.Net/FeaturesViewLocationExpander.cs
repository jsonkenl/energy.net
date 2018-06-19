using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Razor;
using System.Collections.Generic;

namespace Energy.Net
{
    public class FeaturesViewLocationExpander : IViewLocationExpander
    {
        public void PopulateValues(ViewLocationExpanderContext context)
        {
            context.Values["customviewlocation"] = nameof(FeaturesViewLocationExpander);
        }

        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
        {
            var controllerActionDescriptor = context.ActionContext.ActionDescriptor as ControllerActionDescriptor;
            string featureName = controllerActionDescriptor.Properties["feature"] as string;
            foreach (var location in viewLocations)
            {
                yield return location.Replace("{3}", featureName);
            }
        }
    }
}
