using System;

namespace Mangos.Dominio.Entities
{
    public class SaldoContaBancaria
    {
        public int Id { get; set; }

        public int ContaBancariaId { get; set; }

        public DateTime Data { get; set; }

        public decimal ValorMovimentacoes { get; set; }

        public decimal ValorSaldo { get; set; }

        public DateTime? DataHoraConferenciaSaldo { get; set; }

        public decimal? ValorConferenciaSaldo { get; set; }

        public bool Fechado { get; set; }

        public DateTime? DataHoraFechamento { get; set; }

        public virtual ContaBancaria? ContaBancaria { get; set; }
    }
}