using Mangos.Dominio.Interfaces.Repository;
using Mangos.Dominio.Services;
using Mangos.Dominio.Services.User;
using Mangos.Mvc.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Threading.Tasks;
using Mangos.Mvc.Models.Mappers;

namespace Mangos.Mvc.Controllers
{
    [Authorize]
    public class SaldoContaController : BaseController
    {
        private readonly ISaldoContaBancariaRepository _saldoContaBancariaRepository;
        private readonly SaldoContaBancariaService _saldoContaBancariaService;
        private readonly IContaBancariaRepository _contaBancariaRepository;

        public SaldoContaController(DataKeeperService dataKeeperService, IUserResolverService userResolverService, ISaldoContaBancariaRepository saldoContaBancariaRepository, SaldoContaBancariaService saldoContaBancariaService, IContaBancariaRepository contaBancariaRepository) : base(dataKeeperService, userResolverService)
        {
            _saldoContaBancariaRepository = saldoContaBancariaRepository;
            _saldoContaBancariaService = saldoContaBancariaService;
            _contaBancariaRepository = contaBancariaRepository;
        }

        public async Task<IActionResult> Index()
        {
            DateTime dataInicial = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime dataFinal = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            ViewData["MesInicial"] = dataInicial.ToString("MM/yyyy");
            ViewData["MesFinal"] = dataFinal.ToString("MM/yyyy");

            ViewData["ContasBancariasLista"] = new SelectList(await _contaBancariaRepository.ListaContasBancariasAsync(_userResolverService.GrupoId, string.Empty, false), "Id", "Descricao", null);

            var saldosContasBancarias = await _saldoContaBancariaRepository.ListarSaldosContasBancariasAsync(_userResolverService.GrupoId, null, dataInicial, dataFinal);
            var saldosContasBancariaModel = SaldoContaBancariaMappers.ToListaModel(saldosContasBancarias);

            return View(saldosContasBancariaModel);
        }

        public async Task<IActionResult> IndexLista(int? contaBancariaId, string mesInicial, string mesFinal)
        {
            DateTime? dataInicial = mesInicial != null ? Convert.ToDateTime("01/" + mesInicial) : null;
            DateTime? dataFinal = mesFinal != null ? Convert.ToDateTime("01/" + mesFinal) : null;

            var saldosContasBancarias = await _saldoContaBancariaRepository.ListarSaldosContasBancariasAsync(_userResolverService.GrupoId, contaBancariaId, dataInicial, dataFinal);
            var saldosContasBancariaModel = SaldoContaBancariaMappers.ToListaModel(saldosContasBancarias);

            return PartialView(saldosContasBancariaModel);
        }

        public async Task<IActionResult> GerarSaldos(int? contaBancariaId, string mesInicial)
        {
            if (mesInicial != null)
            {
                var mesInicialDate = Convert.ToDateTime("01/" + mesInicial);

                await _saldoContaBancariaService.Gerar(_userResolverService.GrupoId, contaBancariaId, mesInicialDate);

                return SucessoResult("Saldos gerados com sucesso.");
            }
            else
            {
                return ErroResult("Período inicial inválido.");
            }
        }

        [AllowAnonymous]
        public async Task<IActionResult> GerarTodos()
        {
            await _saldoContaBancariaService.GerarTodosMesAtual();

            return OkResult();
        }

        public async Task<IActionResult> SetarDataHoraConferenciaSaldo(int id)
        {
            await _saldoContaBancariaService.SetarDataHoraConferenciaSaldo(id);

            return SucessoResult($"Conferência do registro atualizado.");
        }

        public async Task<IActionResult> Fechar(int id)
        {
            var resultFechamento = await _saldoContaBancariaService.Fechar(id);

            if (resultFechamento.IsValid)
                return SucessoResult($"Registro fechado com sucesso.");
            else
                return ErroResult(resultFechamento.FirstMessage);
        }

        public async Task<IActionResult> Reabrir(int id)
        {
            var resultReabertura = await _saldoContaBancariaService.ReabrirFechamento(id);

            if (resultReabertura.IsValid)
                return SucessoResult($"Registro reaberto com sucesso.");
            else
                return ErroResult(resultReabertura.FirstMessage);
        }
    }
}