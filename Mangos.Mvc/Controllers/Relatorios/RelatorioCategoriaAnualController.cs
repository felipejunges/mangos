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
    public class RelatorioCategoriaAnualController : BaseController
    {
        private readonly RelatorioCategoriaService _relatorioCategoriaService;

        public RelatorioCategoriaAnualController(DataKeeperService dataKeeperService, IUserResolverService userResolverService, RelatorioCategoriaService relatorioCategoriaService) : base(dataKeeperService, userResolverService)
        {
            _relatorioCategoriaService = relatorioCategoriaService;
        }

        public async Task<IActionResult> Index()
        {
            int anoInicial = DateTime.Now.Year - 9;
            int anoFinal = DateTime.Now.Year;

            ViewData["AnoInicial"] = anoInicial.ToString();
            ViewData["AnoFinal"] = anoFinal.ToString();
            ViewData["AnoInicialDate"] = new DateTime(anoInicial, 1, 1);
            ViewData["AnoFinalDate"] = new DateTime(anoFinal, 1, 1);

            var relatorio = await _relatorioCategoriaService.ListarRelatorioCategoriaAnualAsync(_userResolverService.GrupoId, anoInicial, anoFinal, "T", "R", "A", false);

            return View(relatorio);
        }

        public async Task<IActionResult> IndexLista(int anoInicial, int anoFinal, string situacao, string ordenacao, bool agruparSubcategorias)
        {
            ViewData["AnoInicial"] = anoInicial;
            ViewData["AnoFinal"] = anoFinal;
            ViewData["AnoInicialDate"] = new DateTime(anoInicial, 1, 1);
            ViewData["AnoFinalDate"] = new DateTime(anoFinal, 1, 1);

            var relatorio = await _relatorioCategoriaService.ListarRelatorioCategoriaAnualAsync(_userResolverService.GrupoId, anoInicial, anoFinal, "T", situacao, ordenacao, agruparSubcategorias);

            return PartialView(relatorio);
        }
    }
}