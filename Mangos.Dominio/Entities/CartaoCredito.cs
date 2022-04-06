using System.Collections.Generic;

namespace Mangos.Dominio.Entities
{
    public class CartaoCredito
    {
        public int Id { get; set; }

        public int GrupoId { get; set; }

        public string Descricao { get; set; }

        public int? CategoriaId { get; set; }

        public int DiaFechamento { get; set; }

        public int DiaVencimento { get; set; }

        public int OffsetReferenciaVencimento { get; set; }

        public decimal ValorLimite { get; set; }

        public bool ExibirDadosRelatorio { get; set; }

        public bool GerarLancamentoFecharMes { get; set; }

        public bool Ativo { get; set; }

        public virtual Categoria? Categoria { get; set; }

        public virtual Grupo? Grupo { get; set; }

        public virtual ICollection<LancamentoCartao> LancamentosCartao { get; set; }

        public virtual ICollection<LancamentoFixo> LancamentosFixos { get; set; }

        public CartaoCredito()
        {
            Descricao = string.Empty;

            LancamentosCartao = new HashSet<LancamentoCartao>();
            LancamentosFixos = new HashSet<LancamentoFixo>();
        }
    }
}