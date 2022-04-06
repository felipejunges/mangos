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
    public class TransferenciaContaController : BaseController
    {
        private readonly TransferenciaContaService _transferenciaContaService;
        private readonly ITransferenciaContaRepository _transferenciaContaRepository;
        private readonly TransferenciaContaDadosModelBuilder _builder;

        public TransferenciaContaController(DataKeeperService dataKeeperService, IUserResolverService userResolverService, TransferenciaContaService transferenciaContaService, ITransferenciaContaRepository transferenciaContaRepository, TransferenciaContaDadosModelBuilder builder) : base(dataKeeperService, userResolverService)
        {
            _transferenciaContaService = transferenciaContaService;
            _transferenciaContaRepository = transferenciaContaRepository;
            _builder = builder;
        }

        public async Task<IActionResult> Index()
        {
            DateTime dataInicial = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime dataFinal = dataInicial.AddMonths(1).AddDays(-1);

            var transferenciasConta = await _transferenciaContaRepository.ListarTransferenciasContaAsync(_userResolverService.GrupoId, null, dataInicial, dataFinal);

            var model = new TransferenciaContaIndexModel(
                dataInicial: dataInicial,
                dataFinal: dataFinal,
                itens: TransferenciaContaMappers.ToListaModel(transferenciasConta)
            );

            return View(model);
        }

        public async Task<IActionResult> IndexLista(string tipoBusca, string descricao, string data, DateTime dataInicial, DateTime dataFinal)
        {
            if (tipoBusca == "S")
            {
                dataInicial = Convert.ToDateTime("01/" + data);
                dataFinal = Convert.ToDateTime("01/" + data).AddMonths(1).AddDays(-1);
            }

            var transferenciasConta = await _transferenciaContaRepository.ListarTransferenciasContaAsync(_userResolverService.GrupoId, descricao, dataInicial, dataFinal);

            var transferenciasContaModel = TransferenciaContaMappers.ToListaModel(transferenciasConta);

            return PartialView(transferenciasContaModel);
        }

        public async Task<IActionResult> Dados(int? id)
        {
            TransferenciaContaDadosModel? transferenciaContaDadosModel = await _builder.CriarModelPeloIdAsync(id);

            if (transferenciaContaDadosModel is null)
                return NotFound();

            return View(transferenciaContaDadosModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Dados(TransferenciaContaDadosModel transferenciaContaDadosModel)
        {
            if (ModelState.IsValid)
            {
                if (transferenciaContaDadosModel.GrupoId != _userResolverService.GrupoId)
                    return BadRequest();

                var transferenciaConta = await _builder.CriarEntidadeAsync(transferenciaContaDadosModel);

                if (transferenciaConta is null)
                    return BadRequest();

                await _transferenciaContaService.PersistirAsync(transferenciaConta);

                return OkResult();
            }

            //
            await _builder.AtualizarAsync(transferenciaContaDadosModel);

            return View(transferenciaContaDadosModel);
        }

        public async Task<IActionResult> Excluir(int id)
        {
            var transferenciaConta = await _transferenciaContaRepository.ObterTransferenciaContaAsync(id);

            if (transferenciaConta == null)
                return NotFound();

            if (transferenciaConta.GrupoId != _userResolverService.GrupoId)
                return BadRequest();

            try
            {
                await _transferenciaContaService.RemoverAsync(transferenciaConta);

                return SucessoResult("Transferência de conta excluída com sucesso.");
            }
            catch
            {
                return ErroResult("Erro ao excluir transferência de conta.");
            }
        }
    }
}