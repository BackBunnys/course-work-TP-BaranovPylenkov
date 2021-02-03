using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace BestStudentCafedra.TagHelpers
{
    [HtmlTargetElement(Attributes = "display-for")]
    public class DisplayForRolesTagHelper : TagHelper
    {
        public string DisplayFor { get; set; }
        
        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            List<string> roles = new List<string>(DisplayFor.Split(','));

            if (roles.Any(x => ViewContext.HttpContext.User.IsInRole(x.Trim())))
                return;   

            output.SuppressOutput();
        }
    }
}
