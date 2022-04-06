using Mangos.Dominio.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mangos.Dominio.Interfaces.Repository
{
    public interface ITransferenciaContaRepository
    {
        Task<TransferenciaConta?> ObterTransferenciaContaAsync(int id);
        Task<TransferenciaConta?> ObterTransferenciaContaNoTrackingAsync(int id);
        Task<List<TransferenciaConta>> ListarTransferenciasContaAsync(int grupoId, string? descricao, DateTime dataInicial, DateTime dataFinal);
        Task<List<TransferenciaConta>> ListarTransferenciasContaOrigemRelatorioExtratoAsync(int grupoId, int contaBancariaId, DateTime dataInicial, DateTime dataFinal);
        Task<List<TransferenciaConta>> ListarTransferenciasContaDestinoRelatorioExtratoAsync(int grupoId, int contaBancariaId, DateTime dataInicial, DateTime dataFinal);
        Task<DateTime?> ObterMenorDataDaContaBancariaOrigemAsync(int contaBancariaId);
        Task<DateTime?> ObterMenorDataDaContaBancariaDestinoAsync(int contaBancariaId);
        Task<decimal> ObterValorTransferenciasContaNoPeriodo(int contaBancariaId, DateTime dataInicial, DateTime dataFinal);
        Task IncluirAsync(TransferenciaConta transferenciaConta);
        Task AlterarAsync(TransferenciaConta transferenciaConta);
        Task RemoverAsync(TransferenciaConta transferenciaConta);
        Task RemoverDoGrupoAsync(Grupo grupo);
    }
}