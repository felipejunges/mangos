using Mangos.Dominio.Enums;
using System;

namespace Mangos.Dominio.Models
{
    public class IncluirLancamentoPagoCommand
    {
        public int GrupoId { get; private set; }

        public TipoLancamento TipoLancamento { get; private set; }

        public string? Descricao { get; private set; }

        public decimal Valor { get; private set; }

        public DateTime? DataPagamento { get; private set; }

        public int? PessoaId { get; private set; }

        public int? CategoriaId { get; private set; }

        public int? ContaBancariaId { get; private set; }

        public IncluirLancamentoPagoCommand(int grupoId, TipoLancamento tipoLancamento, string? descricao, decimal valor, DateTime? dataPagamento, int? pessoaId, int? categoriaId, int? contaBancariaId)
        {
            GrupoId = grupoId;
            TipoLancamento = tipoLancamento;
            Descricao = descricao;
            Valor = valor;
            DataPagamento = dataPagamento;
            PessoaId = pessoaId;
            CategoriaId = categoriaId;
            ContaBancariaId = contaBancariaId;
        }
    }
}