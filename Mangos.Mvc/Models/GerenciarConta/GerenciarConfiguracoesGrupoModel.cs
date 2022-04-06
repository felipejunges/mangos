using System.ComponentModel.DataAnnotations;

namespace Mangos.Mvc.Models.GerenciarConta
{
    public class GerenciarConfiguracoesGrupoModel
    {
        [Display(Name = "Antecedência lançamento")]
        public int MesesAntecedenciaGerarLancamento { get; set; }

        [Display(Name = "Antecedência lançamento cartão")]
        public int MesesAntecedenciaGerarLancamentoCartao { get; set; }

        [Display(Name = "Meses gráficos dashboard")]
        public int MesesGraficosDashboard { get; set; }

        public bool Sucesso { get; set; }

        public GerenciarConfiguracoesGrupoModel()
        {
        }

        public GerenciarConfiguracoesGrupoModel(int mesesAntecedenciaGerarLancamento, int mesesAntecedenciaGerarLancamentoCartao, int mesesGraficosDashboard)
        {
            MesesAntecedenciaGerarLancamento = mesesAntecedenciaGerarLancamento;
            MesesAntecedenciaGerarLancamentoCartao = mesesAntecedenciaGerarLancamentoCartao;
            MesesGraficosDashboard = mesesGraficosDashboard;
            Sucesso = false;
        }
    }
}