namespace Mangos.Mvc.Models.Chart
{
    public class PizzaChartItemModel
    {
        public string Tipo { get; set; }

        public double Valor { get; set; }

        public PizzaChartItemModel(string tipo, double valor)
        {
            Tipo = tipo;
            Valor = valor;
        }
    }
}