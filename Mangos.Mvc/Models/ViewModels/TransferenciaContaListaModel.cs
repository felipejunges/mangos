using System;
using System.ComponentModel.DataAnnotations;

namespace Mangos.Mvc.Models.ViewModels
{
    public class TransferenciaContaListaModel
    {
        public int Id { get; set; }

        [DisplayFormat(DataFormatString = "{0:c2}")]
        public decimal Valor { get; set; }

        [Display(Name = "Data débito")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? DataDebito { get; set; }

        [Display(Name = "Data crédito")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? DataCredito { get; set; }

        [Display(Name = "Descrição")]
        public string Descricao { get; set; }

        [Display(Name = "Conta origem")]
        public string ContaBancariaOrigem { get; set; }

        [Display(Name = "Conta destino")]
        public string ContaBancariaDestino { get; set; }

        public TransferenciaContaListaModel(int id, decimal valor, DateTime? dataDebito, DateTime? dataCredito, string descricao, string contaBancariaOrigem, string contaBancariaDestino)
        {
            Id = id;
            Valor = valor;
            DataDebito = dataDebito;
            DataCredito = dataCredito;
            Descricao = descricao;
            ContaBancariaOrigem = contaBancariaOrigem;
            ContaBancariaDestino = contaBancariaDestino;
        }
    }
}