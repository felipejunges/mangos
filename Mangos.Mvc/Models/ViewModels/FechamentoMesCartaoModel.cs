using System;
using System.ComponentModel.DataAnnotations;

namespace Mangos.Mvc.Models.ViewModels
{
    public class FechamentoMesCartaoModel
    {
        public int GrupoId { get; set; }

        public int? CartaoCreditoId { get; set; }

        public int? CategoriaId { get; set; }

        public DateTime? MesReferencia { get; set; }

        [Display(Name = "Gerar lançamento de despesa")]
        public bool GerarLancamento { get; set; }
    }
}