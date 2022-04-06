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
    public class PessoaCoordenadaController : BaseController
    {
        private readonly PessoaCoordenadaDadosModelBinder _modelBinder;
        private readonly PessoaCoordenadaService _pessoaCoordenadaService;
        private readonly IPessoaCoordenadaRepository _pessoaCoordenadaRepository;

        public PessoaCoordenadaController(DataKeeperService dataKeeperService, IUserResolverService userResolverService, PessoaCoordenadaDadosModelBinder modelBinder, PessoaCoordenadaService pessoaCoordenadaService, IPessoaCoordenadaRepository pessoaCoordenadaRepository) : base(dataKeeperService, userResolverService)
        {
            _modelBinder = modelBinder;
            _pessoaCoordenadaService = pessoaCoordenadaService;
            _pessoaCoordenadaRepository = pessoaCoordenadaRepository;
        }

        public async Task<IActionResult> Index()
        {
            int pessoaId = Convert.ToInt32(Request.Query["Pessoa"]);

            var pessoaCoordenadas = await _pessoaCoordenadaRepository.ListarPessoasCoordenadasPorPessoaAsync(_userResolverService.GrupoId, pessoaId);

            ViewData["PessoaId"] = pessoaId;

            return View(pessoaCoordenadas);
        }

        public async Task<IActionResult> IndexLista(int id)
        {
            var pessoaCoordenadas = await _pessoaCoordenadaRepository.ListarPessoasCoordenadasPorPessoaAsync(_userResolverService.GrupoId, id);

            return PartialView(pessoaCoordenadas);
        }

        public async Task<IActionResult> Dados(int? id)
        {
            var pessoaCoordenadaDadosModel = await _modelBinder.CriarModelPeloIdAsync(id);

            if (pessoaCoordenadaDadosModel is null)
                return NotFound();

            return View(pessoaCoordenadaDadosModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Dados(PessoaCoordenadaDadosModel pessoaCoordenadaDadosModel)
        {
            if (ModelState.IsValid)
            {
                var pessoaCoordenada = await _modelBinder.CriarEntidadeAsync(pessoaCoordenadaDadosModel);

                if (pessoaCoordenada is null)
                    return BadRequest();

                await _pessoaCoordenadaService.PersistirAsync(pessoaCoordenada);

                return OkResult();
            }

            //
            return View(pessoaCoordenadaDadosModel);
        }

        public async Task<IActionResult> Excluir(int id)
        {
            var pessoaCoordenada = await _pessoaCoordenadaRepository.ObterPessoaCoordenadaAsync(id);

            if (pessoaCoordenada == null)
                return NotFound();

            if (pessoaCoordenada.Pessoa!.GrupoId != _userResolverService.GrupoId)
                return BadRequest();

            try
            {
                await _pessoaCoordenadaService.RemoverAsync(pessoaCoordenada);

                return SucessoResult("Coodenada excluída com sucesso.");
            }
            catch (Exception ex)
            {
                return ErroResult("Erro ao excluir a coordenada: " + ex.Message);
            }
        }
    }
}