using Mangos.Dominio.Interfaces.Repository;
using Mangos.Dominio.Enums;
using Mangos.Dominio.Models.Relatorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mangos.Dominio.Services.Relatorios
{
    public class RelatorioProjecaoSaldoService
    {
        private readonly ILancamentoRepository _lancamentoRepository;
        private readonly ILancamentoCartaoRepository _lancamentoCartaoRepository;
        private readonly ISaldoContaBancariaRepository _saldoContaBancariaRepository;

        public RelatorioProjecaoSaldoService(ILancamentoRepository lancamentoRepository, ILancamentoCartaoRepository lancamentoCartaoRepository, ISaldoContaBancariaRepository saldoContaBancariaRepository)
        {
            _lancamentoRepository = lancamentoRepository;
            _lancamentoCartaoRepository = lancamentoCartaoRepository;
            _saldoContaBancariaRepository = saldoContaBancariaRepository;
        }

        public async Task<List<RelatorioProjecaoSaldoModel>> ListarRelatorioProjecaoSaldoAsync(int grupoId, DateTime dataFinal, decimal valorInicial)
        {
            DateTime dataMes = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            var saldosContas = await _saldoContaBancariaRepository.ListarSaldosContaRelatorioProjecaoSaldoAsync(grupoId, dataMes);
            var lancamentosAtrasados = await _lancamentoRepository.ListarLancamentosAtrasadosRelatorioProjecaoSaldo(grupoId);
            var lancamentosPendentes = await _lancamentoRepository.ListarLancamentosPendentesRelatorioProjecaoSaldo(grupoId, dataFinal);
            var lancamentosCartaoAtrasados = await _lancamentoCartaoRepository.ListarLancamentosCartaoAtrasadosRelatorioProjecaoSaldo(grupoId);
            var lancamentosCartaoPendentes = await _lancamentoCartaoRepository.ListarLancamentosCartaoPendentesRelatorioProjecaoSaldo(grupoId, dataFinal);

            var contasBancariasModel = saldosContas
                    .Select(o => new RelatorioProjecaoSaldoModel(
                        ordem: 1,
                        data: dataMes,
                        descricao: "Conta bancária " + o.ContaBancaria!.Descricao,
                        valor: o.ValorSaldo,
                        tipoValor: TipoLancamento.Receita)
                    )
                    .ToList();

            var lancamentosAtrasadosModel = lancamentosAtrasados
                    .GroupBy(o => o.Tipo)
                    .Select(o => new RelatorioProjecaoSaldoModel(
                        ordem: 3,
                        data: DateTime.Now.Date,
                        descricao: "Lançamentos atrasados",
                        valor: o.Sum(s => s.Valor),
                        tipoValor: o.Key)
                    )
                    .ToList();

            var lancamentosPendentesModel = lancamentosPendentes
                    .Select(o => new RelatorioProjecaoSaldoModel(
                        ordem: 4,
                        data: o.DataVencimento,
                        descricao: o.DescricaoComParcelas,
                        valor: o.Valor,
                        tipoValor: o.Tipo)
                    )
                    .ToList();

            var lancamentosCartaoAtrasadosModel = lancamentosCartaoAtrasados
                    .GroupBy(o => o.CartaoCredito!)
                    .Select(o => new RelatorioProjecaoSaldoModel(
                        ordem: 3,
                        data: DateTime.Now.Date,
                        descricao: $"Cartão de crédito {o.Key.Descricao} (não gerado - atrasado)",
                        valor: o.Sum(s => s.Valor),
                        tipoValor: TipoLancamento.Despesa)
                    )
                    .ToList();

            var lancamentosCartaoModel = lancamentosCartaoPendentes
                    .GroupBy(o => new
                    {
                        o.MesReferencia,
                        CartaoCredito = o.CartaoCredito!
                    })
                    .Select(o => new RelatorioProjecaoSaldoModel(
                        ordem: 4,
                        data: o.Key.MesReferencia.AddMonths(o.Key.CartaoCredito.OffsetReferenciaVencimento).AddDays(o.Key.CartaoCredito.DiaVencimento - 1),
                        descricao: $"Cartão de crédito {o.Key.CartaoCredito.Descricao} (não gerado)",
                        valor: o.Sum(s => s.Valor),
                        tipoValor: TipoLancamento.Despesa)
                    )
                    .ToList();

            //
            var relatorio = contasBancariasModel
                                .Concat(lancamentosPendentesModel)
                                .Concat(lancamentosAtrasadosModel)
                                .Concat(lancamentosCartaoModel)
                                .Concat(lancamentosCartaoAtrasadosModel)
                                .ToList();

            if (valorInicial != 0)
                relatorio.Add(new RelatorioProjecaoSaldoModel(
                    ordem: 2,
                    data: DateTime.Now.Date,
                    descricao: "Valor inicial",
                    valor: valorInicial,
                    tipoValor: TipoLancamento.Receita)
                );

            relatorio.Sort(
                (o, i) => o.Ordem != i.Ordem ? o.Ordem.CompareTo(i.Ordem) 
                        : o.Data != i.Data ? o.Data.CompareTo(i.Data)
                        : o.Descricao.CompareTo(i.Descricao)
            );

            return relatorio;
        }
    }
}