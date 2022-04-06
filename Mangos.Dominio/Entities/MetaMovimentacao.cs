using System;

namespace Mangos.Dominio.Entities
{
    public class MetaMovimentacao
    {
        public int Id { get; set; }

        public int GrupoId { get; set; }

        public decimal ValorMensal { get; set; }

        public DateTime MesInicial { get; set; }

        public DateTime MesFinal { get; set; }

        public virtual Grupo? Grupo { get; set; }
    }
}