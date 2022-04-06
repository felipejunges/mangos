using System;
using System.ComponentModel.DataAnnotations;

namespace Mangos.Dominio.Models.Relatorios
{
    [Serializable]
    public class RelatorioCartaoCreditoLimiteModel
    {
        public int CartaoCreditoId { get; set; }

        [Display(Name = "Cartão")]
        public string CartaoCredito { get; set; }

        [DisplayFormat(DataFormatString = "{0:c2}")]
        [Display(Name = "Limite total")]
        public decimal ValorLimiteTotal { get; set; }

        [DisplayFormat(DataFormatString = "{0:c2}")]
        [Display(Name = "Utilizado")]
        public decimal ValorLimiteUtilizado { get; set; }


        [DisplayFormat(DataFormatString = "{0:c2}")]
        [Display(Name = "Disponível")]
        public decimal ValorLimiteDisponivel => ValorLimiteUtilizado >= ValorLimiteTotal ? 0
                                                    : ValorLimiteUtilizado < 0 ? 100
                                                    : ValorLimiteTotal - ValorLimiteUtilizado;

        [DisplayFormat(DataFormatString = "{0:f0}")]
        [Display(Name = "% utilizado")]
        public decimal PercentualLimiteUtilizado => ValorLimiteUtilizado * 100 / ValorLimiteTotal;

        public RelatorioCartaoCreditoLimiteModel(int cartaoCreditoId, string cartaoCredito, decimal valorLimiteTotal, decimal valorLimiteUtilizado)
        {
            CartaoCreditoId = cartaoCreditoId;
            CartaoCredito = cartaoCredito;
            ValorLimiteTotal = valorLimiteTotal;
            ValorLimiteUtilizado = valorLimiteUtilizado;
        }
    }
}