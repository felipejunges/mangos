using System;

namespace Mangos.Dominio.Entities
{
    public class MetaSaldo
    {
        public int Id { get; set; }

        public int GrupoId { get; set; }

        public DateTime Mes { get; set; }

        public decimal Valor { get; set; }

        public virtual Grupo? Grupo { get; set; }
    }
}