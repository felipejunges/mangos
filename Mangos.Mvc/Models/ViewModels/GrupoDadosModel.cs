using System.ComponentModel.DataAnnotations;

namespace Mangos.Mvc.Models.ViewModels
{
    public class GrupoDadosModel
    {
        public int Id { get; set; }

        [Display(Name = "Descrição")]
        public string? Descricao { get; set; }

        [Display(Name = "Antecedência lançamento")]
        public int MesesAntecedenciaGerarLancamento { get; set; }

        [Display(Name = "Antecedência lançamento cartão")]
        public int MesesAntecedenciaGerarLancamentoCartao { get; set; }

        [Display(Name = "Meses gráficos dashboard")]
        public int MesesGraficosDashboard { get; set; }

        public GrupoDadosModel()
        {
        }

        public GrupoDadosModel(int id, string? descricao, int mesesAntecedenciaGerarLancamento, int mesesAntecedenciaGerarLancamentoCartao, int mesesGraficosDashboard)
        {
            Id = id;
            Descricao = descricao;
            MesesAntecedenciaGerarLancamento = mesesAntecedenciaGerarLancamento;
            MesesAntecedenciaGerarLancamentoCartao = mesesAntecedenciaGerarLancamentoCartao;
            MesesGraficosDashboard = mesesGraficosDashboard;
        }

        public static GrupoDadosModel Novo()
        {
            return new GrupoDadosModel(
                id: 0,
                descricao: null,
                mesesAntecedenciaGerarLancamento: 6,
                mesesAntecedenciaGerarLancamentoCartao: 0,
                mesesGraficosDashboard: 12
            );
        }
    }
}