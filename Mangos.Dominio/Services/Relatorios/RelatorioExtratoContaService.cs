using Mangos.Dominio.Enums;
using Mangos.Dominio.Models.Relatorios;
using Mangos.Dominio.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mangos.Dominio.Services.Relatorios
{
    public class RelatorioExtratoContaService
    {
        private readonly SaldoContaBancariaService _saldoContaBancariaService;
        private readonly ContaBancariaService _contaBancariaService;
        private readonly ILancamentoRepository _lancamentoRepository;
        private readonly IRendimentoMensalContaRepository _rendimentoMensalContaRepository;
        private readonly ITransferenciaContaRepository _transferenciaContaRepository;

        public RelatorioExtratoContaService(SaldoContaBancariaService saldoContaBancariaService, ContaBancariaService contaBancariaService, ILancamentoRepository lancamentoRepository, IRendimentoMensalContaRepository rendimentoMensalContaRepository, ITransferenciaContaRepository transferenciaContaRepository)
        {
            _saldoContaBancariaService = saldoContaBancariaService;
            _contaBancariaService = contaBancariaService;
            _lancamentoRepository = lancamentoRepository;
            _rendimentoMensalContaRepository = rendimentoMensalContaRepository;
            _transferenciaContaRepository = transferenciaContaRepository;
        }

        public async Task<RelatorioExtratoContaModel> ObterRelatorioExtratoContaAsync(int grupoId, int contaBancariaId, DateTime dataInicial, DateTime dataFinal)
        {
            if (!await _contaBancariaService.VerificarGrupoId(contaBancariaId, grupoId))
                return new RelatorioExtratoContaModel();

            decimal valorSaldoInicial = await _saldoContaBancariaService.ObterValorSaldoNaDataAsync(contaBancariaId, dataInicial);

            var lancamentos = await ListarLancamentosRelatorioAsync(grupoId, contaBancariaId, dataInicial, dataFinal);

            var transferenciasContaOrigem = await ListarTransferenciasContaOrigemRelatorioAsync(grupoId, contaBancariaId, dataInicial, dataFinal);

            var transferenciasContaDestino = await ListarTransferenciasContaDestinoRelatorioAsync(grupoId, contaBancariaId, dataInicial, dataFinal);

            var rendimentosMensalConta = await ListarRendimentosContaRelatorioAsync(grupoId, contaBancariaId, dataInicial, dataFinal);

            var itens = lancamentos
                            .Concat(rendimentosMensalConta)
                            .Concat(transferenciasContaOrigem)
                            .Concat(transferenciasContaDestino)
                            .OrderBy(o => o.DataContaBancaria)
                            .ThenBy(o => o.DataPagamento)
                            .ToList();

            var relatorio = new RelatorioExtratoContaModel()
            {
                ValorSaldoInicial = valorSaldoInicial,
                Itens = itens
            };

            return relatorio;
        }

        private async Task<List<RelatorioExtratoContaItemModel>> ListarLancamentosRelatorioAsync(int grupoId, int contaBancariaId, DateTime dataInicial, DateTime dataFinal)
        {
            var lancamentos = await _lancamentoRepository.ListarLancamentosRelatorioExtratoAsync(grupoId, contaBancariaId, dataInicial, dataFinal);

            return lancamentos
                    .Select(l => new RelatorioExtratoContaItemModel(
                        dataContaBancaria: l.DataContaBancaria!.Value,
                        dataPagamento: l.DataPagamento!.Value,
                        descricao: l.DescricaoComParcelas,
                        pessoa: l.Pessoa != null ? l.Pessoa.Nome : null,
                        tipo: l.Tipo,
                        valor: l.ValorPago!.Value)
                    )
                    .ToList();
        }

        private async Task<List<RelatorioExtratoContaItemModel>> ListarRendimentosContaRelatorioAsync(int grupoId, int contaBancariaId, DateTime dataInicial, DateTime dataFinal)
        {
            var rendimentosConta = await _rendimentoMensalContaRepository.ListarRendimentosContaRelatorioExtratoAsync(grupoId, contaBancariaId, dataInicial, dataFinal);

            return rendimentosConta
                    .Select(r => new RelatorioExtratoContaItemModel(
                        dataContaBancaria: r.MesReferencia,
                        dataPagamento: r.MesReferencia,
                        descricao: "Rendimento mensal conta",
                        pessoa: null,
                        tipo: TipoLancamento.Receita,
                        valor: r.Valor)
                    )
                    .ToList();
        }

        private async Task<List<RelatorioExtratoContaItemModel>> ListarTransferenciasContaOrigemRelatorioAsync(int grupoId, int contaBancariaId, DateTime dataInicial, DateTime dataFinal)
        {
            var transferenciasConta = await _transferenciaContaRepository.ListarTransferenciasContaOrigemRelatorioExtratoAsync(grupoId, contaBancariaId, dataInicial, dataFinal);

            return transferenciasConta
                    .Select(o => new RelatorioExtratoContaItemModel(
                        dataContaBancaria: o.DataDebito!.Value,
                        dataPagamento: o.DataDebito.Value,
                        descricao: o.Descricao,
                        pessoa: null,
                        tipo: TipoLancamento.Despesa,
                        valor: o.Valor)
                    )
                    .ToList();
        }

        private async Task<List<RelatorioExtratoContaItemModel>> ListarTransferenciasContaDestinoRelatorioAsync(int grupoId, int contaBancariaId, DateTime dataInicial, DateTime dataFinal)
        {
            var transferenciasConta = await _transferenciaContaRepository.ListarTransferenciasContaDestinoRelatorioExtratoAsync(grupoId, contaBancariaId, dataInicial, dataFinal);

            return transferenciasConta
                    .Select(o => new RelatorioExtratoContaItemModel(
                        dataContaBancaria: o.DataCredito!.Value,
                        dataPagamento: o.DataCredito.Value,
                        descricao: o.Descricao,
                        pessoa: null,
                        tipo: TipoLancamento.Receita,
                        valor: o.Valor)
                    )
                    .ToList();
        }
    }
}