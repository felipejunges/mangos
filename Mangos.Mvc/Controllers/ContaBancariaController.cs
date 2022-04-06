using Mangos.Dominio.Entities;
using Mangos.Dominio.Interfaces.Repository;
using Mangos.Dominio.Services;
using Mangos.Dominio.Services.User;
using Mangos.Mvc.Models.Mappers;
using Mangos.Mvc.Models.ViewModels;
using Mangos.Mvc.Models.ViewModels.ModelBuilders;
using Mangos.Mvc.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Mangos.Mvc.Controllers
{
    [Authorize]
    public class ContaBancariaController : BaseController
    {
        private readonly IContaBancariaRepository _contaBancariaRepository;
        private readonly ContaBancariaService _contaBancariaService;
        private readonly ContaBancariaDadosModelBuilder _modelBuilder;

        public ContaBancariaController(DataKeeperService dataKeeperService, IContaBancariaRepository contaBancariaRepository, IUserResolverService userResolverService, ContaBancariaService contaBancariaService, ContaBancariaDadosModelBuilder modelBuilder) : base(dataKeeperService, userResolverService)
        {
            _contaBancariaRepository = contaBancariaRepository;
            _contaBancariaService = contaBancariaService;
            _modelBuilder = modelBuilder;
        }

        public async Task<IActionResult> Index()
        {
            var contasBancarias = await _contaBancariaRepository.ListaContasBancariasAsync(_userResolverService.GrupoId, string.Empty, false);

            var contasBancariasModel = ContaBancariaMappers.ToListaModel(contasBancarias);

            return View(contasBancariasModel);
        }

        public async Task<IActionResult> IndexLista(string? descricao = null, bool buscarInativos = false)
        {
            var contasBancarias = await _contaBancariaRepository.ListaContasBancariasAsync(_userResolverService.GrupoId, descricao, buscarInativos);

            var contasBancariasModel = ContaBancariaMappers.ToListaModel(contasBancarias);

            return PartialView(contasBancariasModel);
        }

        public async Task<IActionResult> Dados(int? id)
        {
            ContaBancariaDadosModel? contaBancariaDadosModel = await _modelBuilder.CriarModelPeloIdAsync(id);

            if (contaBancariaDadosModel == null)
                return NotFound();

            return View(contaBancariaDadosModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Dados(ContaBancariaDadosModel contaBancariaDadosModel)
        {
            if (ModelState.IsValid)
            {
                ContaBancaria? contaBancaria = await _modelBuilder.CriarEntidadeAsync(contaBancariaDadosModel);

                if (contaBancaria is null)
                    return BadRequest();

                await _contaBancariaService.PersistirAsync(contaBancaria);

                return OkResult();
            }

            //
            await _modelBuilder.AtualizarAsync(contaBancariaDadosModel);

            return View(contaBancariaDadosModel);
        }

        public async Task<IActionResult> Excluir(int id)
        {
            var contaBancaria = await _contaBancariaRepository.ObterContaBancariaAsync(id);

            if (contaBancaria == null)
                return NotFound();

            if (contaBancaria.GrupoId != _userResolverService.GrupoId)
                return BadRequest();

            try
            {
                await _contaBancariaService.RemoverAsync(contaBancaria);

                return SucessoResult("Conta bancária excluída com sucesso.");
            }
            catch
            {
                return ErroResult("Erro ao excluir a conta bancária.");
            }
        }
    }
}