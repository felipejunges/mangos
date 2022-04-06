using Mangos.Dominio.Entities;
using System.ComponentModel.DataAnnotations;

namespace Mangos.Mvc.Models.ViewModels
{
    public class ContaBancariaListaModel
    {
        public int Id { get; set; }

        [Display(Name = "Descrição")]
        public string Descricao { get; set; }

        [Display(Name = "Banco")]
        public string? NumeroBanco { get; set; }

        [Display(Name = "Agência")]
        public string? Agencia { get; set; }

        [Display(Name = "Conta")]
        public string? NumeroConta { get; set; }

        public bool Ativo { get; set; }

        [Display(Name = "Saldo inicial")]
        [DisplayFormat(DataFormatString = "{0:c2}")]
        public decimal SaldoInicial { get; set; }

        public ContaBancariaListaModel(int id, string descricao, string? numeroBanco, string? agencia, string? numeroConta, bool ativo, decimal saldoInicial)
        {
            Id = id;
            Descricao = descricao;
            NumeroBanco = numeroBanco;
            Agencia = agencia;
            NumeroConta = numeroConta;
            Ativo = ativo;
            SaldoInicial = saldoInicial;
        }
    }
}