using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Mangos.Mvc.Utils.TagHelpers
{
    public class ExecuteActionIconTagHelper : TagHelper
    {
        [HtmlAttributeName("controller")]
        public string? Controller { get; set; }

        [HtmlAttributeName("action")]
        public string? Action { get; set; }

        [HtmlAttributeName("value")]
        public string? Value { get; set; }

        [HtmlAttributeName("glyph-icon")]
        public string? GlyphIcon { get; set; }

        [HtmlAttributeName("confirmation-message")]
        public string? ConfirmationMessage { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "a";
            output.Attributes.SetAttribute("href", null);

            if (!string.IsNullOrEmpty(ConfirmationMessage))
                output.Attributes.SetAttribute("onclick", $"javascript: if (confirm('{ConfirmationMessage}')) {{ executeAction('{Controller}', '{Action}', '{Value}'); }} return false;");
            else
                output.Attributes.SetAttribute("onclick", $"javascript: executeAction('{Controller}', '{Action}', '{Value}'); return false;");

            var span = new TagBuilder("span");
            span.AddCssClass("fa");

            if (!string.IsNullOrEmpty(GlyphIcon))
                span.AddCssClass($"fa-{GlyphIcon}");

            span.MergeAttribute("aria-hidden", "true");

            output.Content.AppendHtml(span);
        }
    }
}