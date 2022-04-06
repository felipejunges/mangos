using Mangos.Dominio.Enums;
using Mangos.Dominio.Models.Relatorios;
using Mangos.Dominio.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mangos.Dominio.Services.Relatorios
{
    public class RelatorioCartaoCreditoService
    {
        private readonly ILancamentoCartaoRepository _lancamentoCartaoRepository;

        public RelatorioCartaoCreditoService(ILancamentoCartaoRepository lancamentoCartaoRepository)
        {
            _lancamentoCartaoRepository = lancamentoCartaoRepository;
        }

        public async Task<RelatorioCartaoCreditoModel> ObterRelatorioCartoesCreditoAsync(int grupoId, int? cartaoCreditoId, DateTime? mesInicial, DateTime? mesFinal, bool agruparCartoes)
        {
            return new RelatorioCartaoCreditoModel(
                    lancamentos: await ListarRelatorioCartoesCreditoLancamentosAsync(grupoId, cartaoCreditoId, mesInicial, mesFinal, agruparCartoes),
                    limites: await ObterRelatorioCartoesCreditoLimitesAsync(grupoId, cartaoCreditoId)
                );
        }

        private async Task<IEnumerable<RelatorioCartaoCreditoLancamentoModel>> ListarRelatorioCartoesCreditoLancamentosAsync(int grupoId, int? cartaoCreditoId, DateTime? mesInicial, DateTime? mesFinal, bool agruparCartoes)
        {
            var lancamentosCartao = await _lancamentoCartaoRepository.ListarLancamentosCartaoRelatorioAsync(grupoId, cartaoCreditoId, mesInicial, mesFinal);

            return lancamentosCartao
                        .GroupBy(l => new
                        {
                            CartaoCreditoId = agruparCartoes ? 0 : l.CartaoCreditoId,
                            CartaoCredito = agruparCartoes ? "Todos" : l.CartaoCredito!.Descricao,
                            l.MesReferencia
                        })
                        .OrderBy(o => o.Key.MesReferencia)
                        .Select(o => new RelatorioCartaoCreditoLancamentoModel(
                            cartaoCreditoId: o.Key.CartaoCreditoId,
                            cartaoCredito: o.Key.CartaoCredito,
                            mesReferencia: o.Key.MesReferencia,
                            valorTotal: o.Sum(s => s.TipoLancamento == TipoLancamentoCartao.Despesa ? s.Valor : s.Valor * -1))
                        )
                        .ToList();
        }

        private async Task<IEnumerable<RelatorioCartaoCreditoLimiteModel>> ObterRelatorioCartoesCreditoLimitesAsync(int grupoId, int? cartaoCreditoId)
        {
            var lancamentosCartao = await _lancamentoCartaoRepository.ListarCartoesLancamentosLimitesAsync(grupoId, cartaoCreditoId);

            return lancamentosCartao
                                .OrderBy(l => l.CartaoCredito)
                                .Select(l => new RelatorioCartaoCreditoLimiteModel(
                                    cartaoCreditoId: l.CartaoCreditoId,
                                    cartaoCredito: l.CartaoCredito,
                                    valorLimiteTotal: l.ValorLimite,
                                    valorLimiteUtilizado: l.ValorLimiteUtilizado)
                                )
                                .ToList();
        }
    }
}