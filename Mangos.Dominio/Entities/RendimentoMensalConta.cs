using System;

namespace Mangos.Dominio.Entities
{
    public class RendimentoMensalConta
    {
        public int Id { get; set; }

        public DateTime DataHoraCadastro { get; set; }

        public int ContaBancariaId { get; set; }

        public DateTime MesReferencia { get; set; }

        public decimal Valor { get; set; }

        public virtual ContaBancaria? ContaBancaria { get; set; }
    }
}