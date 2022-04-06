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
    public class MetaMovimentacaoController : BaseController
    {
        private readonly MetaMovimentacaoService _metaMovimentacaoService;
        private readonly IMetaMovimentacaoRepository _metaMovimentacaoRepository;
        private readonly MetaMovimentacaoDadosModelBinder _modelBinder;

        public MetaMovimentacaoController(DataKeeperService dataKeeperService, IUserResolverService userResolverService, MetaMovimentacaoService metaMovimentacaoService, IMetaMovimentacaoRepository metaMovimentacaoRepository, MetaMovimentacaoDadosModelBinder modelBinder) : base(dataKeeperService, userResolverService)
        {
            _metaMovimentacaoService = metaMovimentacaoService;
            _metaMovimentacaoRepository = metaMovimentacaoRepository;
            _modelBinder = modelBinder;
        }

        public async Task<IActionResult> Index()
        {
            var mes = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            ViewData["MesInicial"] = mes.ToString("MM/yyyy");
            ViewData["MesFinal"] = mes.ToString("MM/yyyy");

            var metasMovimentacao = await _metaMovimentacaoRepository.ListarMetasMovimentacaoAsync(_userResolverService.GrupoId, mes, mes);

            return View(metasMovimentacao);
        }

        public async Task<IActionResult> IndexLista(string mesInicial, string mesFinal)
        {
            DateTime mesInicialDate = Convert.ToDateTime("01/" + mesInicial);
            DateTime mesFinalDate = Convert.ToDateTime("01/" + mesFinal);

            var metasMovimentacao = await _metaMovimentacaoRepository.ListarMetasMovimentacaoAsync(_userResolverService.GrupoId, mesInicialDate, mesFinalDate);

            return PartialView(metasMovimentacao);
        }

        public async Task<IActionResult> Dados(int? id)
        {
            MetaMovimentacaoDadosModel? metaMovimentacaoDadosModel = await _modelBinder.CriarModelPeloIdAsync(id);

            if (metaMovimentacaoDadosModel is null)
                return NotFound();

            return View(metaMovimentacaoDadosModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Dados(MetaMovimentacaoDadosModel metaMovimentacaoDadosModel)
        {
            if (ModelState.IsValid)
            {
                var metaMovientacao = await _modelBinder.CriarEntidadeAsync(metaMovimentacaoDadosModel);

                if (metaMovientacao is null)
                    return BadRequest();

                await _metaMovimentacaoService.PersistirAsync(metaMovientacao);

                return OkResult();
            }

            //
            return View(metaMovimentacaoDadosModel);
        }

        public async Task<IActionResult> Burnup(int id)
        {
            var relatorio = await _metaMovimentacaoService.GetRelatorioBurnup(id, _userResolverService.GrupoId);

            return View(relatorio);
        }

        public async Task<IActionResult> Excluir(int id)
        {
            var metaMovimentacao = await _metaMovimentacaoRepository.ObterMetaMovimentacaoAsync(id);

            if (metaMovimentacao == null)
                return NotFound();

            if (metaMovimentacao.GrupoId != _userResolverService.GrupoId)
                return BadRequest();

            try
            {
                await _metaMovimentacaoService.RemoverAsync(metaMovimentacao);

                return SucessoResult("Meta excluída com sucesso.");
            }
            catch
            {
                return ErroResult("Erro ao excluir a meta.");
            }
        }
    }
}