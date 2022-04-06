using System;

namespace Mangos.Mvc.Models.ViewModels
{
    public class ReaberturaMesCartaoModel
    {
        public int GrupoId { get; set; }

        public int? CartaoCreditoId { get; set; }

        public DateTime? MesReferencia { get; set; }
    }
}