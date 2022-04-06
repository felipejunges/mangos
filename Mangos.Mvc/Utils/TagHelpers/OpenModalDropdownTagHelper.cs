using Mangos.Mvc.Utils.TagHelpers.Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Mangos.Mvc.Utils.TagHelpers
{
    public class OpenModalDropdownTagHelper : TagHelper
    {
        [HtmlAttributeName("opcoes")]
        public DropdownOption[]? Opcoes { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "button";
            output.AddCssClass("dropdown-toggle dropdown-toggle-split");
            output.Attributes.Add("type", "button");
            output.Attributes.Add("data-toggle", "dropdown");
            output.Attributes.Add("aria-haspopup", "true");
            output.Attributes.Add("aria-expanded", "false");

            var span1 = new TagBuilder("span");
            span1.AddCssClass("sr-only");
            span1.InnerHtml.Append("Toggle Dropdown");

            output.Content.AppendHtml(span1);

            //
            var div = new TagBuilder("div");
            div.AddCssClass("dropdown-menu");

            if (Opcoes != null)
            {
                foreach (var opcao in this.Opcoes)
                {
                    if (opcao.Divider)
                    {
                        var divSep = new TagBuilder("div");

                        divSep.AddCssClass("dropdown-divider");
                        divSep.MergeAttribute("role", "separator");

                        div.InnerHtml.AppendHtml(divSep);
                    }
                    else
                    {
                        var a = new TagBuilder("a");
                        a.AddCssClass("dropdown-item");
                        a.MergeAttribute("href", string.Empty);
                        a.MergeAttribute("onclick", "javascript: openModal('" + opcao.Controller + "', '" + opcao.Action + "', " + opcao.Value + "); return false;");
                        a.InnerHtml.Append(opcao.Text);

                        div.InnerHtml.AppendHtml(a);
                    }
                }
            }

            output.PostElement.AppendHtml(div);
        }
    }

    public class DropdownOption
    {
        public string Text { get; set; }

        public string Controller { get; set; }
        public string Action { get; set; }
        public string Value { get; set; }

        public bool Divider { get; set; }

        public DropdownOption()
        {
            this.Text = string.Empty;
            this.Controller = string.Empty;
            this.Action = string.Empty;
            this.Value = string.Empty;
            this.Divider = true;
        }

        public DropdownOption(string text, string controller, string action, string value)
        {
            this.Text = text;
            this.Controller = controller;
            this.Action = action;
            this.Value = value;
            this.Divider = false;
        }
    }
}