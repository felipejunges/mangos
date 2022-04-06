using System.Collections.Generic;

namespace Mangos.Dominio.Models.Relatorios
{
    public class RelatorioExtratoContaModel
    {
        public decimal ValorSaldoInicial { get; set; }

        public List<RelatorioExtratoContaItemModel> Itens { get; set; }

        public RelatorioExtratoContaModel()
        {
            this.Itens = new List<RelatorioExtratoContaItemModel>();
        }
    }
}