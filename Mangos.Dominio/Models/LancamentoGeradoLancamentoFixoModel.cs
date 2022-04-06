using System;
using System.ComponentModel.DataAnnotations;

namespace Mangos.Dominio.Models
{
    public class LancamentoGeradoLancamentoFixoModel
    {
        public int Id { get; set; }

        public string? Tipo { get; set; }

        [Display(Name = "Descrição")]
        public string? Descricao { get; set; }

        public string? Pessoa { get; set; }

        [DisplayFormat(DataFormatString = "{0:c2}")]
        public decimal Valor { get; set; }

        [Display(Name = "Vencimento")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DataVencimento { get; set; }

        public LancamentoGeradoLancamentoFixoModel(int id, string? tipo, string? descricao, string? pessoa, decimal valor, DateTime dataVencimento)
        {
            Id = id;
            Tipo = tipo;
            Descricao = descricao;
            Pessoa = pessoa;
            Valor = valor;
            DataVencimento = dataVencimento;
        }
    }
}