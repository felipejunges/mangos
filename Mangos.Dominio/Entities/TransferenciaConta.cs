using System;

namespace Mangos.Dominio.Entities
{
    public class TransferenciaConta
    {
        public int Id { get; set; }

        public int GrupoId { get; set; }

        public DateTime DataHoraCadastro { get; set; }

        public decimal Valor { get; set; }

        public DateTime? DataDebito { get; set; }

        public DateTime? DataCredito { get; set; }

        public string Descricao { get; set; }

        public int? ContaBancariaOrigemId { get; set; }

        public int? ContaBancariaDestinoId { get; set; }

        public virtual ContaBancaria? ContaBancariaDestino { get; set; }

        public virtual ContaBancaria? ContaBancariaOrigem { get; set; }

        public virtual Grupo? Grupo { get; set; }

        public TransferenciaConta()
        {
            Descricao = string.Empty;
        }
    }
}