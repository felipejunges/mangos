using Mangos.Mvc.Models.ViewModels.Abstract;
using System;
using System.ComponentModel.DataAnnotations;

namespace Mangos.Mvc.Models.ViewModels
{
    public class MetaSaldoDadosModel : HashedModel
    {
        [Display(Name = "Mês")]
        [DisplayFormat(DataFormatString = "{0:MMM/yyyy}")]
        public DateTime? Mes { get; set; }

        [Display(Name = "Valor")]
        [DisplayFormat(DataFormatString = "{0:c2}")]
        public decimal? Valor { get; set; }

        public MetaSaldoDadosModel()
        {
        }

        public MetaSaldoDadosModel(int id, int grupoId, DateTime? mes, decimal? valor)
        {
            Id = id;
            GrupoId = grupoId;
            Mes = mes;
            Valor = valor;

            SetValidationHash();
        }

        public static MetaSaldoDadosModel Novo(int grupoId)
        {
            return new MetaSaldoDadosModel(
                id: 0,
                grupoId: grupoId,
                mes: null,
                valor: null
            );
        }
    }
}