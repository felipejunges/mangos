using Mangos.Dominio.Entities;
using Mangos.Dominio.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mangos.Infra.Repository
{
    public class SaldoContaBancariaRepository : ISaldoContaBancariaRepository
    {
        private readonly MangosDb Db;

        public SaldoContaBancariaRepository(MangosDb db)
        {
            Db = db;
        }

        public Task<SaldoContaBancaria?> ObterSaldoContaBancariaAsync(int id)
        {
            return Db.SaldosContasBancarias
                        .Where(s => s.Id == id)
                        .FirstOrDefaultAsync();
        }

        public Task<SaldoContaBancaria?> ObterSaldoContaBancariaPeloMesAsync(int contaBancariaId, DateTime data)
        {
            return Db.SaldosContasBancarias
                        .Where(s =>
                            s.ContaBancariaId == contaBancariaId
                            && s.Data == data
                        )
                        .FirstOrDefaultAsync();
        }

        public Task<SaldoContaBancaria?> ObterSaldoUltimoSaldoAnteriorDataAsync(int contaBancariaId, DateTime data)
        {
            return Db.SaldosContasBancarias
                        .Where(s => s.ContaBancariaId == contaBancariaId && s.Data < data)
                        .OrderByDescending(o => o.Data)
                        .FirstOrDefaultAsync();
        }

        public Task<List<SaldoContaBancaria>> ListarSaldosContasBancariasAsync(int grupoId, int? contaBancariaId, DateTime? dataInicial, DateTime? dataFinal)
        {
            return Db.SaldosContasBancarias
                        .Include(s => s.ContaBancaria)
                        .Where(s =>
                            s.ContaBancaria!.GrupoId == grupoId
                            && (contaBancariaId == null || s.ContaBancariaId == contaBancariaId.Value)
                            && (dataInicial == null || s.Data >= dataInicial.Value)
                            && (dataFinal == null || s.Data <= dataFinal.Value)
                        )
                        .OrderBy(s => s.ContaBancaria!.Descricao)
                        .ThenBy(s => s.Data)
                        .ToListAsync();
        }

        public Task<List<SaldoContaBancaria>> ListarSaldosContaRelatorioProjecaoSaldoAsync(int grupoId, DateTime data)
        {
            return Db.SaldosContasBancarias
                        .Include(s => s.ContaBancaria)
                        .Where(s =>
                            s.ContaBancaria!.GrupoId == grupoId
                            && s.ContaBancaria.RelatorioProjecaoSaldo
                            && s.ContaBancaria.Ativo
                            && s.Data == data
                        )
                        .ToListAsync();
        }

        public async Task<bool> ValidarExisteSaldoAnteriorAbertoAsync(int contaBancariaId, DateTime data)
        {
            return await Db.SaldosContasBancarias
                        .Where(s =>
                            s.ContaBancariaId == contaBancariaId
                            && s.Data < data
                            && !s.Fechado
                        )
                        .CountAsync() > 0;
        }

        public async Task<bool> ValidarExisteSaldoPosteriorFechadoAsync(int contaBancariaId, DateTime data)
        {
            return await Db.SaldosContasBancarias
                        .Where(s =>
                            s.ContaBancariaId == contaBancariaId
                            && s.Data > data
                            && s.Fechado
                        )
                        .CountAsync() > 0;
        }

        public async Task IncluirAsync(SaldoContaBancaria saldoContaBancaria)
        {
            await Db.SaldosContasBancarias.AddAsync(saldoContaBancaria);
        }

        public Task AlterarAsync(SaldoContaBancaria saldoContaBancaria)
        {
            return Task.Run(() => Db.SaldosContasBancarias.Update(saldoContaBancaria));
        }

        public async Task RemoverDaContaBancariaAsync(ContaBancaria contaBancaria)
        {
            var saldosContas = await Db.SaldosContasBancarias.Where(s => s.ContaBancariaId == contaBancaria.Id).ToListAsync();

            Db.SaldosContasBancarias.RemoveRange(saldosContas);
        }

        public async Task RemoverDoGrupoAsync(Grupo grupo)
        {
            var saldosContas = await Db.SaldosContasBancarias
                                                .Include(r => r.ContaBancaria)
                                                .Where(r => r.ContaBancaria!.GrupoId == grupo.Id)
                                                .ToListAsync();

            Db.SaldosContasBancarias.RemoveRange(saldosContas);
        }
    }
}