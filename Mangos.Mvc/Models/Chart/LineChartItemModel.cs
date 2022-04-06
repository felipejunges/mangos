namespace Mangos.Mvc.Models.Chart
{
    public class LineChartItemModel
    {
        public string Ponto { get; set; }

        public double Valor { get; set; }

        public LineChartItemModel(string ponto, double valor)
        {
            Ponto = ponto;
            Valor = valor;
        }
    }
}