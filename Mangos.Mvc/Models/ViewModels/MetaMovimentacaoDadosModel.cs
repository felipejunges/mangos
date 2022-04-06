using Mangos.Mvc.Models.ViewModels.Abstract;
using System;
using System.ComponentModel.DataAnnotations;

namespace Mangos.Mvc.Models.ViewModels
{
    public class MetaMovimentacaoDadosModel : HashedModel
    {
        [Display(Name = "Valor mensal")]
        public decimal? ValorMensal { get; set; }

        [Display(Name = "Mês inicial")]
        [DataType(DataType.Text)]
        public DateTime? MesInicial { get; set; }

        [Display(Name = "Mês final")]
        [DataType(DataType.Text)]
        public DateTime? MesFinal { get; set; }

        public MetaMovimentacaoDadosModel()
        {
        }

        public MetaMovimentacaoDadosModel(int id, int grupoId, decimal valorMensal, DateTime? mesInicial, DateTime? mesFinal)
        {
            Id = id;
            GrupoId = grupoId;
            ValorMensal = valorMensal;
            MesInicial = mesInicial;
            MesFinal = mesFinal;

            SetValidationHash();
        }

        public static MetaMovimentacaoDadosModel Novo(int grupoId)
        {
            return new MetaMovimentacaoDadosModel(
                id: 0,
                grupoId: grupoId,
                valorMensal: 0,
                mesInicial: null,
                mesFinal: null
            );
        }
    }
}