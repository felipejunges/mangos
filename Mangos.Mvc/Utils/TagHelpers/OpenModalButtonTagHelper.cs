using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Mangos.Mvc.Utils.TagHelpers
{

    public class OpenModalButtonTagHelper : TagHelper
    {
        [HtmlAttributeName("id")]
        public string? Id { get; set; }

        [HtmlAttributeName("controller")]
        public string? Controller { get; set; }

        [HtmlAttributeName("action")]
        public string? Action { get; set; }

        [HtmlAttributeName("value")]
        public string? Value { get; set; }

        [HtmlAttributeName("query-name")]
        public string? QueryName { get; set; }

        [HtmlAttributeName("query-value")]
        public string? QueryValue { get; set; }

        [HtmlAttributeName("glyph-icon")]
        public string? GlyphIcon { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "button";
            output.Attributes.SetAttribute("onclick", $"javascript: openModal('{Controller}', '{Action}', {Value}, '{QueryName}', '{QueryValue}'); return false;");

            if (!string.IsNullOrEmpty(Id))
                output.Attributes.SetAttribute("id", Id);

            if (!string.IsNullOrEmpty(GlyphIcon))
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
}