using Mangos.Dominio.Interfaces.Repository;
using Mangos.Dominio.Services;
using Mangos.Dominio.Services.User;
using Mangos.Mvc.Models.Mappers;
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
    public class RendimentoMensalContaController : BaseController
    {
        private readonly RendimentoMensalContaDadosModelBinder _modelBinder;
        private readonly RendimentoMensalContaService _rendimentoMensalContaService;
        private readonly IRendimentoMensalContaRepository _rendimentoMensalContaRepository;
        private readonly IContaBancariaRepository _contaBancariaRepository;

        public RendimentoMensalContaController(DataKeeperService dataKeeperService, IUserResolverService userResolverService, RendimentoMensalContaDadosModelBinder modelBinder, RendimentoMensalContaService rendimentoMensalContaService, IRendimentoMensalContaRepository rendimentoMensalContaRepository, IContaBancariaRepository contaBancariaRepository) : base(dataKeeperService, userResolverService)
        {
            _modelBinder = modelBinder;
            _rendimentoMensalContaService = rendimentoMensalContaService;
            _rendimentoMensalContaRepository = rendimentoMensalContaRepository;
            _contaBancariaRepository = contaBancariaRepository;
        }

        public async Task<IActionResult> Index()
        {
            DateTime mesReferenciaInicial = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            var contasBancarias = await _contaBancariaRepository.ListaContasBancariasAsync(_userResolverService.GrupoId, string.Empty, false);

            var rendimentosMensaisConta = await _rendimentoMensalContaRepository.ListarRendimentosMensaisContaAsync(_userResolverService.GrupoId, null, mesReferenciaInicial, mesReferenciaInicial);

            var model = new RendimentoMensalContaIndexModel(
                mesReferencia: new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1),
                contasBancarias: contasBancarias,
                itens: RendimentoMensalContaMappers.ToListaModel(rendimentosMensaisConta)
            );

            return View(model);
        }

        public async Task<IActionResult> IndexLista(string tipoBusca, int? contaBancariaId, string mesReferencia, string mesReferenciaInicial, string mesReferenciaFinal)
        {
            DateTime mesReferenciaInicialDate;
            DateTime mesReferenciaFinalDate;

            if (tipoBusca == "S")
            {
                mesReferenciaInicialDate = mesReferencia != null ? Convert.ToDateTime("01/" + mesReferencia) : DateTime.Now;
                mesReferenciaFinalDate = mesReferencia != null ? (Convert.ToDateTime("01/" + mesReferencia).AddMonths(1).AddDays(-1)) : DateTime.Now;
            }
            else
            {
                mesReferenciaInicialDate = mesReferenciaInicial != null ? Convert.ToDateTime("01/" + mesReferenciaInicial) : DateTime.Now;
                mesReferenciaFinalDate = mesReferenciaFinal != null ? Convert.ToDateTime("01/" + mesReferenciaFinal) : DateTime.Now;
            }

            var rendimentosMensaisConta = await _rendimentoMensalContaRepository.ListarRendimentosMensaisContaAsync(_userResolverService.GrupoId, contaBancariaId, mesReferenciaInicialDate, mesReferenciaFinalDate);

            var rendimentosMensaisContaModel = RendimentoMensalContaMappers.ToListaModel(rendimentosMensaisConta);

            return PartialView(rendimentosMensaisContaModel);
        }

        public async Task<IActionResult> Dados(int? id)
        {
            RendimentoMensalContaDadosModel? rendimentoMensalContaModel = await _modelBinder.CriarModelPeloIdAsync(id);

            if (rendimentoMensalContaModel is null)
                return NotFound();

            return View(rendimentoMensalContaModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Dados(RendimentoMensalContaDadosModel rendimentoMensalContaDadosModel)
        {
            if (ModelState.IsValid)
            {
                var rendimentoMensalConta = await _modelBinder.CriarEntidadeAsync(rendimentoMensalContaDadosModel);

                if (rendimentoMensalConta is null)
                    return NotFound();

                await _rendimentoMensalContaService.PersistirAsync(rendimentoMensalConta);

                return OkResult();
            }

            //
            await _modelBinder.AtualizarAsync(rendimentoMensalContaDadosModel);

            return View(rendimentoMensalContaDadosModel);
        }

        public async Task<IActionResult> Excluir(int id)
        {
            var rendimentoMensalConta = await _rendimentoMensalContaRepository.ObterRendimentoMensalContaAsync(id);

            if (rendimentoMensalConta == null)
                return NotFound();

            if (rendimentoMensalConta.ContaBancaria!.GrupoId != _userResolverService.GrupoId)
                return BadRequest();

            try
            {
                await _rendimentoMensalContaService.RemoverAsync(rendimentoMensalConta);

                return SucessoResult("Rendimento mensal de conta excluída com sucesso.");
            }
            catch
            {
                return ErroResult("Erro ao excluir rendimento mensal de conta.");
            }
        }
    }
}