using Mangos.Dominio.Interfaces.Repository;
using Mangos.Dominio.Models;
using Mangos.Dominio.Services;
using Mangos.Dominio.Services.User;
using Mangos.Mvc.Models;
using Mangos.Mvc.Models.Mappers;
using Mangos.Mvc.Models.ViewModels;
using Mangos.Mvc.Models.ViewModels.ModelBuilders;
using Mangos.Mvc.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Mangos.Mvc.Controllers
{
    [Authorize(Roles = MangosClaimTypes.AdminRole)]
    public class GrupoController : BaseController
    {
        private readonly GrupoService _grupoService;
        private readonly IGrupoRepository _grupoRepository;
        private readonly GrupoDadosModelBuilder _modelBuilder;

        public GrupoController(DataKeeperService dataKeeperService, IUserResolverService userResolverService, GrupoService grupoService, IGrupoRepository grupoRepository, GrupoDadosModelBuilder modelBuilder) : base(dataKeeperService, userResolverService)
        {
            _grupoService = grupoService;
            _grupoRepository = grupoRepository;
            _modelBuilder = modelBuilder;
        }

        public async Task<IActionResult> Index()
        {
            var grupos = await _grupoRepository.ListarGruposAsync(string.Empty);

            return View(grupos);
        }

        public async Task<IActionResult> IndexLista(string? descricao = null)
        {
            var grupos = await _grupoRepository.ListarGruposAsync(descricao);

            return PartialView(grupos);
        }

        public async Task<IActionResult> Dados(int? id)
        {
            GrupoDadosModel? grupoDadosModel = await _modelBuilder.CriarModelPeloIdAsync(id);

            if (grupoDadosModel is null)
                return NotFound();

            return View(grupoDadosModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Dados(GrupoDadosModel grupoDadosModel)
        {
            if (ModelState.IsValid)
            {
                var grupo = await _modelBuilder.CriarEntidadeAsync(grupoDadosModel);

                if (grupo is null)
                    return BadRequest();

                await _grupoService.PersistirAsync(grupo);

                return OkResult();
            }

            //
            return View(grupoDadosModel);
        }

        public async Task<IActionResult> Detalhes(int id)
        {
            var grupo = await _grupoRepository.ObterGrupoComRelacionamentosAsync(id);

            if (grupo == null)
                return NotFound();

            var grupoDetalhesModel = GrupoMappers.ToGrupoDetalhes(grupo);

            return View(grupoDetalhesModel);
        }

        public async Task<IActionResult> Excluir(int id)
        {
            var grupo = await _grupoRepository.ObterGrupoAsync(id);

            if (grupo == null)
                return NotFound();

            if (id == _userResolverService.GrupoId)
                ModelState.AddModelError("", "O grupo logado não pode ser excluído");

            if (ModelState.IsValid)
            {
                await _grupoService.RemoverAsync(grupo);

                return RedirectToAction("Index");
            }

            //
            var grupoDetalhes = await _grupoRepository.ObterGrupoComRelacionamentosAsync(id);
            var grupoDetalhesModel = GrupoMappers.ToGrupoDetalhes(grupoDetalhes!);

            return View("Detalhes", grupoDetalhesModel);
        }
    }
}