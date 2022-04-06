using ChartJSCore.Models;
using Mangos.Dominio.Interfaces.Repository;
using Mangos.Dominio.Services;
using Mangos.Dominio.Services.Relatorios;
using Mangos.Mvc.Models.Chart;
using Mangos.Mvc.Models.Relatorios;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Mangos.Mvc.Services
{
    public class DashboardService
    {
        private readonly ChartService _chartService;
        private readonly GrupoService _grupoService;
        private readonly IGrupoRepository _grupoRepository;
        private readonly RelatorioProjecaoRealizacaoService _relatorioProjecaoRealizacaoService;
        private readonly RelatorioCategoriaService _relatorioCategoriaService;
        private readonly ISaldoContaBancariaRepository _saldoContaBancariaRepository;

        private const int QUANTIDADE_ITENS_CORTE_CATEGORIAS = 5;

        public DashboardService(ChartService chartService, GrupoService grupoService, IGrupoRepository grupoRepository, RelatorioProjecaoRealizacaoService relatorioProjecaoRealizacaoService, RelatorioCategoriaService relatorioCategoriaService, ISaldoContaBancariaRepository saldoContaBancariaRepository)
        {
            _chartService = chartService;
            _grupoService = grupoService;
            _grupoRepository = grupoRepository;
            _relatorioProjecaoRealizacaoService = relatorioProjecaoRealizacaoService;
            _relatorioCategoriaService = relatorioCategoriaService;
            _saldoContaBancariaRepository = saldoContaBancariaRepository;
        }

        public async Task<Chart> CriarChartResultadosMeses(int grupoId)
        {
            var grupo = await _grupoRepository.ObterGrupoAsync(grupoId);
            int quantidadeMeses = grupo?.MesesGraficosDashboard ?? 0;

            var mesFinal = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var mesInicial = mesFinal.AddMonths((quantidadeMeses - 1) * -1);

            var dadosChart = await _relatorioProjecaoRealizacaoService.ListarRelatorioProjecaoRealizacaoAsync(grupoId, mesInicial, mesFinal);
            var chartItems = dadosChart.Select(o => new LineChartItemModel(
                ponto: o.Mes.ToString("MMM/yyyy"),
                valor: (double)o.ValorTotal)
            ).ToList();

            return _chartService.CriarLineChart(chartItems, "Valor no mês", true, false, false);
        }

        public async Task<Chart> CriarChartSaldosConta(int grupoId)
        {
            var grupo = await _grupoRepository.ObterGrupoAsync(grupoId);
            int quantidadeMeses = grupo?.MesesGraficosDashboard ?? 0;

            var mesFinal = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var mesInicial = mesFinal.AddMonths((quantidadeMeses - 1) * -1);

            var saldosConta = await _saldoContaBancariaRepository.ListarSaldosContasBancariasAsync(grupoId, null, mesInicial, mesFinal);
            var dadosChart = saldosConta
                    .GroupBy(s => new
                    {
                        s.Data
                    })
                    .OrderBy(o => o.Key.Data).Select(o => new RelatorioSaldoContaModel()
                    {
                        Data = o.Key.Data,
                        Valor = o.Sum(s => s.ValorSaldo),
                    }).ToList();

            var chartItems = dadosChart.Select(o => new LineChartItemModel(
                ponto: o.Data.ToString("MMM/yyyy"),
                valor: (double)o.Valor)
            ).ToList();

            return _chartService.CriarLineChart(chartItems, "Saldo no mês", false, true, true);
        }

        public async Task<Chart> CriarChartTotaisCategoria(int grupoId)
        {
            var grupo = await _grupoRepository.ObterGrupoAsync(grupoId);
            int quantidadeMeses = grupo?.MesesGraficosDashboard ?? 0;

            var mesFinal = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var mesInicial = mesFinal.AddMonths((quantidadeMeses - 1) * -1);

            var dadosChart = await _relatorioCategoriaService.ListarRelatorioCategoriaMensalAsync(grupoId, mesInicial, mesFinal, "D", "R", "TD", true);
            var chartItens = dadosChart.Select(o => new PizzaChartItemModel(
                tipo: o.Categoria,
                valor: (double)o.ValorTotal
            )).ToList();

            // converte para um decimal para fazer a soma, pois segundo pesquisa no Google, soma de double não é precisa...
            if (chartItens.Count > QUANTIDADE_ITENS_CORTE_CATEGORIAS)
            {
                var totalAposCorte = chartItens.Skip(QUANTIDADE_ITENS_CORTE_CATEGORIAS).Sum(s => (decimal)s.Valor);

                chartItens.RemoveRange(QUANTIDADE_ITENS_CORTE_CATEGORIAS, chartItens.Count - QUANTIDADE_ITENS_CORTE_CATEGORIAS);
                chartItens.Add(new PizzaChartItemModel(tipo: "Outros", valor: (double)totalAposCorte));
            }

            return _chartService.CriarChartPizza(chartItens);
        }
    }
}