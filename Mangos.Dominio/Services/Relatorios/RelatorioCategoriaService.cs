using Mangos.Dominio.Enums;
using Mangos.Dominio.Models.Relatorios;
using Mangos.Dominio.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mangos.Dominio.Services.Relatorios
{
    public class RelatorioCategoriaService
    {
        private readonly ILancamentoRepository _lancamentoRepository;
        private readonly ILancamentoCartaoRepository _lancamentoCartaoRepository;
        private readonly IRendimentoMensalContaRepository _rendimentoMensalContaRepository;

        public RelatorioCategoriaService(ILancamentoRepository lancamentoRepository, ILancamentoCartaoRepository lancamentoCartaoRepository, IRendimentoMensalContaRepository rendimentoMensalContaRepository)
        {
            _lancamentoRepository = lancamentoRepository;
            _lancamentoCartaoRepository = lancamentoCartaoRepository;
            _rendimentoMensalContaRepository = rendimentoMensalContaRepository;
        }


        public async Task<IEnumerable<RelatorioCategoriaModel>> ListarRelatorioCategoriaMensalAsync(int grupoId, DateTime mesInicial, DateTime mesFinal, string tipo, string situacao, string ordenacao, bool agruparSubcategorias)
        {
            var dataInicial = mesInicial;
            var dataFinal = mesFinal.AddMonths(1).AddDays(-1);

            return await ListarRelatorioCategoriaAsync(grupoId, dataInicial, dataFinal, false, tipo, situacao, ordenacao, agruparSubcategorias);
        }

        public async Task<IEnumerable<RelatorioCategoriaModel>> ListarRelatorioCategoriaAnualAsync(int grupoId, int anoInicial, int anoFinal, string tipo, string situacao, string ordenacao, bool agruparSubcategorias)
        {
            var dataInicial = new DateTime(anoInicial, 1, 1);
            var dataFinal = new DateTime(anoFinal, 12, 31);

            return await ListarRelatorioCategoriaAsync(grupoId, dataInicial, dataFinal, true, tipo, situacao, ordenacao, agruparSubcategorias);
        }

        private async Task<IEnumerable<RelatorioCategoriaModel>> ListarRelatorioCategoriaAsync(int grupoId, DateTime dataInicial, DateTime dataFinal, bool agruparAno, string tipo, string situacao, string ordenacao, bool agruparSubcategorias)
        {
            var lancamentos = await _lancamentoRepository.ListarLancamentosRelatorioCategoriaAsync(grupoId, situacao, tipo, dataInicial, dataFinal);
            var lancamentosCartao = await _lancamentoCartaoRepository.ListarLancamentosCartaoRelatorioCategoriaAsync(grupoId, situacao, tipo, dataInicial, dataFinal);
            var rendimentosMensais = await _rendimentoMensalContaRepository.ListarRendimentosMensaisRelatorioCategoriaAsync(grupoId, situacao, tipo, dataInicial, dataFinal);

            //
            var lancamentosModel = lancamentos
                    .Select(o => new
                    {
                        CategoriaId = agruparSubcategorias
                                                ? (o.Categoria!.CategoriaSuperiorId != null ? o.Categoria.CategoriaSuperiorId.Value : o.CategoriaId!.Value)
                                                : o.CategoriaId!.Value,
                        Categoria = agruparSubcategorias
                                                ? (o.Categoria!.CategoriaSuperior != null ? o.Categoria.CategoriaSuperior.Descricao : o.Categoria.Descricao)
                                                : (o.Categoria!.CategoriaSuperior != null ? o.Categoria.CategoriaSuperior.Descricao + " / " : string.Empty) + o.Categoria.Descricao,
                        o.Tipo,
                        Data = o.Pago ? o.DataPagamento!.Value : o.DataVencimento,
                        Valor = o.Pago ? o.ValorPago!.Value : o.Valor
                    });

            //
            var lancamentosCartaoModel = lancamentosCartao
                    .Select(o => new
                    {
                        CategoriaId = agruparSubcategorias
                                                ? (o.Categoria!.CategoriaSuperiorId != null ? o.Categoria.CategoriaSuperiorId.Value : o.CategoriaId!.Value)
                                                : o.CategoriaId!.Value,

                        Categoria = agruparSubcategorias
                                                ? (o.Categoria!.CategoriaSuperior != null ? o.Categoria.CategoriaSuperior.Descricao : o.Categoria.Descricao)
                                                : (o.Categoria!.CategoriaSuperior != null ? o.Categoria.CategoriaSuperior.Descricao + " / " : string.Empty) + o.Categoria.Descricao,
                        Tipo = o.TipoLancamento == TipoLancamentoCartao.Receita ? TipoLancamento.Receita : TipoLancamento.Despesa,
                        Data = o.MesReferencia.AddMonths(o.CartaoCredito!.OffsetReferenciaVencimento),
                        o.Valor
                    });

            //
            var rendimentosMensalContaModel = rendimentosMensais
                    .Select(o => new
                    {
                        CategoriaId = agruparSubcategorias
                                                ? (o.ContaBancaria!.CategoriaRendimentoMensal!.CategoriaSuperiorId != null ? o.ContaBancaria.CategoriaRendimentoMensal.CategoriaSuperiorId.Value : o.ContaBancaria.CategoriaRendimentoMensalId!.Value)
                                                : o.ContaBancaria!.CategoriaRendimentoMensalId!.Value,
                        Categoria = agruparSubcategorias
                                                ? (o.ContaBancaria!.CategoriaRendimentoMensal!.CategoriaSuperior != null ? o.ContaBancaria!.CategoriaRendimentoMensal!.CategoriaSuperior.Descricao : o.ContaBancaria.CategoriaRendimentoMensal.Descricao)
                                                : (o.ContaBancaria!.CategoriaRendimentoMensal!.CategoriaSuperior != null ? o.ContaBancaria!.CategoriaRendimentoMensal!.CategoriaSuperior.Descricao + " / " : string.Empty) + o.ContaBancaria.CategoriaRendimentoMensal.Descricao,
                        Tipo = TipoLancamento.Receita,
                        Data = o.MesReferencia,
                        o.Valor
                    });

            //
            var dadosList = lancamentosModel.Concat(lancamentosCartaoModel).Concat(rendimentosMensalContaModel).ToList();

            //
            // Transforma os dados [Categoria - Tipo - Mes - Valor] em [Categoria - Tipo - Dict<Mes, Valor>]
            var dadosRelatorio = dadosList.GroupBy(o => new
            {
                o.CategoriaId,
                o.Categoria,
                o.Tipo,
                Data = agruparAno ? new DateTime(o.Data.Year, 1, 1) : new DateTime(o.Data.Year, o.Data.Month, 1)
            }).OrderBy(o => o.Key.Categoria).ThenBy(o => o.Key.Data);

            //
            var relatorio = new List<RelatorioCategoriaModel>();

            foreach (var dadoRelatorio in dadosRelatorio)
            {
                var itemRelatorio = relatorio.Where(o => o.CategoriaId == dadoRelatorio.Key.CategoriaId && o.Tipo == dadoRelatorio.Key.Tipo).FirstOrDefault();
                if (itemRelatorio == null)
                {
                    itemRelatorio = new RelatorioCategoriaModel(
                        categoriaId: dadoRelatorio.Key.CategoriaId,
                        categoria: dadoRelatorio.Key.Categoria,
                        tipo: dadoRelatorio.Key.Tipo,
                        valores: new Dictionary<DateTime, decimal>(),
                        valorTotal: 0M);

                    relatorio.Add(itemRelatorio);
                }

                itemRelatorio.Valores.Add(dadoRelatorio.Key.Data, dadoRelatorio.Sum(s => s.Valor));
                itemRelatorio.ValorTotal = itemRelatorio.Valores.Sum(o => o.Value);
            }

            if (ordenacao == "A")
                relatorio.Sort((o, i) => o.Categoria.CompareTo(i.Categoria));
            else if (ordenacao == "TC")
                relatorio.Sort((o, i) => o.ValorTotal.CompareTo(i.ValorTotal));
            else if (ordenacao == "TD")
                relatorio.Sort((o, i) => i.ValorTotal.CompareTo(o.ValorTotal));

            return relatorio;
        }
    }
}