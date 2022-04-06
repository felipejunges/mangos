using ChartJSCore.Helpers;
using ChartJSCore.Models;
using Mangos.Mvc.Models.Chart;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mangos.Mvc.Services
{
    public class ChartService
    {
        public Chart CriarLineChart(List<LineChartItemModel> itens, string tituloDataset, bool calcularMedia, bool preencherFundo, bool iniciarEmZero)
        {
            var chart = new Chart() { Type = Enums.ChartType.Line };
            var pontos = itens.Select(o => o.Ponto).ToArray();
            var valores = itens.Select(o => (double?)o.Valor).ToArray();

            var datasetValores = CriarDefaultLineDataset(tituloDataset, valores, 2, 1, ChartColor.FromRgb(75, 192, 192), preencherFundo);

            chart.Data = new Data()
            {
                Labels = pontos,
                Datasets = new List<Dataset> { datasetValores }
            };

            if (iniciarEmZero)
            {
                chart.Options = new Options()
                {
                    Scales = new Scales()
                    {
                        YAxes = new List<Scale>()
                        {
                            new CartesianScale
                            {
                                Ticks = new CartesianLinearTick
                                {
                                    BeginAtZero = true
                                }
                            }
                        }
                    }
                };
            }

            if (calcularMedia)
            {
                // obtém a média, tratando apenas os valores não nulos do array
                var valorMedia = valores.Count() > 0 ? Math.Round(valores.Where(v => v is not null).Select(v => v!.Value).Average(), 2) : 0;

                var annotations = new
                {
                    annotations = new[] {
                        //new { type = "line", mode = "vertical", scaleID = "x-axis-0", value = "out/2019", borderColor = "red", borderDash = new int[] { 2, 3 } },
                        new { type = "line", mode = "horizontal", scaleID = "y-axis-0", value = valorMedia, borderColor = "red", borderDash = new int[] { 2, 3 } },
                    }
                };

                chart.Options.PluginDynamic = new Dictionary<string, object>
                {
                    { "annotation", annotations }
                };
            }

            return chart;
        }

        public Chart CriarChartPizza(List<PizzaChartItemModel> itens)
        {
            var chart = new Chart() { Type = Enums.ChartType.Doughnut };
            chart.Options = new Options()
            {
                Legend = new Legend()
                {
                    Position = "right"
                }
            };

            var tipos = itens.Select(o => o.Tipo).ToArray();
            var valores = itens.Select(o => (double?)o.Valor).ToArray();

            var dataset = new PieDataset()
            {
                Label = "Contatos por mês",
                Data = valores,
                BackgroundColor = new ChartColor[] {
                    ChartColor.FromRgba(75, 192, 192, 0.4),
                    ChartColor.FromRgba(192, 75, 192, 0.4),
                    ChartColor.FromRgba(192, 192, 75, 0.4),
                    ChartColor.FromRgba(75, 75, 192, 0.4),
                    ChartColor.FromRgba(75, 192, 75, 0.4),
                    ChartColor.FromRgba(192, 75, 75, 0.4),
                },
            };

            chart.Data = new Data()
            {
                Labels = tipos,
                Datasets = new List<Dataset> { dataset }
            };

            return chart;
        }

        private LineDataset CriarDefaultLineDataset(string titulo, IList<double?> valores, int borderWidth, int pointRadius, ChartColor cor, bool preencherFundo)
        {
            var lineDataSet = new LineDataset()
            {
                Label = titulo,
                Data = valores,
                Fill = preencherFundo.ToString(),
                LineTension = 0.2,
                BackgroundColor = ChartColor.FromRgba(cor.Red, cor.Green, cor.Blue, 0.4),
                BorderColor = cor,
                BorderCapStyle = "butt",
                BorderDash = new List<int> { },
                BorderDashOffset = 0.0,
                BorderJoinStyle = "miter",
                BorderWidth = borderWidth,
                PointBorderColor = new List<ChartColor> { cor },
                PointBackgroundColor = new List<ChartColor> { ChartColor.FromHexString("#ffffff") },
                PointBorderWidth = new List<int> { 1 },
                PointHoverRadius = new List<int> { 5 },
                PointHoverBackgroundColor = new List<ChartColor> { cor },
                PointHoverBorderColor = new List<ChartColor> { ChartColor.FromRgb(220, 220, 220) },
                PointHoverBorderWidth = new List<int> { 2 },
                PointRadius = new List<int> { pointRadius },
                PointHitRadius = new List<int> { 10 },
                SpanGaps = false
            };

            return lineDataSet;
        }
    }
}