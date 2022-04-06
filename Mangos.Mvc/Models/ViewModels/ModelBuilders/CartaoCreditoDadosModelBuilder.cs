using Mangos.Dominio.Constants;
using Mangos.Dominio.Entities;
using Mangos.Dominio.Interfaces.Repository;
using Mangos.Dominio.Services.User;
using Mangos.Mvc.Models.ViewModels.ModelBuilders.Interfaces;
using System.Threading.Tasks;

namespace Mangos.Mvc.Models.ViewModels.ModelBuilders
{
    public class CartaoCreditoDadosModelBuilder : IModelBuilder<CartaoCreditoDadosModel, CartaoCredito>
    {
        private readonly ICartaoCreditoRepository _cartaoCreditoRepository;
        private readonly ICategoriaRepository _categoriaRepository;
        private readonly IUserResolverService _userResolverService;

        public CartaoCreditoDadosModelBuilder(ICartaoCreditoRepository cartaoCreditoRepository, ICategoriaRepository categoriaRepository, IUserResolverService userResolverService)
        {
            _cartaoCreditoRepository = cartaoCreditoRepository;
            _categoriaRepository = categoriaRepository;
            _userResolverService = userResolverService;
        }

        public async Task<CartaoCredito?> CriarEntidadeAsync(CartaoCreditoDadosModel cartaoCreditoDadosModel)
        {
            if (!cartaoCreditoDadosModel.CheckValidationHash())
                return null;

            CartaoCredito? cartaoCredito;

            if (cartaoCreditoDadosModel.Id == 0)
                cartaoCredito = new CartaoCredito() { Id = 0, GrupoId = cartaoCreditoDadosModel.GrupoId };
            else
                cartaoCredito = await _cartaoCreditoRepository.ObterCartaoCreditoAsync(cartaoCreditoDadosModel.Id);

            if (cartaoCredito is null)
                return null;

            if (cartaoCredito.GrupoId != _userResolverService.GrupoId)
                return null;

            cartaoCredito.Descricao = cartaoCreditoDadosModel.Descricao!;
            cartaoCredito.CategoriaId = cartaoCreditoDadosModel.CategoriaId;
            cartaoCredito.DiaFechamento = cartaoCreditoDadosModel.DiaFechamento;
            cartaoCredito.DiaVencimento = cartaoCreditoDadosModel.DiaVencimento;
            cartaoCredito.OffsetReferenciaVencimento = cartaoCreditoDadosModel.OffsetReferenciaVencimento ? 1 : 0;
            cartaoCredito.ValorLimite = cartaoCreditoDadosModel.ValorLimite;
            cartaoCredito.ExibirDadosRelatorio = cartaoCreditoDadosModel.ExibirDadosRelatorio;
            cartaoCredito.GerarLancamentoFecharMes = cartaoCreditoDadosModel.GerarLancamentoFecharMes;
            cartaoCredito.Ativo = cartaoCreditoDadosModel.Ativo;

            return cartaoCredito;
        }

        public async Task<CartaoCreditoDadosModel?> CriarModelPeloIdAsync(int? id)
        {
            if (id == null)
            {
                return CartaoCreditoDadosModel.Novo(
                    _userResolverService.GrupoId,
                    await _categoriaRepository.ListarCategoriasAsync(_userResolverService.GrupoId, null, TipoCategoriaPesquisa.Despesa, false)
                );
            }
            else
            {
                var cartaoCredito = await _cartaoCreditoRepository.ObterCartaoCreditoAsync(id.Value);

                if (cartaoCredito == null)
                    return null;

                if (cartaoCredito.GrupoId != _userResolverService.GrupoId)
                    return null;

                return new CartaoCreditoDadosModel(
                    id: cartaoCredito.Id,
                    grupoId: cartaoCredito.GrupoId,
                    descricao: cartaoCredito.Descricao,
                    categoriaId: cartaoCredito.CategoriaId,
                    diaFechamento: cartaoCredito.DiaFechamento,
                    diaVencimento: cartaoCredito.DiaVencimento,
                    offsetReferenciaVencimento: cartaoCredito.OffsetReferenciaVencimento > 0,
                    valorLimite: cartaoCredito.ValorLimite,
                    exibirDadosRelatorio: cartaoCredito.ExibirDadosRelatorio,
                    gerarLancamentoFecharMes: cartaoCredito.GerarLancamentoFecharMes,
                    ativo: cartaoCredito.Ativo,
                    categorias: await _categoriaRepository.ListarCategoriasAsync(cartaoCredito.GrupoId, null, TipoCategoriaPesquisa.Despesa, false, cartaoCredito.CategoriaId)
                );
            }
        }

        public async Task AtualizarAsync(CartaoCreditoDadosModel model)
        {
            model.AtualizarCategorias(
                await _categoriaRepository.ListarCategoriasAsync(model.GrupoId, null, TipoCategoriaPesquisa.Despesa, false, model.CategoriaId)
            );
        }
    }
}