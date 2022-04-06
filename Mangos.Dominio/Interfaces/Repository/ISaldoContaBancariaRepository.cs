using Mangos.Dominio.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mangos.Dominio.Interfaces.Repository
{
    public interface ISaldoContaBancariaRepository
    {
        Task<SaldoContaBancaria?> ObterSaldoContaBancariaAsync(int id);
        Task<SaldoContaBancaria?> ObterSaldoContaBancariaPeloMesAsync(int contaBancariaId, DateTime data);
        Task<SaldoContaBancaria?> ObterSaldoUltimoSaldoAnteriorDataAsync(int contaBancariaId, DateTime data);
        Task<List<SaldoContaBancaria>> ListarSaldosContasBancariasAsync(int grupoId, int? contaBancariaId, DateTime? dataInicial, DateTime? dataFinal);
        Task<List<SaldoContaBancaria>> ListarSaldosContaRelatorioProjecaoSaldoAsync(int grupoId, DateTime data);
        Task<bool> ValidarExisteSaldoAnteriorAbertoAsync(int contaBancariaId, DateTime data);
        Task<bool> ValidarExisteSaldoPosteriorFechadoAsync(int contaBancariaId, DateTime data);
        Task IncluirAsync(SaldoContaBancaria saldoContaBancaria);
        Task AlterarAsync(SaldoContaBancaria saldoContaBancaria);
        Task RemoverDaContaBancariaAsync(ContaBancaria contaBancaria);
        Task RemoverDoGrupoAsync(Grupo grupo);
    }
}