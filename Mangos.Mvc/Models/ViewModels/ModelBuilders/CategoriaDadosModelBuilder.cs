using Mangos.Dominio.Entities;
using Mangos.Dominio.Interfaces.Repository;
using Mangos.Dominio.Services.User;
using Mangos.Mvc.Models.ViewModels.ModelBuilders.Interfaces;
using System.Threading.Tasks;

namespace Mangos.Mvc.Models.ViewModels.ModelBuilders
{
    public class CategoriaDadosModelBuilder : IModelBuilder<CategoriaDadosModel, Categoria>
    {
        private readonly ICategoriaRepository _categoriaRepository;
        private readonly IUserResolverService _userResolverService;

        public CategoriaDadosModelBuilder(ICategoriaRepository categoriaRepository, IUserResolverService userResolverService)
        {
            _categoriaRepository = categoriaRepository;
            _userResolverService = userResolverService;
        }

        public async Task<Categoria?> CriarEntidadeAsync(CategoriaDadosModel categoriaDadosModel)
        {
            if (!categoriaDadosModel.CheckValidationHash())
                return null;

            Categoria? categoria;

            if (categoriaDadosModel.Id == 0)
                categoria = new Categoria() { Id = 0, GrupoId = categoriaDadosModel.GrupoId };
            else
                categoria = await _categoriaRepository.ObterCartaoCreditoAsync(categoriaDadosModel.Id);

            if (categoria is null)
                return null;

            if (categoria.GrupoId != _userResolverService.GrupoId)
                return null;

            categoria.CategoriaSuperiorId = categoriaDadosModel.CategoriaSuperiorId;
            categoria.Descricao = categoriaDadosModel.Descricao!;
            categoria.Receita = categoriaDadosModel.Receita;
            categoria.Despesa = categoriaDadosModel.Despesa;
            categoria.RelatorioTotal = categoriaDadosModel.RelatorioTotal;
            categoria.Ativo = categoriaDadosModel.Ativo;

            return categoria;
        }

        public async Task<CategoriaDadosModel?> CriarModelPeloIdAsync(int? id)
        {
            if (id == null)
            {
                return CategoriaDadosModel.Novo(
                    _userResolverService.GrupoId,
                    await _categoriaRepository.ListarCategoriasSuperioresAsync(_userResolverService.GrupoId, false)
                );
            }
            else
            {
                var categoria = await _categoriaRepository.ObterCartaoCreditoAsync(id.Value);

                if (categoria == null)
                    return null;

                if (categoria.GrupoId != _userResolverService.GrupoId)
                    return null;

                return new CategoriaDadosModel(
                    id: categoria.Id,
                    grupoId: categoria.GrupoId,
                    categoriaSuperiorId: categoria.CategoriaSuperiorId,
                    descricao: categoria.Descricao,
                    receita: categoria.Receita,
                    despesa: categoria.Despesa,
                    relatorioTotal: categoria.RelatorioTotal,
                    ativo: categoria.Ativo,
                    categoriaTemFilhas: await _categoriaRepository.ValidarCategoriasTemFilhasAsync(categoria.Id),
                    categoriasSuperiores: await _categoriaRepository.ListarCategoriasSuperioresAsync(categoria.GrupoId, false, categoria.CategoriaSuperiorId)
                );
            }
        }

        public async Task AtualizarAsync(CategoriaDadosModel model)
        {
            model.AtualizarCategorias(
                categoriaTemFilhas: await _categoriaRepository.ValidarCategoriasTemFilhasAsync(model.Id),
                categoriasSuperiores: await _categoriaRepository.ListarCategoriasSuperioresAsync(model.GrupoId, false, model.CategoriaSuperiorId)
            );
        }
    }
}