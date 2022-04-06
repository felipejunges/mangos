using Mangos.Dominio.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace Mangos.Dominio.Models.Relatorios
{
    public class RelatorioExtratoContaItemModel
    {
        [Display(Name = "Data")]
        [DisplayFormat(DataFormatString = "{0:dd/MMM/yyyy}")]
        public DateTime DataContaBancaria { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MMM}")]
        public DateTime DataPagamento { get; set; }

        [Display(Name = "Descrição")]
        public string Descricao { get; set; }

        public string? Pessoa { get; set; }

        public TipoLancamento Tipo { get; set; }

        [Display(Name = "Valor")]
        [DisplayFormat(DataFormatString = "{0:c2}")]
        public decimal Valor { get; set; }

        public RelatorioExtratoContaItemModel(DateTime dataContaBancaria, DateTime dataPagamento, string descricao, string? pessoa, TipoLancamento tipo, decimal valor)
        {
            DataContaBancaria = dataContaBancaria;
            DataPagamento = dataPagamento;
            Descricao = descricao;
            Pessoa = pessoa;
            Tipo = tipo;
            Valor = valor;
        }
    }
}