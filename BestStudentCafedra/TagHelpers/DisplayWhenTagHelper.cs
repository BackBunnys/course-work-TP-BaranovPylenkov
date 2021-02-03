using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Collections.Generic;
using System.Security.Claims;
using System.Linq;

namespace BestStudentCafedra.TagHelpers
{
    [HtmlTargetElement(Attributes = "display-when")]
    public class DisplayWhenTagHelper : TagHelper
    {
        public bool DisplayWhen { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = context.TagName;

            if (!DisplayWhen)
                output.SuppressOutput();
        }
    }
}
