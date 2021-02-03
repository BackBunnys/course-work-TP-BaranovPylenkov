using Microsoft.AspNetCore.Razor.TagHelpers;

namespace BestStudentCafedra.TagHelpers
{
    [HtmlTargetElement(Attributes = "display-when")]
    public class DisplayWhenTagHelper : TagHelper
    {
        public bool DisplayWhen { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (!DisplayWhen)
                output.SuppressOutput();
        }
    }
}
