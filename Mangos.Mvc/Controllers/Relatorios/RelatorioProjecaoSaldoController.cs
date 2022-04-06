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
    public class RelatorioProjecaoSaldoController : BaseController
    {
        private readonly RelatorioProjecaoSaldoService _relatorioProjecaoSaldoService;

        public RelatorioProjecaoSaldoController(DataKeeperService dataKeeperService, IUserResolverService userResolverService, RelatorioProjecaoSaldoService relatorioProjecaoSaldoService) : base(dataKeeperService, userResolverService)
        {
            _relatorioProjecaoSaldoService = relatorioProjecaoSaldoService;
        }

        public async Task<IActionResult> Index()
        {
            DateTime dataFinal = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));

            ViewData["DataFinal"] = dataFinal.ToString("dd/MM/yyyy");

            var relatorio = await _relatorioProjecaoSaldoService.ListarRelatorioProjecaoSaldoAsync(_userResolverService.GrupoId, dataFinal, 0);

            return View(relatorio);
        }

        public async Task<IActionResult> IndexLista(string dataFinal, decimal? valorInicial)
        {
            DateTime dataFinalDate = dataFinal != null ? Convert.ToDateTime(dataFinal) : new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));
            decimal valorInicialDecimal = valorInicial != null ? valorInicial.Value : 0M;

            ViewData["DataFinal"] = dataFinal;

            var relatorio = await _relatorioProjecaoSaldoService.ListarRelatorioProjecaoSaldoAsync(_userResolverService.GrupoId, dataFinalDate, valorInicialDecimal);

            return PartialView(relatorio);
        }
    }
}