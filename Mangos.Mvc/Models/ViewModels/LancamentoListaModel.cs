using System;
using System.ComponentModel.DataAnnotations;

namespace Mangos.Mvc.Models.ViewModels
{
    public class LancamentoListaModel
    {
        public int Id { get; set; }

        [Display(Name = "Descrição")]
        public string Descricao { get; set; }

        public string? Pessoa { get; set; }

        public string? Categoria { get; set; }

        [Display(Name = "Conta bancária")]
        public string? ContaBancaria { get; set; }

        [Display(Name = "Vencimento")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DataVencimento { get; set; }

        [Display(Name = "Pagamento")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? DataPagamento { get; set; }

        [DisplayFormat(DataFormatString = "{0:c2}")]
        public decimal Valor { get; set; }

        public Guid? Agrupador { get; set; }

        public bool Pago { get; set; }

        public bool TemAgrupador => Agrupador.HasValue;
        
        public LancamentoListaModel(int id, string descricao, string? pessoa, string? categoria, string? contaBancaria, DateTime dataVencimento, DateTime? dataPagamento, decimal valor, Guid? agrupador, bool pago)
        {
            Id = id;
            Descricao = descricao;
            Pessoa = pessoa;
            Categoria = categoria;
            ContaBancaria = contaBancaria;
            DataVencimento = dataVencimento;
            DataPagamento = dataPagamento;
            Valor = valor;
            Agrupador = agrupador;
            Pago = pago;
        }
    }
}