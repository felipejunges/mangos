using System.Collections.Generic;

namespace Mangos.Dominio.Entities
{
    public class Grupo
    {
        public int Id { get; set; }

        public string Descricao { get; set; }

        public int MesesAntecedenciaGerarLancamento { get; set; }

        public int MesesAntecedenciaGerarLancamentoCartao { get; set; }

        public int MesesGraficosDashboard { get; set; }

        public virtual ICollection<CartaoCredito> CartoesCredito { get; set; }

        public virtual ICollection<Categoria> Categorias { get; set; }

        public virtual ICollection<ContaBancaria> ContasBancarias { get; set; }

        public virtual ICollection<Lancamento> Lancamentos { get; set; }

        public virtual ICollection<LancamentoCartao> LancamentosCartao { get; set; }

        public virtual ICollection<LancamentoFixo> LancamentosFixos { get; set; }

        public virtual ICollection<MetaMovimentacao> MetasMovimentacao { get; set; }

        public virtual ICollection<MetaSaldo> MetasSaldo { get; set; }

        public virtual ICollection<Pessoa> Pessoas { get; set; }

        public virtual ICollection<TransferenciaConta> TransferenciasConta { get; set; }

        public virtual ICollection<Usuario> Usuarios { get; set; }

        public Grupo()
        {
            Descricao = string.Empty;

            CartoesCredito = new HashSet<CartaoCredito>();
            Categorias = new HashSet<Categoria>();
            ContasBancarias = new HashSet<ContaBancaria>();
            Lancamentos = new HashSet<Lancamento>();
            LancamentosCartao = new HashSet<LancamentoCartao>();
            LancamentosFixos = new HashSet<LancamentoFixo>();
            MetasMovimentacao = new HashSet<MetaMovimentacao>();
            MetasSaldo = new HashSet<MetaSaldo>();
            Pessoas = new HashSet<Pessoa>();
            TransferenciasConta = new HashSet<TransferenciaConta>();
            Usuarios = new HashSet<Usuario>();
        }
    }
}