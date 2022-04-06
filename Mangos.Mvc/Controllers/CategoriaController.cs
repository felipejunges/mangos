using Mangos.Dominio.Constants;
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
    public class CategoriaController : BaseController
    {
        private readonly CategoriaService _categoriaService;
        private readonly ICategoriaRepository _categoriaRepository;
        private readonly CategoriaDadosModelBuilder _modelBuilder;

        public CategoriaController(DataKeeperService dataKeeperService, IUserResolverService userResolverService, CategoriaService categoriaService, ICategoriaRepository categoriaRepository, CategoriaDadosModelBuilder modelBuilder) : base(dataKeeperService, userResolverService)
        {
            _categoriaService = categoriaService;
            _categoriaRepository = categoriaRepository;
            _modelBuilder = modelBuilder;
        }

        public async Task<IActionResult> Index()
        {
            var categorias = await _categoriaRepository.ListarCategoriasAsync(_userResolverService.GrupoId, string.Empty, TipoCategoriaPesquisa.Todos, false);

            var categoriasModel = CategoriaMappers.ToListaModel(categorias);

            return View(categoriasModel);
        }

        public async Task<IActionResult> IndexLista(string descricao, bool buscarInativos)
        {
            var categorias = await _categoriaRepository.ListarCategoriasAsync(_userResolverService.GrupoId, descricao, TipoCategoriaPesquisa.Todos, buscarInativos);

            var categoriasModel = CategoriaMappers.ToListaModel(categorias);

            return PartialView(categoriasModel);
        }

        public async Task<IActionResult> Dados(int? id)
        {
            CategoriaDadosModel? categoriaDadosModel = await _modelBuilder.CriarModelPeloIdAsync(id);

            if (categoriaDadosModel == null)
                return NotFound();

            return View(categoriaDadosModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Dados(CategoriaDadosModel categoriaDadosModel)
        {
            if (ModelState.IsValid)
            {
                Categoria? categoria = await _modelBuilder.CriarEntidadeAsync(categoriaDadosModel);

                if (categoria is null)
                    return BadRequest();

                await _categoriaService.PersistirAsync(categoria);

                return OkResult();
            }

            //
            await _modelBuilder.AtualizarAsync(categoriaDadosModel);

            return View(categoriaDadosModel);
        }

        public async Task<IActionResult> Excluir(int id)
        {
            var categoria = await _categoriaRepository.ObterCartaoCreditoAsync(id);

            if (categoria == null)
                return NotFound();

            if (categoria.GrupoId != _userResolverService.GrupoId)
                return BadRequest();

            try
            {
                await _categoriaService.RemoverAsync(categoria);

                return SucessoResult("Categoria excluída com sucesso.");
            }
            catch
            {
                return ErroResult("Erro ao excluir a categoria.");
            }
        }
    }
}