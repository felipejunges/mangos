using Mangos.Dominio.Interfaces.Repository;
using Mangos.Dominio.Services;
using Mangos.Dominio.Services.User;
using Mangos.Mvc.Models.ViewModels;
using Mangos.Mvc.Models.ViewModels.ModelBuilders;
using Mangos.Mvc.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Mangos.Mvc.Controllers
{
    [Authorize]
    public class MetaSaldoController : BaseController
    {
        private readonly MetaSaldoDadosModelBinder _modelBinder;
        private readonly IMetaSaldoRepository _metaSaldoRepository;
        private readonly MetaSaldoService _metaSaldoService;

        public MetaSaldoController(DataKeeperService dataKeeperService, IUserResolverService userResolverService, MetaSaldoDadosModelBinder modelBinder, IMetaSaldoRepository metaSaldoRepository, MetaSaldoService metaSaldoService) : base(dataKeeperService, userResolverService)
        {
            _modelBinder = modelBinder;
            _metaSaldoRepository = metaSaldoRepository;
            _metaSaldoService = metaSaldoService;
        }

        public async Task<IActionResult> Index()
        {
            DateTime mesInicial = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime mesFinal = mesInicial.AddMonths(11);

            ViewData["MesInicial"] = mesInicial.ToString("MM/yyyy");
            ViewData["MesFinal"] = mesFinal.ToString("MM/yyyy");

            var metasMovimentacao = await _metaSaldoRepository.ListarMetasSaldoAsync(_userResolverService.GrupoId, mesInicial, mesFinal);

            return View(metasMovimentacao);
        }

        public async Task<IActionResult> IndexLista(string mesInicial, string mesFinal)
        {
            DateTime mesInicialDate = Convert.ToDateTime("01/" + mesInicial);
            DateTime mesFinalDate = Convert.ToDateTime("01/" + mesFinal);

            var metasMovimentacao = await _metaSaldoRepository.ListarMetasSaldoAsync(_userResolverService.GrupoId, mesInicialDate, mesFinalDate);

            return PartialView(metasMovimentacao);
        }

        public async Task<IActionResult> Dados(int? id)
        {
            var metaSaldoDadosModel = await _modelBinder.CriarModelPeloIdAsync(id);

            if (metaSaldoDadosModel is null)
                return NotFound();

            return View(metaSaldoDadosModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Dados(MetaSaldoDadosModel metaSaldoDadosModel)
        {
            if (ModelState.IsValid)
            {
                var metaSaldo = await _modelBinder.CriarEntidadeAsync(metaSaldoDadosModel);

                if (metaSaldo is null)
                    return BadRequest();

                await _metaSaldoService.PersistirAsync(metaSaldo);

                return OkResult();
            }

            //
            return View(metaSaldoDadosModel);
        }

        public async Task<IActionResult> Excluir(int id)
        {
            var metaSaldo = await _metaSaldoRepository.ObterMetaSaldoAsync(id);

            if (metaSaldo == null)
                return NotFound();

            if (metaSaldo.GrupoId != _userResolverService.GrupoId)
                return BadRequest();

            try
            {
                await _metaSaldoService.RemoverAsync(metaSaldo);

                return SucessoResult("Meta excluída com sucesso.");
            }
            catch
            {
                return ErroResult("Erro ao excluir a meta.");
            }
        }
    }
}