namespace Mangos.Mvc.Models.ViewModels
{
    public class DashboardViewModel
    {
        public ChartJSCore.Models.Chart Chart1 { get; set; }
        public ChartJSCore.Models.Chart Chart2 { get; set; }
        public ChartJSCore.Models.Chart Chart3 { get; set; }

        public DashboardViewModel(ChartJSCore.Models.Chart chart1, ChartJSCore.Models.Chart chart2, ChartJSCore.Models.Chart chart3)
        {
            Chart1 = chart1;
            Chart2 = chart2;
            Chart3 = chart3;
        }
    }
}