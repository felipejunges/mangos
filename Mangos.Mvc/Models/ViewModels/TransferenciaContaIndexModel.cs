using System;
using System.Collections.Generic;

namespace Mangos.Mvc.Models.ViewModels
{
    public class TransferenciaContaIndexModel
    {
        public string Data { get; set; }

        public string DataInicial { get; set; }

        public string DataFinal { get; set; }

        public IEnumerable<TransferenciaContaListaModel> Itens { get; set; }

        public TransferenciaContaIndexModel(DateTime dataInicial, DateTime dataFinal, IEnumerable<TransferenciaContaListaModel> itens)
        {
            Data = dataInicial.ToString("MM/yyyy");
            DataInicial = dataInicial.ToString("dd/MM/yyyy");
            DataFinal = dataFinal.ToString("dd/MM/yyyy");
            Itens = itens;
        }
    }
}