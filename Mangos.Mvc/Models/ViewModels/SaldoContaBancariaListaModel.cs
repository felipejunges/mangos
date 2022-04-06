using System;
using System.ComponentModel.DataAnnotations;

namespace Mangos.Mvc.Models.ViewModels
{
    public class SaldoContaBancariaListaModel
    {
        public int Id { get; set; }

        public int ContaBancariaId { get; set; }

        [Display(Name = "Conta bancária")]
        public string ContaBancaria { get; set; }

        [Display(Name = "Mês")]
        [DisplayFormat(DataFormatString = "{0:MMM/yyyy}")]
        public DateTime Data { get; set; }

        [Display(Name = "Movimentos")]
        [DisplayFormat(DataFormatString = "{0:c2}")]
        public decimal ValorMovimentacoes { get; set; }

        [Display(Name = "Saldo")]
        [DisplayFormat(DataFormatString = "{0:c2}")]
        public decimal ValorSaldo { get; set; }

        [Display(Name = "Conferência")]
        public string? UltimaConferenciaSaldo { get; set; }

        public decimal? ValorConferenciaSaldo { get; set; }

        public bool Fechado { get; set; }

        public bool SaldoAtualizado { get => ValorConferenciaSaldo != null && ValorConferenciaSaldo.Value == ValorSaldo; }

        public bool PodeFechar { get => SaldoAtualizado && Data < new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1); }

        public SaldoContaBancariaListaModel(int id, int contaBancariaId, string contaBancaria, DateTime data, decimal valorMovimentacoes, decimal valorSaldo, string? ultimaConferenciaSaldo, decimal? valorConferenciaSaldo, bool fechado)
        {
            Id = id;
            ContaBancariaId = contaBancariaId;
            ContaBancaria = contaBancaria;
            Data = data;
            ValorMovimentacoes = valorMovimentacoes;
            ValorSaldo = valorSaldo;
            UltimaConferenciaSaldo = ultimaConferenciaSaldo;
            ValorConferenciaSaldo = valorConferenciaSaldo;
            Fechado = fechado;
        }
    }
}