using Mangos.Dominio.Entities;
using Mangos.Dominio.Enums;
using Mangos.Dominio.Interfaces;
using Mangos.Dominio.Interfaces.Repository;
using Mangos.Dominio.Models;
using System;
using System.Threading.Tasks;

namespace Mangos.Dominio.Services
{
    public class LancamentoService
    {
        private readonly SaldoContaBancariaService _saldoContaBancariaService;
        private readonly IContaBancariaRepository _contaBancariaRepository;
        private readonly ILancamentoRepository _lancamentoRepository;
        private readonly IUnitOfWork _unitOfWork;

        public LancamentoService(SaldoContaBancariaService saldoContaBancariaService, IContaBancariaRepository contaBancariaRepository, ILancamentoRepository lancamentoRepository, IUnitOfWork unitOfWork)
        {
            _saldoContaBancariaService = saldoContaBancariaService;
            _contaBancariaRepository = contaBancariaRepository;
            _lancamentoRepository = lancamentoRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task IncluirCompletaAsync(IncluirLancamentoCompletoCommand incluirLancamento)
        {
            decimal valorLancamentos = incluirLancamento.Parcelado && incluirLancamento.TipoParcelamento == TipoParcelamentoLancamento.Parcelar && incluirLancamento.NumeroParcelas != null
                ? Math.Round(incluirLancamento.Valor / incluirLancamento.NumeroParcelas.Value, 2)
                : incluirLancamento.Valor;

            int totalParcelas = incluirLancamento.Parcelado && incluirLancamento.TipoParcelamento == TipoParcelamentoLancamento.Parcelar
                ? incluirLancamento.NumeroParcelas!.Value
                : 1;

            var agrupador = incluirLancamento.Parcelado ? Guid.NewGuid() : (Guid?)null;

            await _unitOfWork.BeginTransactionAsync();

            var lancamento = new Lancamento()
            {
                Id = 0,
                GrupoId = incluirLancamento.GrupoId,
                DataHoraCadastro = DateTime.Now,
                Tipo = incluirLancamento.TipoLancamento,
                Valor = valorLancamentos,
                DataVencimento = incluirLancamento.DataVencimento!.Value,
                Descricao = incluirLancamento.Descricao,
                Agrupador = agrupador,
                NumeroParcela = 1,
                TotalParcelas = totalParcelas,
                PessoaId = incluirLancamento.PessoaId,
                CategoriaId = incluirLancamento.CategoriaId,
                Observacoes = incluirLancamento.Observacoes,
                Pago = incluirLancamento.Pago,
                DataPagamento = incluirLancamento.DataPagamento,
                ValorPago = incluirLancamento.ValorPago,
                ContaBancariaId = incluirLancamento.ContaBancariaId,
                DataContaBancaria = incluirLancamento.DataContaBancaria
            };

            await _lancamentoRepository.IncluirAsync(lancamento);
            await _unitOfWork.SaveChangesAsync();

            //
            if (incluirLancamento.Parcelado)
            {
                for (int i = 2; i <= incluirLancamento.NumeroParcelas!.Value; i++)
                {
                    var lancamentoFor = new Lancamento()
                    {
                        Id = 0,
                        GrupoId = incluirLancamento.GrupoId,
                        DataHoraCadastro = DateTime.Now,
                        Tipo = incluirLancamento.TipoLancamento,
                        Valor = valorLancamentos,
                        DataVencimento = incluirLancamento.DataVencimento.Value.AddMonths(i - 1),
                        Descricao = incluirLancamento.Descricao,
                        Agrupador = agrupador,
                        NumeroParcela = incluirLancamento.TipoParcelamento == TipoParcelamentoLancamento.Replicar ? 1 : i,
                        TotalParcelas = totalParcelas,
                        PessoaId = incluirLancamento.PessoaId,
                        CategoriaId = incluirLancamento.CategoriaId,
                        Observacoes = incluirLancamento.Observacoes,
                        Pago = false
                    };

                    await _lancamentoRepository.IncluirAsync(lancamentoFor);
                    await _unitOfWork.SaveChangesAsync();
                }
            }

            // seta o saldo da conta corrente
            await _saldoContaBancariaService.SetarSaldo(incluirLancamento.ContaBancariaId, lancamento.DataContaBancaria);

            await _unitOfWork.CommitTransactionAsync();
        }

        public async Task IncluirPagaAsync(IncluirLancamentoPagoCommand incluirLancamentoPago)
        {
            await _unitOfWork.BeginTransactionAsync();

            DateTime? dataContaBancaria = await CalcularDataContaBancariaAsync(incluirLancamentoPago.ContaBancariaId, incluirLancamentoPago.DataPagamento!.Value);

            var lancamento = new Lancamento()
            {
                Id = 0,
                GrupoId = incluirLancamentoPago.GrupoId,
                DataHoraCadastro = DateTime.Now,
                Tipo = incluirLancamentoPago.TipoLancamento,
                Valor = incluirLancamentoPago.Valor,
                DataVencimento = incluirLancamentoPago.DataPagamento!.Value,
                Descricao = incluirLancamentoPago.Descricao!,
                Agrupador = null,
                NumeroParcela = 1,
                TotalParcelas = 1,
                PessoaId = incluirLancamentoPago.PessoaId,
                CategoriaId = incluirLancamentoPago.CategoriaId,
                Observacoes = null,
                Pago = true,
                DataPagamento = incluirLancamentoPago.DataPagamento,
                ValorPago = incluirLancamentoPago.Valor,
                ContaBancariaId = incluirLancamentoPago.ContaBancariaId,
                DataContaBancaria = dataContaBancaria
            };

            await _lancamentoRepository.IncluirAsync(lancamento);
            await _unitOfWork.SaveChangesAsync();

            // seta o saldo da conta corrente
            await _saldoContaBancariaService.SetarSaldo(incluirLancamentoPago.ContaBancariaId, incluirLancamentoPago.DataPagamento);

            await _unitOfWork.CommitTransactionAsync();
        }

        public async Task AlterarAsync(Lancamento lancamento)
        {
            var lancamentoAtual = await _lancamentoRepository.ObterLancamentoNoTrackingAsync(lancamento.Id);

            await _unitOfWork.BeginTransactionAsync();

            await _lancamentoRepository.AlterarAsync(lancamento);
            await _unitOfWork.SaveChangesAsync();

            // seta o saldo da conta corrente
            await _saldoContaBancariaService.SetarSaldo(
                lancamentoAtual?.ContaBancariaId,
                lancamento.ContaBancariaId,
                lancamentoAtual?.DataContaBancaria,
                lancamento.DataContaBancaria);

            await _unitOfWork.CommitTransactionAsync();
        }

        public async Task PagarAsync(int lancamentoId, int? contaBancariaId, DateTime dataPagamento)
        {
            DateTime? dataContaBancaria = await CalcularDataContaBancariaAsync(contaBancariaId, dataPagamento);

            var lancamento = await _lancamentoRepository.ObterLancamentoAsync(lancamentoId);

            if (lancamento is null)
                return;

            if (!lancamento.Pago)
            {
                await _unitOfWork.BeginTransactionAsync();

                lancamento.Pago = true;
                lancamento.DataPagamento = dataPagamento;
                lancamento.ValorPago = lancamento.Valor;
                lancamento.ContaBancariaId = contaBancariaId;
                lancamento.DataContaBancaria = dataContaBancaria;

                await _lancamentoRepository.AlterarAsync(lancamento);
                await _unitOfWork.SaveChangesAsync();

                await _saldoContaBancariaService.SetarSaldo(contaBancariaId, dataContaBancaria);

                await _unitOfWork.CommitTransactionAsync();
            }
        }

        public async Task RemoverAsync(Lancamento lancamento)
        {
            // o EF obriga isso (após o "remove", os campos int? ficam nulos no objeto)
            int? contaBancariaId = lancamento.ContaBancariaId;
            DateTime? dataContaBancaria = lancamento.DataContaBancaria;

            await _unitOfWork.BeginTransactionAsync();

            // exclui o lançamento
            await _lancamentoRepository.RemoverAsync(lancamento);
            await _unitOfWork.SaveChangesAsync();

            // recalcula o saldo no período
            await _saldoContaBancariaService.SetarSaldo(contaBancariaId, dataContaBancaria);

            await _unitOfWork.CommitTransactionAsync();
        }

        public async Task<DateTime?> CalcularDataContaBancariaAsync(int? contaBancariaId, DateTime dataPagamento)
        {
            if (contaBancariaId is null)
                return null;

            var contaBancaria = await _contaBancariaRepository.ObterContaBancariaAsync(contaBancariaId.Value);

            if (contaBancaria is null)
                return dataPagamento;

            if (contaBancaria.PularFinaisSemanaLancamentoRapido)
            {
                while (dataPagamento.DayOfWeek == DayOfWeek.Saturday || dataPagamento.DayOfWeek == DayOfWeek.Sunday)
                {
                    dataPagamento = dataPagamento.AddDays(1);
                }
            }

            return dataPagamento;
        }
    }
}