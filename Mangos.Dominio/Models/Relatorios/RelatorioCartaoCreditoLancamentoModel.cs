using System;
using System.ComponentModel.DataAnnotations;

namespace Mangos.Dominio.Models.Relatorios
{
    [Serializable]
    public class RelatorioCartaoCreditoLancamentoModel
    {
        public int CartaoCreditoId { get; set; }

        [Display(Name = "Cartão")]
        public string CartaoCredito { get; set; }

        [Display(Name = "Mês ref.")]
        [DisplayFormat(DataFormatString = "{0:MMM/yyyy}")]
        public DateTime MesReferencia { get; set; }

        [Display(Name = "Valor total")]
        [DisplayFormat(DataFormatString = "{0:c2}")]
        public decimal ValorTotal { get; set; }

        public RelatorioCartaoCreditoLancamentoModel(int cartaoCreditoId, string cartaoCredito, DateTime mesReferencia, decimal valorTotal)
        {
            CartaoCreditoId = cartaoCreditoId;
            CartaoCredito = cartaoCredito;
            MesReferencia = mesReferencia;
            ValorTotal = valorTotal;
        }
    }
}