using Mangos.Dominio.Entities;
using System;
using System.Collections.Generic;

namespace Mangos.Mvc.Models.ViewModels
{
    public class RendimentoMensalContaIndexModel
    {
        public string MesReferencia { get; set; }

        public IEnumerable<ContaBancaria> ContasBancarias { get; set; }

        public IEnumerable<RendimentoMensalContaListaModel> Itens { get; set; }

        public RendimentoMensalContaIndexModel(DateTime mesReferencia, IEnumerable<ContaBancaria> contasBancarias, IEnumerable<RendimentoMensalContaListaModel> itens)
        {
            MesReferencia = mesReferencia.ToString("MM/yyyy");
            ContasBancarias = contasBancarias;
            Itens = itens;
        }
    }
}