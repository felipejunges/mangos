using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Mangos.Mvc.Utils.TagHelpers
{
    public class ExcluirIconTagHelper : TagHelper
    {
        private const string ControllerAttributeName = "controller";
        private const string ValueAttributeName = "value";

        [HtmlAttributeName(ControllerAttributeName)]
        public string? Controller { get; set; }

        [HtmlAttributeName(ValueAttributeName)]
        public string? Value { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "a";
            output.Attributes.SetAttribute("href", null);
            output.Attributes.SetAttribute("onclick", $"javascript: if (confirm('Confirma a exclusão?')) {{ excludeItem('{Controller}', {Value}); }} return false;");

            var span = new TagBuilder("span");
            span.AddCssClass("fa");
            span.AddCssClass("fa-trash");
            span.MergeAttribute("aria-hidden", "true");

            output.Content.AppendHtml(span);
        }
    }
}