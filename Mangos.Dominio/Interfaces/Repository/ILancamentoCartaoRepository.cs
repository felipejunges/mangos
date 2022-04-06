using Mangos.Dominio.DTOs;
using Mangos.Dominio.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mangos.Dominio.Interfaces.Repository
{
    public interface ILancamentoCartaoRepository
    {
        Task<LancamentoCartao?> ObterLancamentoCartaoAsync(int id);
        Task<DateTime?> ObterDataPrimeiroLancamentoAbertoAsync(int cartaoCreditoId);
        Task<DateTime?> ObterDataUltimoLancamentoGeradoAsync(int cartaoCreditoId);
        Task<DateTime> ObterMesReferenciaGerarLancamentoCartaoAsync(int cartaoCreditoId);
        Task<List<LancamentoCartao>> ListarLancamentosCartaoAsync(int grupoId, int? cartaoCreditoId, string? descricao, string? pessoa, DateTime? mesReferenciaInicial, DateTime? mesReferenciaFinal);
        Task<List<LancamentoCartao>> ListarLancamentosCartaoAgrupadosAsync(int grupoId, Guid agrupador);
        Task<List<LancamentoCartao>> ListarLancamentosCartaoFecharAsync(int grupoId, int cartaoCreditoId, DateTime mesReferencia);
        Task<List<LancamentoCartao>> ListarLancamentosCartaoReabrirAsync(int grupoId, int cartaoCreditoId, DateTime mesReferencia);
        Task<(DateTime? DataMinima, DateTime? DataMaxima)> ObterDatasMenorMaiorVencimentosNaoFechadosAsync(int grupoId);
        Task<List<LancamentoCartao>> ListarLancamentosCartaoAlertaAsync(int grupoId);
        Task<List<LancamentoCartao>> ListarDespesasPagasDaPessoaNoTrackingAsync(int grupoId, int pessoaId, DateTime dataMinima);
        Task<List<LancamentoCartao>> ListarLancamentosCartaoDaPessoaAsync(int grupoId, int pessoaId);
        Task<List<LancamentoCartao>> ListarLancamentosCartaoVencendoAsync(int grupoId, DateTime dataFinal);
        Task<List<LancamentoCartao>> ListarLancamentosCartaoRelatorioAsync(int grupoId, int? cartaoCreditoId, DateTime? mesInicial, DateTime? mesFinal);
        Task<List<LancamentoCartao>> ListarLancamentosCartaoPendentesRelatorioProjecaoAsync(int grupoId, DateTime mesInicial, DateTime mesFinal);
        Task<List<LancamentoCartao>> ListarLancamentosCartaoPagosRelatorioProjecaoAsync(int grupoId, DateTime mesInicial, DateTime mesFinal);
        Task<List<LancamentoCartao>> ListarLancamentosCartaoDoLancamentoFixoAsync(int lancamentoFixoId);
        Task<List<LancamentoCartao>> ListarLancamentosCartaoRelatorioCategoriaAsync(int grupoId, string situacao, string tipo, DateTime dataInicial, DateTime dataFinal);
        Task<List<LancamentoCartao>> ListarLancamentosCartaoAtrasadosRelatorioProjecaoSaldo(int grupoId);
        Task<List<LancamentoCartao>> ListarLancamentosCartaoPendentesRelatorioProjecaoSaldo(int grupoId, DateTime dataFinal);
        Task<List<CartaoCreditoLimiteDto>> ListarCartoesLancamentosLimitesAsync(int grupoId, int? cartaoCreditoId);
        Task IncluirAsync(LancamentoCartao lancamentoCartao);
        Task AlterarAsync(LancamentoCartao lancamentoCartao);
        Task RemoverAsync(LancamentoCartao lancamentoCartao);
        Task RemoverDoGrupoAsync(Grupo grupo);
    }
}