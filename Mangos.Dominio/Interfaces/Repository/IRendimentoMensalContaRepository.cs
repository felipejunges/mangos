using Mangos.Dominio.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mangos.Dominio.Interfaces.Repository
{
    public interface IRendimentoMensalContaRepository
    {
        Task<RendimentoMensalConta?> ObterRendimentoMensalContaAsync(int id);
        Task<RendimentoMensalConta?> ObterRendimentoMensalContaNoTrackingAsync(int id);
        Task<DateTime?> ObterMenorDataDaContaBancariaAsync(int contaBancariaId);
        Task<decimal> ObterValorRendimentosContaNoPeriodoAsync(int contaBancariaId, DateTime dataInicial, DateTime dataFinal);
        Task<List<RendimentoMensalConta>> ListarRendimentosMensaisContaAsync(int grupoId, int? contaBancariaId, DateTime dataInicial, DateTime dataFinal);
        Task<List<RendimentoMensalConta>> ListarRendimentosContaRelatorioExtratoAsync(int grupoId, int contaBancariaId, DateTime dataInicial, DateTime dataFinal);
        Task<List<RendimentoMensalConta>> ListarRendimentosMensaisRelatorioProjecaoAsync(int grupoId, DateTime mesInicial, DateTime mesFinal);
        Task<List<RendimentoMensalConta>> ListarRendimentosMensaisRelatorioCategoriaAsync(int grupoId, string situacao, string tipo, DateTime dataInicial, DateTime dataFinal);
        Task IncluirAsync(RendimentoMensalConta rendimentoMensalConta);
        Task AtualizarAsync(RendimentoMensalConta rendimentoMensalConta);
        Task RemoverAsync(RendimentoMensalConta rendimentoMensalConta);
        Task RemoverDaContaBancariaAsync(ContaBancaria contaBancaria);
        Task RemoverDoGrupoAsync(Grupo grupo);
    }
}