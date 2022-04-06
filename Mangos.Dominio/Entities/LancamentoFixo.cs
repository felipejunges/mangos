using Mangos.Dominio.Enums;
using System;
using System.Collections.Generic;

namespace Mangos.Dominio.Entities
{
    public class LancamentoFixo : IComparable
    {
        public int Id { get; set; }

        public int GrupoId { get; set; }

        public TipoLancamentoFixo Tipo { get; set; }

        public PeriodicidadeLancamentoFixo Periodicidade { get; set; }

        public decimal Valor { get; set; }

        public int DiaVencimento { get; set; }

        public int? MesVencimento { get; set; }

        public int? CartaoCreditoId { get; set; }

        public string Descricao { get; set; }

        public int? PessoaId { get; set; }

        public int? CategoriaId { get; set; }

        public DateTime? DataUltimoMesGerado { get; set; }

        public DateTime? DataHoraUltimaGeracao { get; set; }

        public bool Ativo { get; set; }

        public virtual CartaoCredito? CartaoCredito { get; set; }

        public virtual Categoria? Categoria { get; set; }

        public virtual Grupo? Grupo { get; set; }

        public virtual ICollection<Lancamento> LancamentosOrigem { get; set; }

        public virtual ICollection<LancamentoCartao> LancamentosCartaoOrigem { get; set; }

        public virtual Pessoa? Pessoa { get; set; }

        public int CompareTo(object? obj)
        {
            var outroLancamento = obj as LancamentoFixo;

            if (outroLancamento == null)
                throw new ArgumentException($"{nameof(obj)} deve ser um {nameof(LancamentoFixo)}.");

            if (this.Tipo != outroLancamento.Tipo)
                return this.TipoOrder.CompareTo(outroLancamento.TipoOrder);

            if (this.Periodicidade != outroLancamento.Periodicidade)
                return this.PeriodicidadeOrder.CompareTo(outroLancamento.PeriodicidadeOrder);

            if (this.MesVencimento != outroLancamento.MesVencimento)
                return (this.MesVencimento ?? 0).CompareTo(outroLancamento.MesVencimento ?? 0);

            if (this.DiaVencimento != outroLancamento.DiaVencimento)
                return this.DiaVencimento.CompareTo(outroLancamento.DiaVencimento);

            return 0;
        }

        private int TipoOrder
        {
            get => Tipo == TipoLancamentoFixo.Receita ? 1
                    : Tipo == TipoLancamentoFixo.ReceitaCartao ? 2
                    : Tipo == TipoLancamentoFixo.Despesa ? 3
                    : Tipo == TipoLancamentoFixo.DebitoCartao ? 4
                    : 5;
        }

        private int PeriodicidadeOrder
        {
            get => Periodicidade == PeriodicidadeLancamentoFixo.Anual ? 1
                    : Periodicidade == PeriodicidadeLancamentoFixo.Mensal ? 2
                    : 3;
        }

        public LancamentoFixo()
        {
            Descricao = string.Empty;

            LancamentosOrigem = new HashSet<Lancamento>();
            LancamentosCartaoOrigem = new HashSet<LancamentoCartao>();
        }
    }
}