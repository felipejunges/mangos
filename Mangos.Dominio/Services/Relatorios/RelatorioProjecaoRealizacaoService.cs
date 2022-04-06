using Mangos.Dominio.Interfaces.Repository;
using Mangos.Dominio.Enums;
using Mangos.Dominio.Models.Relatorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mangos.Dominio.Services.Relatorios
{
    public class RelatorioProjecaoRealizacaoService
    {
        private readonly ILancamentoRepository _lancamentoRepository;
        private readonly ILancamentoCartaoRepository _lancamentoCartaoRepository;
        private readonly IRendimentoMensalContaRepository _rendimentoMensalContaRepository;

        public RelatorioProjecaoRealizacaoService(ILancamentoRepository lancamentoRepository, ILancamentoCartaoRepository lancamentoCartaoRepository, IRendimentoMensalContaRepository rendimentoMensalContaRepository)
        {
            _lancamentoRepository = lancamentoRepository;
            _lancamentoCartaoRepository = lancamentoCartaoRepository;
            _rendimentoMensalContaRepository = rendimentoMensalContaRepository;
        }

        public async Task<IEnumerable<RelatorioProjecaoRealizacaoModel>> ListarRelatorioProjecaoRealizacaoAsync(int grupoId, DateTime mesInicial, DateTime mesFinal)
        {
            DateTime dataInicial = mesInicial;
            DateTime dataFinal = mesFinal.AddMonths(1).AddDays(-1);

            var lancamentosPendentes = await _lancamentoRepository.ListarLancamentosPendentesRelatorioProjecaoAsync(grupoId, dataInicial, dataFinal);
            var lancamentosPagos = await _lancamentoRepository.ListarLancamentosPagosRelatorioProjecaoAsync(grupoId, dataInicial, dataFinal);
            var lancamentosCartaoPendentes = await _lancamentoCartaoRepository.ListarLancamentosCartaoPendentesRelatorioProjecaoAsync(grupoId, mesInicial, mesFinal);
            var lancamentosCartaoPagos = await _lancamentoCartaoRepository.ListarLancamentosCartaoPagosRelatorioProjecaoAsync(grupoId, mesInicial, mesFinal);
            var rendimentosMensais = await _rendimentoMensalContaRepository.ListarRendimentosMensaisRelatorioProjecaoAsync(grupoId, mesInicial, mesFinal);

            //
            var lancamentosProjecaoModel = lancamentosPendentes
                    .Select(o => new RelatorioProjecaoRealizacaoModel()
                    {
                        Mes = new DateTime(o.DataVencimento.Year, o.DataVencimento.Month, 1),
                        ValorReceitasProjecao = o.Tipo == TipoLancamento.Receita ? o.Valor : 0,
                        ValorReceitasRealizacao = 0,
                        ValorDespesasProjecao = o.Tipo == TipoLancamento.Despesa ? o.Valor : 0,
                        ValorDespesasRealizacao = 0
                    })
                    .ToList();

            //
            var lancamentosRealizacaoModel = lancamentosPagos
                    .Select(o => new RelatorioProjecaoRealizacaoModel()
                    {
                        Mes = new DateTime(o.DataPagamento!.Value.Year, o.DataPagamento.Value.Month, 1),
                        ValorReceitasProjecao = 0,
                        ValorReceitasRealizacao = o.Tipo == TipoLancamento.Receita ? o.ValorPago!.Value : 0,
                        ValorDespesasProjecao = 0,
                        ValorDespesasRealizacao = o.Tipo == TipoLancamento.Despesa ? o.ValorPago!.Value : 0
                    })
                    .ToList();

            //
            var lancamentosCartaoProjecaoModel = lancamentosCartaoPendentes
                    .Select(o => new RelatorioProjecaoRealizacaoModel()
                    {
                        Mes = o.MesReferencia.AddMonths(o.CartaoCredito!.OffsetReferenciaVencimento),
                        ValorReceitasProjecao = o.TipoLancamento == TipoLancamentoCartao.Receita ? o.Valor : 0,
                        ValorReceitasRealizacao = 0,
                        ValorDespesasProjecao = o.TipoLancamento == TipoLancamentoCartao.Despesa ? o.Valor : 0,
                        ValorDespesasRealizacao = 0
                    })
                    .ToList();

            //
            var lancamentosCartaoRealizadoModel = lancamentosCartaoPagos
                    .Select(o => new RelatorioProjecaoRealizacaoModel()
                    {
                        Mes = o.MesReferencia.AddMonths(o.CartaoCredito!.OffsetReferenciaVencimento),
                        ValorReceitasProjecao = 0,
                        ValorReceitasRealizacao = o.TipoLancamento == TipoLancamentoCartao.Receita ? o.Valor : 0,
                        ValorDespesasProjecao = 0,
                        ValorDespesasRealizacao = o.TipoLancamento == TipoLancamentoCartao.Despesa ? o.Valor : 0
                    })
                    .ToList();

            //
            var rendimentosMensaisModel = rendimentosMensais
                    .Select(o => new RelatorioProjecaoRealizacaoModel()
                    {
                        Mes = o.MesReferencia,
                        ValorReceitasProjecao = 0,
                        ValorReceitasRealizacao = o.Valor,
                        ValorDespesasProjecao = 0,
                        ValorDespesasRealizacao = 0
                    })
                    .ToList();

            //
            var dadosList = lancamentosProjecaoModel
                                .Concat(lancamentosRealizacaoModel)
                                .Concat(lancamentosCartaoProjecaoModel)
                                .Concat(lancamentosCartaoRealizadoModel)
                                .Concat(rendimentosMensaisModel)
                                .ToList();

            //
            var relatorio = dadosList
                    .GroupBy(o => new
                    {
                        o.Mes
                    })
                    .OrderBy(o => o.Key.Mes)
                    .Select(o => new RelatorioProjecaoRealizacaoModel()
                    {
                        Mes = o.Key.Mes,
                        ValorReceitasProjecao = o.Sum(s => s.ValorReceitasProjecao),
                        ValorReceitasRealizacao = o.Sum(s => s.ValorReceitasRealizacao),
                        ValorDespesasProjecao = o.Sum(s => s.ValorDespesasProjecao),
                        ValorDespesasRealizacao = o.Sum(s => s.ValorDespesasRealizacao)
                    });

            return relatorio;
        }
    }
}