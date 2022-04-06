using Mangos.Dominio.Interfaces.Repository;
using Mangos.Dominio.Services.Relatorios;
using Mangos.Dominio.Services.User;
using Mangos.Mvc.Models.Mappers;
using Mangos.Mvc.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Threading.Tasks;

namespace Mangos.Mvc.Controllers.Relatorios
{
    [Authorize]
    public class RelatorioCartaoCreditoController : BaseController
    {
        private readonly RelatorioCartaoCreditoService _relatorioCartaoCreditoService;
        private readonly ILancamentoCartaoRepository _lancamentoCartaoRepository;
        private readonly ICartaoCreditoRepository _cartaoCreditoRepository;

        public RelatorioCartaoCreditoController(DataKeeperService dataKeeperService, IUserResolverService userResolverService, RelatorioCartaoCreditoService relatorioCartaoCreditoService, ILancamentoCartaoRepository lancamentoCartaoRepository, ICartaoCreditoRepository cartaoCreditoRepository) : base(dataKeeperService, userResolverService)
        {
            _relatorioCartaoCreditoService = relatorioCartaoCreditoService;
            _lancamentoCartaoRepository = lancamentoCartaoRepository;
            _cartaoCreditoRepository = cartaoCreditoRepository;
        }

        public async Task<IActionResult> Index()
        {
            var (dataMinima, dataMaxima) = await _lancamentoCartaoRepository.ObterDatasMenorMaiorVencimentosNaoFechadosAsync(_userResolverService.GrupoId);

            DateTime mesInicial = dataMinima ?? new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime mesFinal = dataMaxima ?? new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            bool agruparCartoes = true;

            ViewData["MesInicial"] = mesInicial.ToString("MM/yyyy");
            ViewData["MesFinal"] = mesFinal.ToString("MM/yyyy");
            ViewData["AgruparCartoes"] = agruparCartoes;

            var relatorio = await _relatorioCartaoCreditoService.ObterRelatorioCartoesCreditoAsync(_userResolverService.GrupoId, null, mesInicial, mesFinal, agruparCartoes);

            var cartoesCredito = await _cartaoCreditoRepository.ListarCartoesCreditoAsync(_userResolverService.GrupoId, string.Empty, false);

            ViewData["CartoesCreditoLista"] = new SelectList(CartaoCreditoMappers.ToListaModel(cartoesCredito), "Id", "Descricao", null);

            return View(relatorio);
        }

        public async Task<IActionResult> IndexLista(int? cartaoCreditoId, string mesInicial, string mesFinal, bool agruparCartoes = false)
        {
            DateTime? mesInicialDate = mesInicial != null ? (DateTime?)Convert.ToDateTime("01/" + mesInicial) : null;
            DateTime? mesFinalDate = mesFinal != null ? (DateTime?)Convert.ToDateTime("01/" + mesFinal) : null;

            ViewData["AgruparCartoes"] = agruparCartoes;

            var relatorio = await _relatorioCartaoCreditoService.ObterRelatorioCartoesCreditoAsync(_userResolverService.GrupoId, cartaoCreditoId, mesInicialDate, mesFinalDate, agruparCartoes);

            return PartialView(relatorio);
        }
    }
}