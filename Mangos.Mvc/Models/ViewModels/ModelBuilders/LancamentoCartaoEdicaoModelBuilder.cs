using Mangos.Dominio.Constants;
using Mangos.Dominio.Entities;
using Mangos.Dominio.Interfaces.Repository;
using Mangos.Dominio.Services.User;
using Mangos.Mvc.Models.Mappers;
using Mangos.Mvc.Models.ViewModels.ModelBuilders.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mangos.Mvc.Models.ViewModels.ModelBuilders
{
    public class LancamentoCartaoEdicaoModelBuilder : IModelBuilder<LancamentoCartaoEdicaoModel, LancamentoCartao>
    {
        private readonly ILancamentoCartaoRepository _lancamentoCartaoRepository;
        private readonly ICategoriaRepository _categoriaRepository;
        private readonly ICartaoCreditoRepository _cartaoCreditoRepository;
        private readonly IUserResolverService _userResolverService;

        public LancamentoCartaoEdicaoModelBuilder(ILancamentoCartaoRepository lancamentoCartaoRepository, ICategoriaRepository categoriaRepository, ICartaoCreditoRepository cartaoCreditoRepository, IUserResolverService userResolverService)
        {
            _lancamentoCartaoRepository = lancamentoCartaoRepository;
            _categoriaRepository = categoriaRepository;
            _cartaoCreditoRepository = cartaoCreditoRepository;
            _userResolverService = userResolverService;
        }

        public async Task<LancamentoCartao?> CriarEntidadeAsync(LancamentoCartaoEdicaoModel lancamentoCartaoDadosModel)
        {
            var lancamentoCartao = await _lancamentoCartaoRepository.ObterLancamentoCartaoAsync(lancamentoCartaoDadosModel.Id);

            if (lancamentoCartao is null)
                return null;

            lancamentoCartao.CartaoCreditoId = lancamentoCartaoDadosModel.CartaoCreditoId;
            lancamentoCartao.TipoLancamento = lancamentoCartaoDadosModel.TipoLancamento;
            lancamentoCartao.Valor = lancamentoCartaoDadosModel.Valor;
            lancamentoCartao.MesReferencia = lancamentoCartaoDadosModel.MesReferencia;
            lancamentoCartao.Descricao = lancamentoCartaoDadosModel.Descricao!;
            lancamentoCartao.NumeroParcela = lancamentoCartaoDadosModel.NumeroParcela;
            lancamentoCartao.TotalParcelas = lancamentoCartaoDadosModel.TotalParcelas;
            lancamentoCartao.PessoaId = lancamentoCartaoDadosModel.PessoaId;
            lancamentoCartao.CategoriaId = lancamentoCartaoDadosModel.CategoriaId;
            lancamentoCartao.Observacoes = lancamentoCartaoDadosModel.Observacoes;

            return lancamentoCartao;
        }

        public async Task<LancamentoCartaoEdicaoModel?> CriarModelPeloIdAsync(int? id)
        {
            if (id is null)
                return null;

            var lancamentoCartao = await _lancamentoCartaoRepository.ObterLancamentoCartaoAsync(id.Value);

            if (lancamentoCartao == null)
                return null;

            if (lancamentoCartao.GrupoId != _userResolverService.GrupoId)
                return null;

            return new LancamentoCartaoEdicaoModel(
                id: lancamentoCartao.Id,
                grupoId: lancamentoCartao.GrupoId,
                cartaoCreditoId: lancamentoCartao.CartaoCreditoId,
                tipoLancamento: lancamentoCartao.TipoLancamento,
                valor: lancamentoCartao.Valor,
                mesReferencia: lancamentoCartao.MesReferencia,
                descricao: lancamentoCartao.Descricao!,
                numeroParcela: lancamentoCartao.NumeroParcela,
                totalParcelas: lancamentoCartao.TotalParcelas,
                pessoaId: lancamentoCartao.PessoaId,
                pessoa: lancamentoCartao.Pessoa?.Nome ?? string.Empty,
                categoriaId: lancamentoCartao.CategoriaId,
                observacoes: lancamentoCartao.Observacoes,
                geradoLancamento: lancamentoCartao.GeradoLancamento,
                categorias: await ListarCategoriasModelAsync(lancamentoCartao.CategoriaId),
                cartoesCredito: await ListarCartoesCreditoAsync(lancamentoCartao.CartaoCreditoId)
            );
        }

        public async Task AtualizarAsync(LancamentoCartaoEdicaoModel model)
        {
            model.AtualizarCategoriasCartoesCredito(
                categorias: await ListarCategoriasModelAsync(model.CategoriaId),
                cartoesCredito: await ListarCartoesCreditoAsync(model.CartaoCreditoId)
            );
        }

        private async Task<IEnumerable<CategoriaListaModel>> ListarCategoriasModelAsync(int? categoriaId = null)
        {
            var categorias = await _categoriaRepository.ListarCategoriasAsync(_userResolverService.GrupoId, string.Empty, TipoCategoriaPesquisa.Todos, false, categoriaId);
            return CategoriaMappers.ToListaModel(categorias);
        }

        private async Task<IEnumerable<CartaoCreditoListaModel>> ListarCartoesCreditoAsync(int? cartaoCreditoId = null)
        {
            var cartoesCredito = await _cartaoCreditoRepository.ListarCartoesCreditoAsync(_userResolverService.GrupoId, string.Empty, false, cartaoCreditoId);
            return CartaoCreditoMappers.ToListaModel(cartoesCredito);
        }
    }
}