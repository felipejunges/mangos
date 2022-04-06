using Mangos.Dominio.Enums;
using System;

namespace Mangos.Dominio.Models
{
    public class IncluirLancamentoCartaoCommand
    {
        public int GrupoId { get; private set; }

        public int CartaoCreditoId { get; private set; }

        public TipoLancamentoCartao TipoLancamento { get; private set; }

        public string Descricao { get; private set; }

        public int? PessoaId { get; private set; }

        public int? CategoriaId { get; private set; }

        public decimal ValorTotal { get; private set; }

        public bool Parcelado { get; private set; }

        public int? NumeroParcelas { get; private set; }

        public DateTime? DataReferencia { get; private set; }

        public IncluirLancamentoCartaoCommand(int grupoId, int cartaoCreditoId, TipoLancamentoCartao tipoLancamento, string descricao, int? pessoaId, int? categoriaId, decimal valorTotal, bool parcelado, int? numeroParcelas, DateTime? dataReferencia)
        {
            GrupoId = grupoId;
            CartaoCreditoId = cartaoCreditoId;
            TipoLancamento = tipoLancamento;
            Descricao = descricao;
            PessoaId = pessoaId;
            CategoriaId = categoriaId;
            ValorTotal = valorTotal;
            Parcelado = parcelado;
            NumeroParcelas = numeroParcelas;
            DataReferencia = dataReferencia;
        }
    }
}