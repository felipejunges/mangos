using Mangos.Dominio.Entities;
using Mangos.Dominio.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mangos.Infra.Repository
{
    public class TransferenciaContaRepository : ITransferenciaContaRepository
    {
        private readonly MangosDb Db;

        public TransferenciaContaRepository(MangosDb db)
        {
            Db = db;
        }

        public Task<TransferenciaConta?> ObterTransferenciaContaAsync(int id)
        {
            return Db.TransferenciasContas
                        .Where(t => t.Id == id)
                        .FirstOrDefaultAsync();
        }

        public Task<TransferenciaConta?> ObterTransferenciaContaNoTrackingAsync(int id)
        {
            return Db.TransferenciasContas
                        .Where(t => t.Id == id)
                        .AsNoTracking()
                        .FirstOrDefaultAsync();
        }

        public Task<List<TransferenciaConta>> ListarTransferenciasContaAsync(int grupoId, string? descricao, DateTime dataInicial, DateTime dataFinal)
        {
            return Db.TransferenciasContas
                        .Include(t => t.ContaBancariaOrigem)
                        .Include(t => t.ContaBancariaDestino)
                        .Where(t =>
                            t.GrupoId == grupoId
                            && (string.IsNullOrEmpty(descricao) || t.Descricao.Contains(descricao))
                            && (
                                (t.DataDebito != null && t.DataDebito >= dataInicial && t.DataDebito <= dataFinal)
                                || (t.DataCredito != null && t.DataCredito >= dataInicial && t.DataCredito <= dataFinal)
                            )
                        )
                        .OrderBy(t => t.DataDebito != null ? t.DataDebito : t.DataCredito)
                        .ThenBy(t => t.DataHoraCadastro)
                        .ToListAsync();
        }

        public Task<List<TransferenciaConta>> ListarTransferenciasContaOrigemRelatorioExtratoAsync(int grupoId, int contaBancariaId, DateTime dataInicial, DateTime dataFinal)
        {
            return Db.TransferenciasContas
                        .Where(t =>
                            t.GrupoId == grupoId
                            && t.ContaBancariaOrigemId == contaBancariaId
                            && t.DataDebito != null
                            && t.DataDebito >= dataInicial
                            && t.DataDebito <= dataFinal
                        )
                        .ToListAsync();
        }

        public Task<List<TransferenciaConta>> ListarTransferenciasContaDestinoRelatorioExtratoAsync(int grupoId, int contaBancariaId, DateTime dataInicial, DateTime dataFinal)
        {
            return Db.TransferenciasContas
                        .Where(t =>
                            t.GrupoId == grupoId
                            && t.ContaBancariaDestinoId == contaBancariaId
                            && t.DataCredito != null
                            && t.DataCredito >= dataInicial
                            && t.DataCredito <= dataFinal
                        )
                        .ToListAsync();
        }

        public Task<DateTime?> ObterMenorDataDaContaBancariaDestinoAsync(int contaBancariaId)
        {
            return Db.TransferenciasContas.Where(t => t.ContaBancariaOrigemId == contaBancariaId).MinAsync(l => l.DataDebito);
        }

        public Task<DateTime?> ObterMenorDataDaContaBancariaOrigemAsync(int contaBancariaId)
        {
            return Db.TransferenciasContas.Where(t => t.ContaBancariaDestinoId == contaBancariaId).MinAsync(l => l.DataCredito);
        }

        public Task<decimal> ObterValorTransferenciasContaNoPeriodo(int contaBancariaId, DateTime dataInicial, DateTime dataFinal)
        {
            return Db.TransferenciasContas
                            .Where(t =>
                                (
                                    t.ContaBancariaOrigemId == contaBancariaId
                                    && t.DataDebito != null
                                    && t.DataDebito >= dataInicial
                                    && t.DataDebito <= dataFinal
                                ) || (
                                    t.ContaBancariaDestinoId == contaBancariaId
                                    && t.DataCredito != null
                                    && t.DataCredito >= dataInicial
                                    && t.DataCredito <= dataFinal
                                )
                            )
                            .SumAsync(t =>
                                t.ContaBancariaOrigemId == contaBancariaId
                                    ? t.Valor * -1
                                    : t.ContaBancariaDestinoId == contaBancariaId
                                        ? t.Valor
                                        : 0M);
        }

        public async Task IncluirAsync(TransferenciaConta transferenciaConta)
        {
            await Db.TransferenciasContas.AddAsync(transferenciaConta);
        }

        public Task AlterarAsync(TransferenciaConta transferenciaConta)
        {
            return Task.Run(() => Db.TransferenciasContas.Update(transferenciaConta));
        }

        public Task RemoverAsync(TransferenciaConta transferenciaConta)
        {
            return Task.Run(() => Db.TransferenciasContas.Remove(transferenciaConta));
        }

        public async Task RemoverDoGrupoAsync(Grupo grupo)
        {
            var transferenciasConta = await Db.TransferenciasContas.Where(s => s.GrupoId == grupo.Id).ToListAsync();

            Db.TransferenciasContas.RemoveRange(transferenciasConta);
        }
    }
}