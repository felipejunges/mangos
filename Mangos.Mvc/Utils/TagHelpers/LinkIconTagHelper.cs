using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Mangos.Mvc.Utils.TagHelpers
{
    [HtmlTargetElement("a", Attributes = GlyphIconAttributeName)]
    public class LinkIconTagHelper : TagHelper
    {
        private const string GlyphIconAttributeName = "glyph-icon";

        [HtmlAttributeName(GlyphIconAttributeName)]
        public string? GlyphIcon { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var span = new TagBuilder("span");
            span.AddCssClass("fa");

            if (!string.IsNullOrEmpty(GlyphIcon))
                span.AddCssClass($"fa-{GlyphIcon}");

            span.MergeAttribute("aria-hidden", "true");

            output.PreContent.AppendHtml(span);
            output.PreContent.AppendHtml(" ");
        }
    }
}