using Mangos.Dominio.Interfaces.Repository;
using Mangos.Dominio.Services;
using Mangos.Dominio.Services.User;
using Mangos.Mvc.Models;
using Mangos.Mvc.Models.ViewModels;
using Mangos.Mvc.Models.ViewModels.ModelBuilders;
using Mangos.Mvc.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace Mangos.Mvc.Controllers
{
    [Authorize(Roles = MangosClaimTypes.AdminRole)]
    public class UsuarioController : BaseController
    {
        private readonly UsuarioDadosModelBinder _modelBinder;
        private readonly UsuarioService _usuarioService;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IGrupoRepository _grupoRepository;

        public UsuarioController(DataKeeperService dataKeeperService, IUserResolverService userResolverService, UsuarioDadosModelBinder modelBinder, UsuarioService usuarioService, IUsuarioRepository usuarioRepository, IGrupoRepository grupoRepository) : base(dataKeeperService, userResolverService)
        {
            _modelBinder = modelBinder;
            _usuarioService = usuarioService;
            _usuarioRepository = usuarioRepository;
            _grupoRepository = grupoRepository;
        }

        public async Task<IActionResult> Index()
        {
            var usuarios = await _usuarioRepository.ListarUsuariosAsync(_userResolverService.GrupoId, string.Empty);

            ViewData["GruposLista"] = new SelectList(await _grupoRepository.ListarGruposAsync(string.Empty), "Id", "Descricao", _userResolverService.GrupoId);

            return View(usuarios);
        }

        public async Task<IActionResult> IndexLista(int? grupoId = null, string? nome = null)
        {
            var usuarios = await _usuarioRepository.ListarUsuariosAsync(grupoId, nome);

            return PartialView(usuarios);
        }

        public async Task<IActionResult> Dados(int? id)
        {
            UsuarioDadosModel? usuarioDadosModel = await _modelBinder.CriarModelPeloIdAsync(id);

            if (usuarioDadosModel is null)
                return NotFound();

            return View(usuarioDadosModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Dados(UsuarioDadosModel usuarioDadosModel)
        {
            if (ModelState.IsValid)
            {
                var usuario = await _modelBinder.CriarEntidadeAsync(usuarioDadosModel);

                if (usuario is null)
                    return BadRequest();

                await _usuarioService.PersistirAsync(usuario);

                return OkResult();
            }

            //
            await _modelBinder.AtualizarAsync(usuarioDadosModel);

            return View(usuarioDadosModel);
        }

        public async Task<IActionResult> Excluir(int id)
        {
            var usuario = await _usuarioRepository.ObterUsuarioAsync(id);

            if (usuario == null)
                return NotFound();

            if (usuario.Id == _userResolverService.UsuarioId)
                return ErroResult("O usuário logado não pode ser excluído.");

            try
            {
                await _usuarioService.RemoverAsync(usuario);

                return SucessoResult("Usuário excluído com sucesso.");
            }
            catch
            {
                return ErroResult("Erro ao excluir usuário.");
            }
        }
    }
}