using System;
using System.ComponentModel.DataAnnotations;

namespace Mangos.Mvc.Models.ViewModels
{
    public class RendimentoMensalContaListaModel
    {
        public int Id { get; set; }

        [Display(Name = "Conta bancária")]
        public string ContaBancaria { get; set; }

        [Display(Name = "Mês referência")]
        [DisplayFormat(DataFormatString = "{0:MMM/yyyy}")]
        public DateTime MesReferencia { get; set; }

        [DisplayFormat(DataFormatString = "{0:c2}")]
        public decimal Valor { get; set; }

        public RendimentoMensalContaListaModel(int id, string contaBancaria, DateTime mesReferencia, decimal valor)
        {
            Id = id;
            ContaBancaria = contaBancaria;
            MesReferencia = mesReferencia;
            Valor = valor;
        }
    }
}