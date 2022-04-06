using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Linq;

namespace Mangos.Mvc.Utils.TagHelpers.Extensions
{
    public static class TagHelperExtensions
    {
        public static void AddCssClass(this TagHelperOutput output, string @class)
        {
            string? classes = output.Attributes.FirstOrDefault(a => a.Name == "class")?.Value.ToString();
            classes = string.IsNullOrEmpty(classes) ? @class : $"{classes} {@class}";

            output.Attributes.SetAttribute("class", classes);
        }
    }
}