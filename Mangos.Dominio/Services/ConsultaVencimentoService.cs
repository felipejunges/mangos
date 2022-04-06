using Mangos.Dominio.Models;
using Mangos.Dominio.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mangos.Dominio.Services
{
    public class ConsultaVencimentoService
    {
        private readonly ILancamentoRepository _lancamentoRepository;
        private readonly ILancamentoCartaoRepository _lancamentoCartaoRepository;

        public ConsultaVencimentoService(ILancamentoRepository lancamentoRepository, ILancamentoCartaoRepository lancamentoCartaoRepository)
        {
            _lancamentoRepository = lancamentoRepository;
            _lancamentoCartaoRepository = lancamentoCartaoRepository;
        }

        public async Task<List<ConsultaVencimentoModel>> ListarConsultaVencimentosAsync(int grupoId, DateTime dataFinal)
        {
            var lancamentos = await _lancamentoRepository.ListarLancamentosVencendoAsync(grupoId, dataFinal);
            var lancamentosCartao = await _lancamentoCartaoRepository.ListarLancamentosCartaoVencendoAsync(grupoId, dataFinal);

            var lancamentosModel =
                    lancamentos
                    .OrderBy(o => o.DataVencimento)
                    .Select(o => new ConsultaVencimentoModel(
                        id: o.Id,
                        tipo: o.Tipo == Enums.TipoLancamento.Receita ? TipoRegistroConsultaVencimento.Receita : TipoRegistroConsultaVencimento.Despesa,
                        dataVencimento: o.DataVencimento,
                        valor: o.Valor,
                        pessoa: o.Pessoa != null ? o.Pessoa.Nome : string.Empty,
                        descricao: o.DescricaoComParcelas)
                    ).ToList();

            var lancamentosCartaoModel = lancamentosCartao
                    .GroupBy(o => new
                    {
                        o.CartaoCreditoId,
                        CartaoCredito = o.CartaoCredito!.Descricao,
                        o.CartaoCredito.DiaVencimento,
                        o.CartaoCredito.OffsetReferenciaVencimento,
                        o.MesReferencia
                    })
                    .Select(o => new ConsultaVencimentoModel(
                        id: o.Key.CartaoCreditoId,
                        tipo: TipoRegistroConsultaVencimento.CartaoCredito,
                        dataVencimento: o.Key.MesReferencia.AddMonths(o.Key.OffsetReferenciaVencimento).AddDays(o.Key.DiaVencimento - 1),
                        valor: o.Sum(s => s.Valor),
                        pessoa: string.Empty,
                        descricao: $"Cartão de crédito {o.Key.CartaoCredito}")
                    )
                    .ToList();

            var vencimentos =
                    lancamentosModel
                    .Concat(lancamentosCartaoModel)
                    .OrderBy(o => o.DataVencimento)
                    .ToList();

            return vencimentos;
        }
    }
}