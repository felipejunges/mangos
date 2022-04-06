using Mangos.Dominio.Services.Relatorios;
using Mangos.Dominio.Services.User;
using Mangos.Mvc.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Mangos.Mvc.Controllers.Relatorios
{
    [Authorize]
    public class RelatorioCategoriaMensalController : BaseController
    {
        private readonly RelatorioCategoriaService _relatorioCategoriaService;

        public RelatorioCategoriaMensalController(DataKeeperService dataKeeperService, IUserResolverService userResolverService, RelatorioCategoriaService relatorioCategoriaService) : base(dataKeeperService, userResolverService)
        {
            _relatorioCategoriaService = relatorioCategoriaService;
        }

        public async Task<IActionResult> Index()
        {
            DateTime mesInicial = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-11);
            DateTime mesFinal = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            ViewData["MesInicial"] = mesInicial.ToString("MM/yyyy");
            ViewData["MesFinal"] = mesFinal.ToString("MM/yyyy");
            ViewData["MesInicialDate"] = mesInicial;
            ViewData["MesFinalDate"] = mesFinal;

            var relatorio = await _relatorioCategoriaService.ListarRelatorioCategoriaMensalAsync(_userResolverService.GrupoId, mesInicial, mesFinal, "T", "R", "A", false);

            return View(relatorio);
        }

        public async Task<IActionResult> IndexLista(string mesInicial, string mesFinal, string situacao, string ordenacao, bool agruparSubcategorias)
        {
            DateTime mesInicialDate = mesInicial != null ? Convert.ToDateTime("01/" + mesInicial) : DateTime.Now.AddDays((DateTime.Now.Day - 1) * -1);
            DateTime mesFinalDate = mesFinal != null ? Convert.ToDateTime("01/" + mesFinal) : DateTime.Now.AddDays((DateTime.Now.Day - 1) * -1);

            ViewData["MesInicialDate"] = mesInicialDate;
            ViewData["MesFinalDate"] = mesFinalDate;

            var relatorio = await _relatorioCategoriaService.ListarRelatorioCategoriaMensalAsync(_userResolverService.GrupoId, mesInicialDate, mesFinalDate, "T", situacao, ordenacao, agruparSubcategorias);

            return PartialView(relatorio);
        }
    }
}