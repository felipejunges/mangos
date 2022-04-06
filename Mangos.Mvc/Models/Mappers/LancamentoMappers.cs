using Mangos.Dominio.Entities;
using Mangos.Dominio.Enums;
using Mangos.Dominio.Models;
using Mangos.Mvc.Models.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace Mangos.Mvc.Models.Mappers
{
    public static class LancamentoMappers
    {
        public static IEnumerable<LancamentoListaModel> ToListaModel(IEnumerable<Lancamento> lancamentos)
            => lancamentos.Select(ToListaModel);

        public static LancamentoListaModel ToListaModel(Lancamento lancamento)
        {
            return new LancamentoListaModel(
                id: lancamento.Id,
                descricao: lancamento.DescricaoComParcelas,
                pessoa: lancamento.Pessoa?.Nome,
                categoria: lancamento.Categoria?.DescricaoComSuperior,
                contaBancaria: lancamento.ContaBancaria?.Descricao,
                dataVencimento: lancamento.DataVencimento,
                dataPagamento: lancamento.DataPagamento,
                valor: lancamento.Valor,
                agrupador: lancamento.Agrupador,
                pago: lancamento.Pago
            );
        }

        public static IncluirLancamentoCompletoCommand ToIncluirCommand(LancamentoInclusaoModel lancamentoInclusaoModel, TipoLancamento tipoLancamento)
        {
            return new IncluirLancamentoCompletoCommand(
                grupoId: lancamentoInclusaoModel.GrupoId,
                tipoLancamento: tipoLancamento,
                descricao: lancamentoInclusaoModel.Descricao!,
                valor: lancamentoInclusaoModel.Valor,
                dataVencimento: lancamentoInclusaoModel.DataVencimento,
                pessoaId: lancamentoInclusaoModel.PessoaId,
                categoriaId: lancamentoInclusaoModel.CategoriaId,
                observacoes: lancamentoInclusaoModel.Observacoes,
                pago: lancamentoInclusaoModel.Pago,
                dataPagamento: lancamentoInclusaoModel.DataPagamento,
                valorPago: lancamentoInclusaoModel.ValorPago,
                contaBancariaId: lancamentoInclusaoModel.ContaBancariaId,
                dataContaBancaria: lancamentoInclusaoModel.DataContaBancaria,
                parcelado: lancamentoInclusaoModel.Parcelado,
                numeroParcelas: lancamentoInclusaoModel.NumeroParcelas,
                tipoParcelamento: lancamentoInclusaoModel.TipoParcelamento
            );
        }

        public static IncluirLancamentoPagoCommand ToIncluirPagoCommand(LancamentoPagoInclusaoModel lancamentoInclusaoModel, TipoLancamento tipoLancamento)
        {
            return new IncluirLancamentoPagoCommand(
                grupoId: lancamentoInclusaoModel.GrupoId,
                tipoLancamento: tipoLancamento,
                descricao: lancamentoInclusaoModel.Descricao,
                valor: lancamentoInclusaoModel.Valor,
                dataPagamento: lancamentoInclusaoModel.DataPagamento,
                pessoaId: lancamentoInclusaoModel.PessoaId,
                categoriaId: lancamentoInclusaoModel.CategoriaId,
                contaBancariaId: lancamentoInclusaoModel.ContaBancariaId
            );
        }
    }
}