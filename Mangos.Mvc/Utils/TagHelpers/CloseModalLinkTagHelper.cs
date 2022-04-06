using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Mangos.Mvc.Utils.TagHelpers
{
    public class CloseModalLinkTagHelper : TagHelper
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "a";
            output.Attributes.SetAttribute("href", null);
            output.Attributes.SetAttribute("onclick", "javascript: closeModal(); return false;");
        }
    }
}