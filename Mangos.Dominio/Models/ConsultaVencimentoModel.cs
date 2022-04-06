using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Mangos.Dominio.Models
{
    public class ConsultaVencimentoModel
    {
        public int Id { get; set; }

        public TipoRegistroConsultaVencimento Tipo { get; set; }

        [Display(Name = "Data vencimento")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy (ddd)}")]
        public DateTime DataVencimento { get; set; }

        [DisplayFormat(DataFormatString = "{0:c2}")]
        public decimal Valor { get; set; }

        public string Pessoa { get; set; }

        [Display(Name = "Descrição")]
        public string Descricao { get; set; }

        public ConsultaVencimentoModel(int id, TipoRegistroConsultaVencimento tipo, DateTime dataVencimento, decimal valor, string pessoa, string descricao)
        {
            Id = id;
            Tipo = tipo;
            DataVencimento = dataVencimento;
            Valor = valor;
            Pessoa = pessoa;
            Descricao = descricao;
        }
    }

    public enum TipoRegistroConsultaVencimento
    {
        Receita = 1,
        Despesa = 2,

        [Description("Cartão de crédito")]
        CartaoCredito = 3
    }
}