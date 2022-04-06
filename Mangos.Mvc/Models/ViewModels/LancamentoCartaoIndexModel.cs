using System;
using System.Collections.Generic;

namespace Mangos.Mvc.Models.ViewModels
{
    public class LancamentoCartaoIndexModel
    {
        public string MesReferencia { get; set; }

        public string MesReferenciaInicial { get; set; }

        public string MesReferenciaFinal { get; set; }

        public IEnumerable<CartaoCreditoListaModel> CartoesCredito { get; set; }

        public IEnumerable<LancamentoCartaoListaModel> Itens { get; set; }

        public LancamentoCartaoIndexModel(DateTime mesReferencia, IEnumerable<CartaoCreditoListaModel> cartoesCredito, IEnumerable<LancamentoCartaoListaModel> itens)
        {
            MesReferencia = mesReferencia.ToString("MM/yyyy");
            MesReferenciaInicial = mesReferencia.ToString("MM/yyyy");
            MesReferenciaFinal = mesReferencia.ToString("MM/yyyy");

            CartoesCredito = cartoesCredito;

            Itens = itens;
        }
    }
}