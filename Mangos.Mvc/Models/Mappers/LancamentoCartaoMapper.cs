using Mangos.Dominio.Entities;
using Mangos.Dominio.Models;
using Mangos.Mvc.Models.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace Mangos.Mvc.Models.Mappers
{
    public static class LancamentoCartaoMapper
    {
        public static IEnumerable<LancamentoCartaoListaModel> ToListaModel(IEnumerable<LancamentoCartao> lancamentosCartao)
            => lancamentosCartao.Select(ToListaModel);

        public static LancamentoCartaoListaModel ToListaModel(LancamentoCartao lancamentoCartao)
        {
            return new LancamentoCartaoListaModel(
                id: lancamentoCartao.Id,
                cartaoCredito: lancamentoCartao.CartaoCredito?.Descricao ?? string.Empty,
                descricao: lancamentoCartao.DescricaoComParcelas,
                pessoa: lancamentoCartao.Pessoa?.Nome ?? string.Empty,
                mesReferencia: lancamentoCartao.MesReferencia,
                valor: lancamentoCartao.Valor,
                tipoLancamento: lancamentoCartao.TipoLancamento,
                geradoLancamento: lancamentoCartao.GeradoLancamento,
                agrupador: lancamentoCartao.Agrupador
            );
        }

        public static IncluirLancamentoCartaoCommand ToIncluirCommand(LancamentoCartaoInclusaoModel lancamentoCartaoInclusaoModel)
        {
            return new IncluirLancamentoCartaoCommand(
                grupoId: lancamentoCartaoInclusaoModel.GrupoId,
                cartaoCreditoId: lancamentoCartaoInclusaoModel.CartaoCreditoId,
                tipoLancamento: lancamentoCartaoInclusaoModel.TipoLancamento,
                descricao: lancamentoCartaoInclusaoModel.Descricao!,
                pessoaId: lancamentoCartaoInclusaoModel.PessoaId,
                categoriaId: lancamentoCartaoInclusaoModel.CategoriaId,
                valorTotal: lancamentoCartaoInclusaoModel.ValorTotal,
                parcelado: lancamentoCartaoInclusaoModel.Parcelado,
                numeroParcelas: lancamentoCartaoInclusaoModel.NumeroParcelas,
                dataReferencia: lancamentoCartaoInclusaoModel.DataReferencia
            );
        }
    }
}