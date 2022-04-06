using Mangos.Dominio.Constants;
using Mangos.Dominio.Entities;
using Mangos.Dominio.Interfaces.Repository;
using Mangos.Dominio.Services.User;
using Mangos.Mvc.Models.ViewModels.ModelBuilders.Interfaces;
using System.Threading.Tasks;

namespace Mangos.Mvc.Models.ViewModels.ModelBuilders
{
    public class PessoaDadosModelBuilder : IModelBuilder<PessoaDadosModel, Pessoa>
    {
        private readonly IPessoaRepository _pessoaRepository;
        private readonly ICategoriaRepository _categoriaRepository;
        private readonly IUserResolverService _userResolverService;

        public PessoaDadosModelBuilder(IPessoaRepository pessoaRepository, ICategoriaRepository categoriaRepository, IUserResolverService userResolverService)
        {
            _pessoaRepository = pessoaRepository;
            _categoriaRepository = categoriaRepository;
            _userResolverService = userResolverService;
        }

        public async Task<Pessoa?> CriarEntidadeAsync(PessoaDadosModel pessoaDadosModel)
        {
            if (!pessoaDadosModel.CheckValidationHash())
                return null;

            Pessoa? pessoa;

            if (pessoaDadosModel.Id == 0)
                pessoa = new Pessoa() { Id = 0, GrupoId = pessoaDadosModel.GrupoId };
            else
                pessoa = await _pessoaRepository.ObterPessoaAsync(pessoaDadosModel.Id);

            if (pessoa is null)
                return null;

            pessoa.Nome = pessoaDadosModel.Nome!;
            pessoa.Telefone1 = pessoaDadosModel.Telefone1;
            pessoa.Telefone2 = pessoaDadosModel.Telefone2;
            pessoa.Telefone3 = pessoaDadosModel.Telefone3;
            pessoa.Site = pessoaDadosModel.Site;
            pessoa.Email = pessoaDadosModel.Email;
            pessoa.Tipo = pessoaDadosModel.Tipo!.Value;
            pessoa.CategoriaPadraoReceitaId = pessoaDadosModel.CategoriaPadraoReceitaId;
            pessoa.CategoriaPadraoDespesaId = pessoaDadosModel.CategoriaPadraoDespesaId;
            pessoa.Ativo = pessoaDadosModel.Ativo;

            return pessoa;
        }

        public async Task<PessoaDadosModel?> CriarModelPeloIdAsync(int? id)
        {
            if (id == null)
            {
                return PessoaDadosModel.Novo(
                    _userResolverService.GrupoId,
                    await _categoriaRepository.ListarCategoriasAsync(_userResolverService.GrupoId, string.Empty, TipoCategoriaPesquisa.Receita, false),
                    await _categoriaRepository.ListarCategoriasAsync(_userResolverService.GrupoId, string.Empty, TipoCategoriaPesquisa.Despesa, false)
                );
            }
            else
            {
                var pessoa = await _pessoaRepository.ObterPessoaAsync(id.Value);

                if (pessoa == null)
                    return null;

                if (pessoa.GrupoId != _userResolverService.GrupoId)
                    return null;

                return new PessoaDadosModel(
                    id: pessoa.Id,
                    grupoId: pessoa.GrupoId,
                    nome: pessoa.Nome,
                    telefone1: pessoa.Telefone1,
                    telefone2: pessoa.Telefone2,
                    telefone3: pessoa.Telefone3,
                    site: pessoa.Site,
                    email: pessoa.Email,
                    tipo: pessoa.Tipo,
                    categoriaPadraoReceitaId: pessoa.CategoriaPadraoReceitaId,
                    categoriaPadraoDespesaId: pessoa.CategoriaPadraoDespesaId,
                    ativo: pessoa.Ativo,
                    categoriasReceita: await _categoriaRepository.ListarCategoriasAsync(_userResolverService.GrupoId, string.Empty, TipoCategoriaPesquisa.Receita, false, pessoa.CategoriaPadraoReceitaId),
                    categoriasDespesa: await _categoriaRepository.ListarCategoriasAsync(_userResolverService.GrupoId, string.Empty, TipoCategoriaPesquisa.Despesa, false, pessoa.CategoriaPadraoDespesaId)
                );
            }
        }

        public async Task AtualizarAsync(PessoaDadosModel model)
        {
            model.AtualizarCategorias(
                categoriasReceita: await _categoriaRepository.ListarCategoriasAsync(_userResolverService.GrupoId, string.Empty, TipoCategoriaPesquisa.Receita, false, model.CategoriaPadraoReceitaId),
                categoriasDespesa: await _categoriaRepository.ListarCategoriasAsync(_userResolverService.GrupoId, string.Empty, TipoCategoriaPesquisa.Despesa, false, model.CategoriaPadraoDespesaId)
            );
        }
    }
}