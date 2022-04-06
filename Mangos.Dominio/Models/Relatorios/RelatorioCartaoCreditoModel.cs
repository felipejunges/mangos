using System.Collections.Generic;

namespace Mangos.Dominio.Models.Relatorios
{
    public class RelatorioCartaoCreditoModel
    {
        public IEnumerable<RelatorioCartaoCreditoLancamentoModel> Lancamentos { get; private set; }

        public IEnumerable<RelatorioCartaoCreditoLimiteModel> Limites { get; private set; }

        public RelatorioCartaoCreditoModel(IEnumerable<RelatorioCartaoCreditoLancamentoModel> lancamentos, IEnumerable<RelatorioCartaoCreditoLimiteModel> limites)
        {
            Lancamentos = lancamentos;
            Limites = limites;
        }
    }
}