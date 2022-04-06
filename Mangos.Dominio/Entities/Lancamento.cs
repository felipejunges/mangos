using Mangos.Dominio.Enums;
using System;
using System.Collections.Generic;

namespace Mangos.Dominio.Entities
{
    public class Lancamento
    {
        public int Id { get; set; }

        public int GrupoId { get; set; }

        public DateTime DataHoraCadastro { get; set; }

        public TipoLancamento Tipo { get; set; }

        public decimal Valor { get; set; }

        public DateTime DataVencimento { get; set; }

        public string Descricao { get; set; }

        public Guid? Agrupador { get; set; }

        public int NumeroParcela { get; set; }

        public int TotalParcelas { get; set; }

        public int? PessoaId { get; set; }

        public int? CategoriaId { get; set; }

        public string? Observacoes { get; set; }

        public bool Pago { get; set; }

        public DateTime? DataPagamento { get; set; }

        public decimal? ValorPago { get; set; }

        public int? ContaBancariaId { get; set; }

        public DateTime? DataContaBancaria { get; set; }

        public int? LancamentoFixoOrigemId { get; set; }

        public virtual Categoria? Categoria { get; set; }

        public virtual ContaBancaria? ContaBancaria { get; set; }

        public virtual Grupo? Grupo { get; set; }

        public virtual LancamentoFixo? LancamentoFixoOrigem { get; set; }

        public virtual ICollection<LancamentoCartao> LancamentosCartao { get; set; }

        public virtual Pessoa? Pessoa { get; set; }

        public string DescricaoComParcelas => Descricao + (TotalParcelas > 1 ? $" [{NumeroParcela.ToString()}/{TotalParcelas.ToString()}]" : string.Empty);

        public Lancamento()
        {
            Descricao = string.Empty;

            LancamentosCartao = new HashSet<LancamentoCartao>();
        }
    }
}