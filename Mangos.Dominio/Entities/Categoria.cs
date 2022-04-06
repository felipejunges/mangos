using System.Collections.Generic;

namespace Mangos.Dominio.Entities
{
    public class Categoria
    {
        public int Id { get; set; }

        public int GrupoId { get; set; }

        public int? CategoriaSuperiorId { get; set; }

        public string Descricao { get; set; }

        public bool Receita { get; set; }

        public bool Despesa { get; set; }

        public bool RelatorioTotal { get; set; }

        public bool Ativo { get; set; }

        public string DescricaoComSuperior => (CategoriaSuperior != null ? CategoriaSuperior.Descricao + " / " : string.Empty) + Descricao;

        public virtual ICollection<CartaoCredito> CartoesCredito { get; set; }

        public virtual Categoria? CategoriaSuperior { get; set; }

        public virtual ICollection<Categoria> CategoriasFilhas { get; set; }

        public virtual ICollection<ContaBancaria> ContasBancariasRendimentoMensal { get; set; }

        public virtual Grupo? Grupo { get; set; }

        public virtual ICollection<Lancamento> Lancamentos { get; set; }

        public virtual ICollection<LancamentoCartao> LancamentosCartao { get; set; }

        public virtual ICollection<LancamentoFixo> LancamentosFixos { get; set; }

        public virtual ICollection<Pessoa> PessoasPadraoDespesa { get; set; }

        public virtual ICollection<Pessoa> PessoasPadraoReceita { get; set; }

        public Categoria()
        {
            Descricao = string.Empty;

            CartoesCredito = new HashSet<CartaoCredito>();
            CategoriasFilhas = new HashSet<Categoria>();
            ContasBancariasRendimentoMensal = new HashSet<ContaBancaria>();
            Lancamentos = new HashSet<Lancamento>();
            LancamentosCartao = new HashSet<LancamentoCartao>();
            LancamentosFixos = new HashSet<LancamentoFixo>();
            PessoasPadraoDespesa = new HashSet<Pessoa>();
            PessoasPadraoReceita = new HashSet<Pessoa>();
        }
    }
}