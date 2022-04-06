using Mangos.Dominio.Constants;
using Mangos.Dominio.Entities;
using Mangos.Dominio.Enums;
using Mangos.Dominio.Interfaces.Repository;
using Mangos.Dominio.Services.User;
using Mangos.Mvc.Models.Mappers;
using Mangos.Mvc.Models.ViewModels.ModelBuilders.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mangos.Mvc.Models.ViewModels.ModelBuilders
{
    public class LancamentoEdicaoModelBinder : IModelBuilder<LancamentoEdicaoModel, Lancamento>
    {
        private readonly ILancamentoRepository _lancamentoRepository;
        private readonly ICategoriaRepository _categoriaRepository;
        private readonly IContaBancariaRepository _contaBancariaRepository;
        private readonly IUserResolverService _userResolverService;

        public LancamentoEdicaoModelBinder(ILancamentoRepository lancamentoRepository, ICategoriaRepository categoriaRepository, IContaBancariaRepository contaBancariaRepository, IUserResolverService userResolverService)
        {
            _lancamentoRepository = lancamentoRepository;
            _categoriaRepository = categoriaRepository;
            _contaBancariaRepository = contaBancariaRepository;
            _userResolverService = userResolverService;
        }

        public async Task<Lancamento?> CriarEntidadeAsync(LancamentoEdicaoModel lancamentoEdicaoModel)
        {
            var lancamento = await _lancamentoRepository.ObterLancamentoAsync(lancamentoEdicaoModel.Id);

            if (lancamento is null)
                return null;

            lancamento.Valor = lancamentoEdicaoModel.Valor;
            lancamento.DataVencimento = lancamentoEdicaoModel.DataVencimento;
            lancamento.Descricao = lancamentoEdicaoModel.Descricao!;
            lancamento.NumeroParcela = lancamentoEdicaoModel.NumeroParcela;
            lancamento.TotalParcelas = lancamentoEdicaoModel.TotalParcelas;
            lancamento.PessoaId = lancamentoEdicaoModel.PessoaId;
            lancamento.CategoriaId = lancamentoEdicaoModel.CategoriaId;
            lancamento.Observacoes = lancamentoEdicaoModel.Observacoes;
            lancamento.Pago = lancamentoEdicaoModel.Pago;

            if (lancamentoEdicaoModel.Pago)
            {
                lancamento.DataPagamento = lancamentoEdicaoModel.DataPagamento;
                lancamento.ValorPago = lancamentoEdicaoModel.ValorPago;
                lancamento.ContaBancariaId = lancamentoEdicaoModel.ContaBancariaId;
                lancamento.DataContaBancaria = lancamentoEdicaoModel.DataContaBancaria;
            }
            else
            {
                lancamento.DataPagamento = null;
                lancamento.ValorPago = null;
                lancamento.ContaBancariaId = null;
                lancamento.DataContaBancaria = null;
            }

            return lancamento;
        }

        public async Task<LancamentoEdicaoModel?> CriarModelPeloIdAsync(int? id)
        {
            if (id is null)
                return null;

            var lancamento = await _lancamentoRepository.ObterLancamentoAsync(id.Value);

            if (lancamento == null)
                return null;

            if (lancamento.GrupoId != _userResolverService.GrupoId)
                return null;

            return new LancamentoEdicaoModel(
                id: lancamento.Id,
                grupoId: lancamento.GrupoId,
                tipo: lancamento.Tipo,
                valor: lancamento.Valor,
                dataVencimento: lancamento.DataVencimento,
                descricao: lancamento.Descricao!,
                numeroParcela: lancamento.NumeroParcela,
                totalParcelas: lancamento.TotalParcelas,
                pessoaId: lancamento.PessoaId,
                pessoa: lancamento.Pessoa?.Nome ?? string.Empty,
                categoriaId: lancamento.CategoriaId,
                observacoes: lancamento.Observacoes,
                pago: lancamento.Pago,
                dataPagamento: lancamento.DataPagamento,
                valorPago: lancamento.ValorPago,
                contaBancariaId: lancamento.ContaBancariaId,
                dataContaBancaria: lancamento.DataContaBancaria,
                categorias: await ListarCategoriasModelAsync(lancamento.Tipo, lancamento.CategoriaId),
                contasBancarias: await ListarContasBancariasAsync(lancamento.ContaBancariaId)
            );
        }

        public async Task AtualizarAsync(LancamentoEdicaoModel model)
        {
            model.AtualizarCategoriasCartoesCredito(
                categorias: await ListarCategoriasModelAsync(model.Tipo, model.CategoriaId),
                contasBancarias: await ListarContasBancariasAsync(model.ContaBancariaId)
            );
        }

        private async Task<IEnumerable<CategoriaListaModel>> ListarCategoriasModelAsync(TipoLancamento tipoLancamento, int? categoriaId = null)
        {
            var tipo = tipoLancamento == TipoLancamento.Receita ? TipoCategoriaPesquisa.Receita : TipoCategoriaPesquisa.Despesa;

            var categorias = await _categoriaRepository.ListarCategoriasAsync(_userResolverService.GrupoId, string.Empty, tipo, false, categoriaId);
            return CategoriaMappers.ToListaModel(categorias);
        }

        private async Task<IEnumerable<ContaBancariaListaModel>> ListarContasBancariasAsync(int? contaBancariaId = null)
        {
            var contasBancarias = await _contaBancariaRepository.ListaContasBancariasAsync(_userResolverService.GrupoId, string.Empty, false, contaBancariaId);
            return ContaBancariaMappers.ToListaModel(contasBancarias);
        }
    }
}