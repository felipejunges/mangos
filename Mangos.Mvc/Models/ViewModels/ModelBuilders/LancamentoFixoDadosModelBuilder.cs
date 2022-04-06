using Mangos.Dominio.Constants;
using Mangos.Dominio.Entities;
using Mangos.Dominio.Interfaces.Repository;
using Mangos.Dominio.Services.User;
using Mangos.Mvc.Models.ViewModels.ModelBuilders.Interfaces;
using System.Threading.Tasks;

namespace Mangos.Mvc.Models.ViewModels.ModelBuilders
{
    public class LancamentoFixoDadosModelBuilder : IModelBuilder<LancamentoFixoDadosModel, LancamentoFixo>
    {
        private readonly ILancamentoFixoRepository _lancamentoFixoRepository;
        private readonly ICategoriaRepository _categoriaRepository;
        private readonly ICartaoCreditoRepository _cartaoCreditoRepository;
        private readonly IUserResolverService _userResolverService;

        public LancamentoFixoDadosModelBuilder(ILancamentoFixoRepository lancamentoFixoRepository, ICategoriaRepository categoriaRepository, ICartaoCreditoRepository cartaoCreditoRepository, IUserResolverService userResolverService)
        {
            _lancamentoFixoRepository = lancamentoFixoRepository;
            _categoriaRepository = categoriaRepository;
            _cartaoCreditoRepository = cartaoCreditoRepository;
            _userResolverService = userResolverService;
        }

        public async Task<LancamentoFixo?> CriarEntidadeAsync(LancamentoFixoDadosModel lancamentoFixoDadosModel)
        {
            if (!lancamentoFixoDadosModel.CheckValidationHash())
                return null;

            LancamentoFixo? lancamentoFixo;

            if (lancamentoFixoDadosModel.Id == 0)
                lancamentoFixo = new LancamentoFixo() { Id = 0, GrupoId = lancamentoFixoDadosModel.GrupoId };
            else
                lancamentoFixo = await _lancamentoFixoRepository.ObterLancamentoFixoAsync(lancamentoFixoDadosModel.Id);

            if (lancamentoFixo is null)
                return null;

            lancamentoFixo.Tipo = lancamentoFixoDadosModel.Tipo!.Value;
            lancamentoFixo.Periodicidade = lancamentoFixoDadosModel.Periodicidade!.Value;
            lancamentoFixo.Valor = lancamentoFixoDadosModel.Valor;
            lancamentoFixo.DiaVencimento = lancamentoFixoDadosModel.DiaVencimento!.Value;
            lancamentoFixo.MesVencimento = lancamentoFixoDadosModel.MesVencimento;
            lancamentoFixo.CartaoCreditoId = lancamentoFixoDadosModel.CartaoCreditoId;
            lancamentoFixo.Descricao = lancamentoFixoDadosModel.Descricao!;
            lancamentoFixo.PessoaId = lancamentoFixoDadosModel.PessoaId;
            lancamentoFixo.CategoriaId = lancamentoFixoDadosModel.CategoriaId;
            lancamentoFixo.DataUltimoMesGerado = lancamentoFixoDadosModel.DataUltimoMesGerado;
            lancamentoFixo.Ativo = lancamentoFixoDadosModel.Ativo;

            return lancamentoFixo;
        }

        public async Task<LancamentoFixoDadosModel?> CriarModelPeloIdAsync(int? id)
        {
            if (id == null)
            {
                return LancamentoFixoDadosModel.Novo(
                    grupoId: _userResolverService.GrupoId,
                    cartoesCredito: await _cartaoCreditoRepository.ListarCartoesCreditoAsync(_userResolverService.GrupoId, string.Empty, false),
                    categorias: await _categoriaRepository.ListarCategoriasAsync(_userResolverService.GrupoId, string.Empty, TipoCategoriaPesquisa.Todos, false)
                );
            }
            else
            {
                var lancamentoFixo = await _lancamentoFixoRepository.ObterLancamentoFixoAsync(id.Value);

                if (lancamentoFixo == null)
                    return null;

                if (lancamentoFixo.GrupoId != _userResolverService.GrupoId)
                    return null;

                return new LancamentoFixoDadosModel(
                    id: lancamentoFixo.Id,
                    grupoId: lancamentoFixo.GrupoId,
                    tipo: lancamentoFixo.Tipo,
                    periodicidade: lancamentoFixo.Periodicidade,
                    valor: lancamentoFixo.Valor,
                    diaVencimento: lancamentoFixo.DiaVencimento,
                    mesVencimento: lancamentoFixo.MesVencimento,
                    cartaoCreditoId: lancamentoFixo.CartaoCreditoId,
                    descricao: lancamentoFixo.Descricao,
                    pessoaId: lancamentoFixo.PessoaId,
                    pessoa: lancamentoFixo.Pessoa?.Nome,
                    categoriaId: lancamentoFixo.CategoriaId,
                    dataUltimoMesGerado: lancamentoFixo.DataUltimoMesGerado,
                    ativo: lancamentoFixo.Ativo,
                    cartoesCredito: await _cartaoCreditoRepository.ListarCartoesCreditoAsync(_userResolverService.GrupoId, string.Empty, false, lancamentoFixo.CartaoCreditoId),
                    categorias: await _categoriaRepository.ListarCategoriasAsync(_userResolverService.GrupoId, string.Empty, TipoCategoriaPesquisa.Todos, false, lancamentoFixo.CategoriaId)
                );
            }
        }

        public async Task AtualizarAsync(LancamentoFixoDadosModel model)
        {
            model.AtualizarCartoesCreditoCategorias(
                cartoesCredito: await _cartaoCreditoRepository.ListarCartoesCreditoAsync(_userResolverService.GrupoId, string.Empty, false, model.CartaoCreditoId),
                categorias: await _categoriaRepository.ListarCategoriasAsync(_userResolverService.GrupoId, string.Empty, TipoCategoriaPesquisa.Todos, false, model.CategoriaId)
            );
        }
    }
}