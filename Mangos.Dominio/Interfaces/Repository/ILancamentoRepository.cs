using Mangos.Dominio.Entities;
using Mangos.Dominio.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mangos.Dominio.Interfaces.Repository
{
    public interface ILancamentoRepository
    {
        Task<Lancamento?> ObterLancamentoAsync(int id);
        Task<Lancamento?> ObterLancamentoNoTrackingAsync(int id);
        Task<List<Lancamento>> ListarLancamentosAsync(int grupoId, TipoLancamento tipoLancamento, string descricao, string tipoPesquisa, string pesquisa, DateTime dataInicial, DateTime dataFinal, LancamentoTipoDataPesquisa tipoData);
        Task<List<Lancamento>> ListarLancamentosPeloAgrupadorAsync(int grupoId, TipoLancamento tipoLancamento, Guid agrupador);
        Task<List<Lancamento>> ListarLancamentosAlertaAsync(int grupoId, int diasAntecedencia, TipoLancamento tipo);
        Task<List<Lancamento>> ListarDespesasPagasDaPessoaNoTrackingAsync(int grupoId, int pessoaId, DateTime dataMinima);
        Task<List<Lancamento>> ListarLancamentosDaPessoaAsync(int grupoId, int pessoaId);
        Task<List<Lancamento>> ListarLancamentosVencendoAsync(int grupoId, DateTime dataFinal);
        Task<List<Lancamento>> ListarLancamentosRelatorioExtratoAsync(int grupoId, int contaBancariaId, DateTime dataInicial, DateTime dataFinal);
        Task<List<Lancamento>> ListarLancamentosPendentesRelatorioProjecaoAsync(int grupoId, DateTime dataInicial, DateTime dataFinal);
        Task<List<Lancamento>> ListarLancamentosPagosRelatorioProjecaoAsync(int grupoId, DateTime dataInicial, DateTime dataFinal);
        Task<List<Lancamento>> ListarLancamentosRelatorioCategoriaAsync(int grupoId, string situacao, string tipo, DateTime dataInicial, DateTime dataFinal);
        Task<List<Lancamento>> ListarLancamentosDoLancamentoFixoAsync(int lancamentoFixoId);
        Task<List<Lancamento>> ListarLancamentosAtrasadosRelatorioProjecaoSaldo(int grupoId);
        Task<List<Lancamento>> ListarLancamentosPendentesRelatorioProjecaoSaldo(int grupoId, DateTime dataFinal);
        Task<DateTime?> ObterMenorDataDaContaBancariaAsync(int contaBancariaId);
        Task<decimal> ObterValorLancamentosNoPeriodoAsync(int contaBancariaId, DateTime dataInicial, DateTime dataFinal);
        Task IncluirAsync(Lancamento lancamento);
        Task AlterarAsync(Lancamento lancamento);
        Task RemoverAsync(Lancamento lancamento);
        Task RemoverDoGrupoAsync(Grupo grupo);
    }
}