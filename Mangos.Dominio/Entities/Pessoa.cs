using Mangos.Dominio.Enums;
using System.Collections.Generic;

namespace Mangos.Dominio.Entities
{
    public class Pessoa
    {
        public int Id { get; set; }

        public int GrupoId { get; set; }

        public string Nome { get; set; }

        public string? Telefone1 { get; set; }

        public string? Telefone2 { get; set; }

        public string? Telefone3 { get; set; }

        public string? Site { get; set; }

        public string? Email { get; set; }

        public TipoPessoa Tipo { get; set; }

        public int? CategoriaPadraoReceitaId { get; set; }

        public int? CategoriaPadraoDespesaId { get; set; }

        public bool Ativo { get; set; }

        public virtual Categoria? CategoriaPadraoDespesa { get; set; }

        public virtual Categoria? CategoriaPadraoReceita { get; set; }

        public virtual Grupo? Grupo { get; set; }

        public virtual ICollection<Lancamento> Lancamentos { get; set; }

        public virtual ICollection<LancamentoCartao> LancamentosCartao { get; set; }

        public virtual ICollection<LancamentoFixo> LancamentosFixos { get; set; }

        public virtual ICollection<PessoaCoordenada> PessoaCoordenadas { get; set; }

        public Pessoa()
        {
            Nome = string.Empty;

            Lancamentos = new HashSet<Lancamento>();
            LancamentosCartao = new HashSet<LancamentoCartao>();
            LancamentosFixos = new HashSet<LancamentoFixo>();
            PessoaCoordenadas = new HashSet<PessoaCoordenada>();
        }
    }
}