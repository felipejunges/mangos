using Mangos.Mvc.Utils.TagHelpers.Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Mangos.Mvc.Utils.TagHelpers
{
    [HtmlTargetElement("pagination", Attributes = "pagina-atual, total-paginas")]
    public class PaginationTagHelper : TagHelper
    {
        [HtmlAttributeName("pagina-atual")]
        public int PaginaAtual { get; set; }

        [HtmlAttributeName("total-paginas")]
        public int TotalPaginas { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "nav";
            output.Attributes.SetAttribute("aria-label", "Paginação");
            output.AddCssClass("p-3");

            var ul = new TagBuilder("ul");
            ul.AddCssClass("pagination");
            ul.AddCssClass("pagination-sm");
            ul.AddCssClass("justify-content-center");

            var primeiroLi = CriarLi("&lt;&lt;", 1, PaginaAtual > 1, false);
            var liAnterior = CriarLi("&lt;", PaginaAtual - 1, PaginaAtual > 1, false);
            ul.InnerHtml.AppendHtml(primeiroLi);
            ul.InnerHtml.AppendHtml(liAnterior);

            var (iInicial, iFinal) = SetarIs(PaginaAtual, TotalPaginas, 6);

            for (int i = iInicial; i <= iFinal; i++)
            {
                var li = CriarLi(i.ToString(), i, true, i == PaginaAtual);
                ul.InnerHtml.AppendHtml(li);
            }

            var proximoLi = CriarLi("&gt;", PaginaAtual + 1, PaginaAtual < TotalPaginas, false);
            var ultimoLi = CriarLi("&gt;&gt;", TotalPaginas, PaginaAtual < TotalPaginas, false);
            ul.InnerHtml.AppendHtml(proximoLi);
            ul.InnerHtml.AppendHtml(ultimoLi);

            output.Content.AppendHtml(ul);
        }

        private (int iInicial, int iFinal) SetarIs(int paginaAtual, int totalPaginas, int step)
        {
            int iInicial = paginaAtual - step;
            int iFinal = paginaAtual + step;

            while (iInicial < 1)
            {
                if (iFinal < totalPaginas) iFinal++;

                iInicial++;
            }

            while (iFinal > totalPaginas)
            {
                if (iInicial > 1) iInicial--;

                iFinal--;
            }

            return (iInicial, iFinal);
        }

        private static TagBuilder CriarLi(string texto, int pagina, bool ativo, bool selecionado)
        {
            var li = new TagBuilder("li");
            li.AddCssClass("page-item");

            if (selecionado)
            {
                var span = new TagBuilder("span");
                span.AddCssClass("page-link");
                span.InnerHtml.AppendHtml(texto);

                li.AddCssClass("active");
                li.MergeAttribute("aria-current", "page");
                li.InnerHtml.AppendHtml(span);
            }
            else if (!ativo)
            {
                var span = new TagBuilder("span");
                span.AddCssClass("page-link");
                span.InnerHtml.AppendHtml(texto);

                li.AddCssClass("disabled");
                li.InnerHtml.AppendHtml(span);
            }
            else
            {
                var a = new TagBuilder("a");
                a.AddCssClass("page-link");
                a.MergeAttribute("href", "#");
                a.MergeAttribute("onclick", $"carregarPagina({pagina}); return false;");
                a.InnerHtml.AppendHtml(texto);

                li.InnerHtml.AppendHtml(a);
            }

            return li;
        }
    }
}