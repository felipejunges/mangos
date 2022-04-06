using Mangos.Dominio.Entities;
using Mangos.Dominio.Enums;
using Mangos.Dominio.Interfaces;
using Mangos.Dominio.Interfaces.Repository;
using Mangos.Dominio.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Mangos.Dominio.Services
{
    public class LancamentoCartaoService
    {
        private readonly ILancamentoCartaoRepository _lancamentoCartaoRepository;
        private readonly ILancamentoRepository _lancamentoRepository;
        private readonly ICartaoCreditoRepository _cartaoCreditoRepository;
        private readonly IUnitOfWork _unitOfWork;

        public LancamentoCartaoService(ILancamentoCartaoRepository lancamentoCartaoRepository, ILancamentoRepository lancamentoRepository, ICartaoCreditoRepository cartaoCreditoRepository, IUnitOfWork unitOfWork)
        {
            _lancamentoCartaoRepository = lancamentoCartaoRepository;
            _lancamentoRepository = lancamentoRepository;
            _cartaoCreditoRepository = cartaoCreditoRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task IncluirAsync(IncluirLancamentoCartaoCommand incluirLancamentoCartao)
        {
            await _unitOfWork.BeginTransactionAsync();

            int numeroParcela = 1;
            int totalParcelas = incluirLancamentoCartao.Parcelado && incluirLancamentoCartao.NumeroParcelas != null ? incluirLancamentoCartao.NumeroParcelas.Value : 1;
            var agrupador = incluirLancamentoCartao.Parcelado ? Guid.NewGuid() : (Guid?)null;

            decimal valorParcela = incluirLancamentoCartao.Parcelado && incluirLancamentoCartao.NumeroParcelas != null
                ? Math.Round(incluirLancamentoCartao.ValorTotal / incluirLancamentoCartao.NumeroParcelas.Value, 2)
                : incluirLancamentoCartao.ValorTotal;

            for (int i = 1; i <= totalParcelas; i++)
            {
                var lancamentoCartao = new LancamentoCartao()
                {
                    Id = 0,
                    GrupoId = incluirLancamentoCartao.GrupoId,
                    DataHoraCadastro = DateTime.Now,
                    CartaoCreditoId = incluirLancamentoCartao.CartaoCreditoId,
                    TipoLancamento = incluirLancamentoCartao.TipoLancamento,
                    Valor = valorParcela,
                    MesReferencia = incluirLancamentoCartao.DataReferencia!.Value.AddMonths(i - 1),
                    Descricao = incluirLancamentoCartao.Descricao!,
                    Agrupador = agrupador,
                    NumeroParcela = numeroParcela,
                    TotalParcelas = totalParcelas,
                    PessoaId = incluirLancamentoCartao.PessoaId,
                    CategoriaId = incluirLancamentoCartao.CategoriaId,
                    GeradoLancamento = false
                };

                await _lancamentoCartaoRepository.IncluirAsync(lancamentoCartao);
                await _unitOfWork.SaveChangesAsync();

                numeroParcela++;
            }

            await _unitOfWork.CommitTransactionAsync();
        }

        public async Task AlterarAsync(LancamentoCartao lancamentoCartao)
        {
            await _lancamentoCartaoRepository.AlterarAsync(lancamentoCartao);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task FecharMesAsync(int grupoId, int cartaoCreditoId, DateTime mesReferencia, bool gerarLancamento, int? categoriaId)
        {
            await _unitOfWork.BeginTransactionAsync();

            var lancamentosCartao = await _lancamentoCartaoRepository.ListarLancamentosCartaoFecharAsync(grupoId, cartaoCreditoId, mesReferencia);

            Lancamento? lancamento = null;

            if (gerarLancamento)
            {
                decimal valorTotalLancamento = lancamentosCartao.Sum(s => s.TipoLancamento == TipoLancamentoCartao.Despesa ? s.Valor : s.Valor * -1);

                if (valorTotalLancamento > 0M)
                {
                    var cartaoCredito = await _cartaoCreditoRepository.ObterCartaoCreditoAsync(cartaoCreditoId);

                    if (cartaoCredito is null)
                        return;

                    //
                    int diaVencimentoCartao = cartaoCredito.DiaVencimento;
                    int offsetReferenciaVencimento = cartaoCredito.OffsetReferenciaVencimento;

                    DateTime dataVencimento = mesReferencia.AddMonths(offsetReferenciaVencimento).AddDays(diaVencimentoCartao - 1);

                    //
                    lancamento = new Lancamento()
                    {
                        GrupoId = grupoId,
                        DataHoraCadastro = DateTime.Now,
                        Id = 0,
                        Tipo = TipoLancamento.Despesa,
                        Valor = valorTotalLancamento,
                        DataVencimento = dataVencimento,
                        Descricao = "Cartão de crédito " + cartaoCredito.Descricao,
                        CategoriaId = categoriaId,
                        NumeroParcela = 1,
                        TotalParcelas = 1,
                        Pago = false
                    };

                    await _lancamentoRepository.IncluirAsync(lancamento);
                    await _unitOfWork.SaveChangesAsync();
                }
            }

            //
            foreach (var lancamentoCartao in lancamentosCartao)
            {
                lancamentoCartao.GeradoLancamento = true;
                lancamentoCartao.Lancamento = lancamento;
                lancamentoCartao.DataHoraGeracaoLancamento = DateTime.Now;

                await _lancamentoCartaoRepository.AlterarAsync(lancamentoCartao);
                await _unitOfWork.SaveChangesAsync();
            }

            await _unitOfWork.CommitTransactionAsync();
        }

        public async Task ReabrirMesAsync(int grupoId, int cartaoCreditoId, DateTime mesReferencia)
        {
            await _unitOfWork.BeginTransactionAsync();

            var lancamentosCartao = await _lancamentoCartaoRepository.ListarLancamentosCartaoReabrirAsync(grupoId, cartaoCreditoId, mesReferencia);

            // busca e exclui os lançamentos vinculados com os lançamentos de cartão
            var lancamentosExcluir = lancamentosCartao.Where(l => l.Lancamento != null).GroupBy(l => l.Lancamento).Select(g => g.Key!).ToList();

            // reabre os lançamentos cartão
            foreach (var lancamentoCartao in lancamentosCartao)
            {
                lancamentoCartao.GeradoLancamento = false;
                lancamentoCartao.LancamentoId = null;
                lancamentoCartao.DataHoraGeracaoLancamento = null;

                await _lancamentoCartaoRepository.AlterarAsync(lancamentoCartao);
                await _unitOfWork.SaveChangesAsync();
            }

            foreach (var lancamentoExcluir in lancamentosExcluir)
            {
                await _lancamentoRepository.RemoverAsync(lancamentoExcluir);
                await _unitOfWork.SaveChangesAsync();
            }

            await _unitOfWork.CommitTransactionAsync();
        }

        public async Task RemoverAsync(LancamentoCartao lancamentoCartao)
        {
            await _lancamentoCartaoRepository.RemoverAsync(lancamentoCartao);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}