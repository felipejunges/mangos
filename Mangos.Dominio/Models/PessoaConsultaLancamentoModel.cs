using System;
using System.ComponentModel.DataAnnotations;

namespace Mangos.Dominio.Models
{
    public class PessoaConsultaLancamentoModel
    {
        [Display(Name = "Descrição")]
        public string Descricao { get; set; }

        public string Tipo { get; set; }

        [Display(Name = "Vencimento")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DataVencimento { get; set; }

        [Display(Name = "Pagamento")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? DataPagamento { get; set; }

        [DisplayFormat(DataFormatString = "{0:c2}")]
        public decimal Valor { get; set; }

        public PessoaConsultaLancamentoModel(string descricao, string tipo, DateTime dataVencimento, DateTime? dataPagamento, decimal valor)
        {
            Descricao = descricao;
            Tipo = tipo;
            DataVencimento = dataVencimento;
            DataPagamento = dataPagamento;
            Valor = valor;
        }
    }
}