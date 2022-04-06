using Mangos.Dominio.Enums;
using System;

namespace Mangos.Dominio.Models
{
    public class IncluirLancamentoCompletoCommand
    {
        public int GrupoId { get; private set; }

        public TipoLancamento TipoLancamento { get; private set; }

        public string Descricao { get; private set; }

        public decimal Valor { get; private set; }

        public DateTime? DataVencimento { get; private set; }

        public int? PessoaId { get; private set; }

        public int? CategoriaId { get; private set; }

        public string? Observacoes { get; private set; }

        public bool Pago { get; private set; }

        public DateTime? DataPagamento { get; private set; }

        public decimal? ValorPago { get; private set; }

        public int? ContaBancariaId { get; private set; }

        public DateTime? DataContaBancaria { get; private set; }

        public bool Parcelado { get; private set; }

        public int? NumeroParcelas { get; private set; }

        public TipoParcelamentoLancamento TipoParcelamento { get; private set; }
        
        public IncluirLancamentoCompletoCommand(int grupoId, TipoLancamento tipoLancamento, string descricao, decimal valor, DateTime? dataVencimento, int? pessoaId, int? categoriaId, string? observacoes, bool pago, DateTime? dataPagamento, decimal? valorPago, int? contaBancariaId, DateTime? dataContaBancaria, bool parcelado, int? numeroParcelas, TipoParcelamentoLancamento tipoParcelamento)
        {
            GrupoId = grupoId;
            TipoLancamento = tipoLancamento;
            Descricao = descricao;
            Valor = valor;
            DataVencimento = dataVencimento;
            PessoaId = pessoaId;
            CategoriaId = categoriaId;
            Observacoes = observacoes;
            Pago = pago;
            DataPagamento = dataPagamento;
            ValorPago = valorPago;
            ContaBancariaId = contaBancariaId;
            DataContaBancaria = dataContaBancaria;
            Parcelado = parcelado;
            NumeroParcelas = numeroParcelas;
            TipoParcelamento = tipoParcelamento;
        }
    }
}