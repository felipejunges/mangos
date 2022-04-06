using Mangos.Dominio.Entities;
using Mangos.Dominio.Interfaces.Repository;
using Mangos.Dominio.Utils;
using System;
using System.Threading.Tasks;
using Mangos.Dominio.Interfaces;

namespace Mangos.Dominio.Services
{
    public class SaldoContaBancariaService
    {
        private readonly IContaBancariaRepository _contaBancariaRepository;
        private readonly ILancamentoRepository _lancamentoRepository;
        private readonly IRendimentoMensalContaRepository _rendimentoMensalContaRepository;
        private readonly ITransferenciaContaRepository _transferenciaContaRepository;
        private readonly ISaldoContaBancariaRepository _saldoContaBancariaRepository;

        private readonly IUnitOfWork _unitOfWork;

        private readonly ContaBancariaService _contaBancariaService;

        public SaldoContaBancariaService(IContaBancariaRepository contaBancariaRepository, ILancamentoRepository lancamentoRepositoryNew, IRendimentoMensalContaRepository rendimentoMensalContaRepositoryNew, ITransferenciaContaRepository transferenciaContaRepositoryNew, ISaldoContaBancariaRepository saldoContaBancariaRepository, ContaBancariaService contaBancariaService, IUnitOfWork unitOfWork)
        {
            _contaBancariaRepository = contaBancariaRepository;
            _lancamentoRepository = lancamentoRepositoryNew;
            _rendimentoMensalContaRepository = rendimentoMensalContaRepositoryNew;
            _transferenciaContaRepository = transferenciaContaRepositoryNew;
            _saldoContaBancariaRepository = saldoContaBancariaRepository;
            this._contaBancariaService = contaBancariaService;

            _unitOfWork = unitOfWork;
        }

        public async Task<decimal> ObterValorSaldoNaDataAsync(int contaBancariaId, DateTime data)
        {
            DateTime dataInicial = new DateTime(data.Year, data.Month, 1);
            DateTime dataFinal = data.AddDays(-1);

            var saldoContaBancaria = await _saldoContaBancariaRepository.ObterSaldoUltimoSaldoAnteriorDataAsync(contaBancariaId, dataInicial);
            decimal valorSaldo;

            if (saldoContaBancaria == null)
            {
                var contaBancaria = await _contaBancariaRepository.ObterContaBancariaAsync(contaBancariaId);
                valorSaldo = contaBancaria?.SaldoInicial ?? 0;
            }
            else
            {
                valorSaldo = saldoContaBancaria.ValorSaldo;
            }

            decimal valorLancamentos = await _lancamentoRepository.ObterValorLancamentosNoPeriodoAsync(contaBancariaId, dataInicial, dataFinal);
            decimal valorRendimentosConta = await _rendimentoMensalContaRepository.ObterValorRendimentosContaNoPeriodoAsync(contaBancariaId, dataInicial, dataFinal);
            decimal valorTransferencias = await _transferenciaContaRepository.ObterValorTransferenciasContaNoPeriodo(contaBancariaId, dataInicial, dataFinal);

            return valorSaldo + valorLancamentos + valorRendimentosConta + valorTransferencias;
        }

        public async Task GerarTodosMesAtual()
        {
            var dataInicial = DateTime.Now.Date.AddDays((DateTime.Now.Day - 1) * -1);

            await _unitOfWork.BeginTransactionAsync();

            var contasBancarias = await _contaBancariaRepository.ListaContasBancariasAtivasTodosAsync();

            foreach (ContaBancaria contaBancaria in contasBancarias)
            {
                await this.SetarSaldo(contaBancaria.Id, dataInicial);
            }

            await _unitOfWork.CommitTransactionAsync();
        }

        public async Task Gerar(int grupoId, int? contaBancariaId, DateTime mesInicial)
        {
            await _unitOfWork.BeginTransactionAsync();

            if (contaBancariaId != null)
            {
                var contaBancaria = await _contaBancariaRepository.ObterContaBancariaAsync(contaBancariaId.Value);

                if (contaBancaria != null && contaBancaria.GrupoId == grupoId)
                    await this.SetarSaldo(contaBancariaId.Value, mesInicial);
            }
            else
            {
                var contasBancarias = await _contaBancariaRepository.ListaContasBancariasAsync(grupoId, string.Empty, false);

                foreach (var contaBancaria in contasBancarias)
                {
                    await this.SetarSaldo(contaBancaria.Id, mesInicial);
                }
            }

            await _unitOfWork.CommitTransactionAsync();
        }

        public async Task SetarDataHoraConferenciaSaldo(int id)
        {
            var saldoContaBancaria = await _saldoContaBancariaRepository.ObterSaldoContaBancariaAsync(id);

            if (saldoContaBancaria != null)
            {
                saldoContaBancaria.DataHoraConferenciaSaldo = DateTime.Now;
                saldoContaBancaria.ValorConferenciaSaldo = saldoContaBancaria.ValorSaldo;

                await _saldoContaBancariaRepository.AlterarAsync(saldoContaBancaria);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        public async Task<NotificationResult> Fechar(int id)
        {
            var result = new NotificationResult();

            var saldoContaBancaria = await _saldoContaBancariaRepository.ObterSaldoContaBancariaAsync(id);

            if (saldoContaBancaria != null)
            {
                if (saldoContaBancaria.Data >= new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1))
                    result.AddNotification("O saldo do mês corrente não pode ser fechado.");
                else if (saldoContaBancaria.ValorConferenciaSaldo == null || saldoContaBancaria.ValorConferenciaSaldo != saldoContaBancaria.ValorSaldo)
                    result.AddNotification("O registro não pode ser fechado se o saldo não foi conferido corretamente.");
                else if (await _saldoContaBancariaRepository.ValidarExisteSaldoAnteriorAbertoAsync(saldoContaBancaria.ContaBancariaId, saldoContaBancaria.Data))
                    result.AddNotification("Existe saldo de período anterior com fechamento pendente.");
                else
                {
                    saldoContaBancaria.Fechado = true;
                    saldoContaBancaria.DataHoraFechamento = DateTime.Now;

                    await _saldoContaBancariaRepository.AlterarAsync(saldoContaBancaria);
                    await _unitOfWork.SaveChangesAsync();
                }
            }

            return result;
        }

        public async Task<NotificationResult> ReabrirFechamento(int id)
        {
            var result = new NotificationResult();

            var saldoContaBancaria = await _saldoContaBancariaRepository.ObterSaldoContaBancariaAsync(id);

            if (saldoContaBancaria != null)
            {
                if (await _saldoContaBancariaRepository.ValidarExisteSaldoPosteriorFechadoAsync(saldoContaBancaria.ContaBancariaId, saldoContaBancaria.Data))
                    result.AddNotification("Existe saldo de período posterior com fechamento realizado.");
                else
                {
                    saldoContaBancaria.Fechado = false;
                    saldoContaBancaria.DataHoraFechamento = null;

                    await _saldoContaBancariaRepository.AlterarAsync(saldoContaBancaria);
                    await _unitOfWork.SaveChangesAsync();
                }
            }

            return result;
        }

        public async Task SetarSaldo(int? contaBancariaId, DateTime? data)
        {
            await this.SetarSaldo(null, contaBancariaId, null, data);
        }

        public async Task SetarSaldo(int? contaBancariaAtualId, int? contaBancariaId, DateTime? dataAtual, DateTime? data)
        {
            DateTime? menorData = dataAtual;
            if (menorData == null || (data != null && menorData.Value > data.Value))
                menorData = data;

            if (menorData != null)
            {
                if (contaBancariaAtualId != null)
                    await this.SetarSaldo(contaBancariaAtualId.Value, menorData.Value);

                if (contaBancariaId != null && contaBancariaId != contaBancariaAtualId)
                    await this.SetarSaldo(contaBancariaId.Value, menorData.Value);
            }
        }

        public async Task SetarSaldo(int contaBancariaId, DateTime data)
        {
            data = new DateTime(data.Year, data.Month, 1);

            var contaBancaria = await _contaBancariaRepository.ObterContaBancariaAsync(contaBancariaId);

            if (contaBancaria is null)
                return;

            if (!contaBancaria.Ativo)
                return;

            DateTime dataAnterior = data.AddMonths(-1);
            DateTime dataHoje = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            var saldoContaBancariaAnterior = await _saldoContaBancariaRepository.ObterSaldoContaBancariaPeloMesAsync(contaBancariaId, dataAnterior);

            decimal valorSaldo = saldoContaBancariaAnterior != null ? saldoContaBancariaAnterior.ValorSaldo : contaBancaria.SaldoInicial;

            DateTime? menorMesValoresContaBancaria = await ObterMenorMesMovimentacaoAsync(contaBancariaId);

            //
            for (DateTime dataGerar = data; dataGerar <= dataHoje; dataGerar = dataGerar.AddMonths(1))
            {
                var saldoContaMes = await _saldoContaBancariaRepository.ObterSaldoContaBancariaPeloMesAsync(contaBancariaId, dataGerar);

                if (saldoContaMes != null && saldoContaMes.Fechado)
                {
                    valorSaldo += saldoContaMes.ValorMovimentacoes;
                }
                else
                {
                    DateTime dataInicial = dataGerar;
                    DateTime dataFinal = dataGerar.AddMonths(1).AddDays(-1);

                    // busca o valor pago/recebido no período e das transferências de conta
                    decimal valorLancamentos = await _lancamentoRepository.ObterValorLancamentosNoPeriodoAsync(contaBancariaId, dataInicial, dataFinal);
                    decimal valorRendimentosConta = await _rendimentoMensalContaRepository.ObterValorRendimentosContaNoPeriodoAsync(contaBancariaId, dataInicial, dataFinal);
                    decimal valorTransferenciasConta = await _transferenciaContaRepository.ObterValorTransferenciasContaNoPeriodo(contaBancariaId, dataInicial, dataFinal);

                    var valorMovimentacoes = valorLancamentos + valorRendimentosConta + valorTransferenciasConta;
                    valorSaldo += valorMovimentacoes;

                    // só inclui novos registros de saldo, caso o mês atual seja posterior à primeira movimentação da conta
                    // senão, só edita os registros já existentes
                    bool criaNovosRegistrosSaldo = menorMesValoresContaBancaria != null && dataGerar >= menorMesValoresContaBancaria.Value;

                    //
                    if (saldoContaMes == null)
                    {
                        if (!criaNovosRegistrosSaldo)
                            continue;

                        saldoContaMes = new SaldoContaBancaria()
                        {
                            Id = 0,
                            ContaBancariaId = contaBancariaId,
                            Data = dataGerar
                        };
                    }

                    saldoContaMes.ValorMovimentacoes = valorMovimentacoes;
                    saldoContaMes.ValorSaldo = valorSaldo;

                    // salva o registro
                    if (saldoContaMes.Id == 0)
                        await _saldoContaBancariaRepository.IncluirAsync(saldoContaMes);
                    else
                        await _saldoContaBancariaRepository.AlterarAsync(saldoContaMes);

                    await _unitOfWork.SaveChangesAsync();
                }
            }
        }

        private async Task<DateTime?> ObterMenorMesMovimentacaoAsync(int contaBancariaId)
        {
            DateTime? menorData = null;

            var menorDataLancamentos = await _lancamentoRepository.ObterMenorDataDaContaBancariaAsync(contaBancariaId);
            var menorDataRendimentosMensais = await _rendimentoMensalContaRepository.ObterMenorDataDaContaBancariaAsync(contaBancariaId);
            var menorDataTransferenciasOrigem = await _transferenciaContaRepository.ObterMenorDataDaContaBancariaOrigemAsync(contaBancariaId);
            var menorDataTransferenciasDestino = await _transferenciaContaRepository.ObterMenorDataDaContaBancariaDestinoAsync(contaBancariaId);

            menorData = menorDataLancamentos;

            if (menorData is null || menorDataRendimentosMensais < menorData)
                menorData = menorDataRendimentosMensais;

            if (menorData is null || menorDataTransferenciasOrigem < menorData)
                menorData = menorDataTransferenciasOrigem;

            if (menorData is null || menorDataTransferenciasDestino < menorData)
                menorData = menorDataTransferenciasDestino;

            return menorData == null ? null : new DateTime(menorData.Value.Year, menorData.Value.Month, 1);
        }
    }
}