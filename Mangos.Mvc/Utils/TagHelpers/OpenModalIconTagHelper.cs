using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Mangos.Mvc.Utils.TagHelpers
{
    public class OpenModalIconTagHelper : TagHelper
    {
        [HtmlAttributeName("controller")]
        public string? Controller { get; set; }

        [HtmlAttributeName("action")]
        public string? Action { get; set; }

        [HtmlAttributeName("value")]
        public string? Value { get; set; }

        [HtmlAttributeName("glyph-icon")]
        public string? GlyphIcon { get; set; }

        [HtmlAttributeName("query-name")]
        public string? QueryName { get; set; }

        [HtmlAttributeName("query-value")]
        public string? QueryValue { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "a";
            output.Attributes.SetAttribute("href", null);
            output.Attributes.SetAttribute("onclick", $"javascript: openModal('{Controller}', '{Action}', {Value}, '{QueryName}', '{QueryValue}'); return false;");

            var span = new TagBuilder("span");
            span.AddCssClass("fa");

            if (!string.IsNullOrEmpty(GlyphIcon))
                span.AddCssClass($"fa-{GlyphIcon}");

            span.MergeAttribute("aria-hidden", "true");

            output.Content.AppendHtml(span);
        }
    }
}