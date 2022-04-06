using System.Collections.Generic;

namespace Mangos.Dominio.Entities
{
    public class ContaBancaria
    {
        public int Id { get; set; }

        public int GrupoId { get; set; }

        public string Descricao { get; set; }

        public string? NumeroBanco { get; set; }

        public string? Agencia { get; set; }

        public string? NumeroConta { get; set; }

        public decimal SaldoInicial { get; set; }

        public int? CategoriaRendimentoMensalId { get; set; }

        public bool RelatorioProjecaoSaldo { get; set; }

        public bool PularFinaisSemanaLancamentoRapido { get; set; }

        public bool Ativo { get; set; }

        public virtual Grupo? Grupo { get; set; }

        public virtual Categoria? CategoriaRendimentoMensal { get; set; }

        public virtual ICollection<Lancamento> Lancamentos { get; set; }

        public virtual ICollection<RendimentoMensalConta> RendimentosMensalConta { get; set; }

        public virtual ICollection<SaldoContaBancaria> SaldosContasBancarias { get; set; }

        public virtual ICollection<TransferenciaConta> TransferenciasContaDestino { get; set; }

        public virtual ICollection<TransferenciaConta> TransferenciasContaOrigem { get; set; }

        public ContaBancaria()
        {
            Descricao = string.Empty;

            Lancamentos = new HashSet<Lancamento>();
            RendimentosMensalConta = new HashSet<RendimentoMensalConta>();
            SaldosContasBancarias = new HashSet<SaldoContaBancaria>();
            TransferenciasContaDestino = new HashSet<TransferenciaConta>();
            TransferenciasContaOrigem = new HashSet<TransferenciaConta>();
        }
    }
}