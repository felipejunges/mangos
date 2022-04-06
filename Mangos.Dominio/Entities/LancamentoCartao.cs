using Mangos.Dominio.Enums;
using System;

namespace Mangos.Dominio.Entities
{
    public class LancamentoCartao
    {
        public int Id { get; set; }

        public int GrupoId { get; set; }

        public DateTime DataHoraCadastro { get; set; }

        public int CartaoCreditoId { get; set; }

        public TipoLancamentoCartao TipoLancamento { get; set; }

        public decimal Valor { get; set; }

        public DateTime MesReferencia { get; set; }

        public string Descricao { get; set; }

        public Guid? Agrupador { get; set; }

        public int NumeroParcela { get; set; }

        public int TotalParcelas { get; set; }

        public int? PessoaId { get; set; }

        public int? CategoriaId { get; set; }

        public string? Observacoes { get; set; }

        public bool GeradoLancamento { get; set; }

        public int? LancamentoId { get; set; }

        public DateTime? DataHoraGeracaoLancamento { get; set; }

        public int? LancamentoFixoOrigemId { get; set; }

        public virtual CartaoCredito? CartaoCredito { get; set; }

        public virtual Categoria? Categoria { get; set; }

        public virtual Grupo? Grupo { get; set; }

        public virtual Lancamento? Lancamento { get; set; }

        public virtual LancamentoFixo? LancamentoFixoOrigem { get; set; }

        public virtual Pessoa? Pessoa { get; set; }

        public string DescricaoComParcelas => Descricao + (TotalParcelas > 1 ? $" [{NumeroParcela.ToString()}/{TotalParcelas.ToString()}]" : string.Empty);

        public LancamentoCartao()
        {
            Descricao = string.Empty;
        }
    }
}