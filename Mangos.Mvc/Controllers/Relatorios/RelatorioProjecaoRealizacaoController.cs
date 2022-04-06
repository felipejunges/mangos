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
    public class RelatorioProjecaoRealizacaoController : BaseController
    {
        private readonly RelatorioProjecaoRealizacaoService _relatorioProjecaoRealizacaoService;

        public RelatorioProjecaoRealizacaoController(DataKeeperService dataKeeperService, IUserResolverService userResolverService, RelatorioProjecaoRealizacaoService relatorioProjecaoRealizacaoService) : base(dataKeeperService, userResolverService)
        {
            _relatorioProjecaoRealizacaoService = relatorioProjecaoRealizacaoService;
        }

        public async Task<IActionResult> Index()
        {
            var mesInicial = DateTime.Now.Date.AddDays((DateTime.Now.Day - 1) * -1).AddMonths(-5);
            var mesFinal = DateTime.Now.Date.AddDays((DateTime.Now.Day - 1) * -1).AddMonths(6);

            ViewData["MesInicial"] = mesInicial.ToString("MM/yyyy");
            ViewData["MesFinal"] = mesFinal.ToString("MM/yyyy");

            var relatorio = await _relatorioProjecaoRealizacaoService.ListarRelatorioProjecaoRealizacaoAsync(_userResolverService.GrupoId, mesInicial, mesFinal);

            return View(relatorio);
        }

        public async Task<IActionResult> IndexLista(string mesInicial, string mesFinal)
        {
            DateTime mesInicialDate = Convert.ToDateTime("01/" + mesInicial);
            DateTime mesFinalDate = Convert.ToDateTime("01/" + mesFinal);

            var relatorio = await _relatorioProjecaoRealizacaoService.ListarRelatorioProjecaoRealizacaoAsync(_userResolverService.GrupoId, mesInicialDate, mesFinalDate);

            return PartialView(relatorio);
        }
    }
}