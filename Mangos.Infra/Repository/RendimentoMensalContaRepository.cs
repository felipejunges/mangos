using Mangos.Dominio.Entities;
using Mangos.Dominio.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mangos.Infra.Repository
{
    public class RendimentoMensalContaRepository : IRendimentoMensalContaRepository
    {
        private readonly MangosDb Db;

        public RendimentoMensalContaRepository(MangosDb db)
        {
            Db = db;
        }

        public Task<RendimentoMensalConta?> ObterRendimentoMensalContaAsync(int id)
        {
            return Db.RendimentosMensaisContas
                            .Include(r => r.ContaBancaria)
                            .Where(r => r.Id == id)
                            .FirstOrDefaultAsync();
        }

        public Task<RendimentoMensalConta?> ObterRendimentoMensalContaNoTrackingAsync(int id)
        {
            return Db.RendimentosMensaisContas
                            .Include(r => r.ContaBancaria)
                            .Where(r => r.Id == id)
                            .AsNoTracking()
                            .FirstOrDefaultAsync();
        }

        public Task<DateTime?> ObterMenorDataDaContaBancariaAsync(int contaBancariaId)
        {
            return Db.RendimentosMensaisContas.Where(r => r.ContaBancariaId == contaBancariaId).MinAsync(r => (DateTime?)r.MesReferencia);
        }

        public Task<decimal> ObterValorRendimentosContaNoPeriodoAsync(int contaBancariaId, DateTime dataInicial, DateTime dataFinal)
        {
            return Db.RendimentosMensaisContas
                        .Where(r =>
                            r.ContaBancariaId == contaBancariaId
                            && r.MesReferencia >= dataInicial
                            && r.MesReferencia <= dataFinal
                        )
                        .SumAsync(r => r.Valor);
        }

        public Task<List<RendimentoMensalConta>> ListarRendimentosMensaisContaAsync(int grupoId, int? contaBancariaId, DateTime dataInicial, DateTime dataFinal)
        {
            return Db.RendimentosMensaisContas
                        .Include(r => r.ContaBancaria)
                        .Where(r =>
                            r.ContaBancaria!.GrupoId == grupoId
                            && (contaBancariaId == null || r.ContaBancariaId == contaBancariaId.Value)
                            && r.MesReferencia >= dataInicial
                            && r.MesReferencia <= dataFinal
                        )
                        .OrderBy(r => r.MesReferencia)
                        .ThenBy(r => r.ContaBancaria!.Descricao)
                        .ToListAsync();
        }

        public Task<List<RendimentoMensalConta>> ListarRendimentosContaRelatorioExtratoAsync(int grupoId, int contaBancariaId, DateTime dataInicial, DateTime dataFinal)
        {
            return Db.RendimentosMensaisContas
                        .Include(r => r.ContaBancaria)
                        .Where(r =>
                            r.ContaBancaria!.GrupoId == grupoId
                            && r.ContaBancariaId == contaBancariaId
                            && r.MesReferencia >= dataInicial
                            && r.MesReferencia <= dataFinal
                        )
                        .ToListAsync();
        }

        public Task<List<RendimentoMensalConta>> ListarRendimentosMensaisRelatorioProjecaoAsync(int grupoId, DateTime mesInicial, DateTime mesFinal)
        {
            return Db.RendimentosMensaisContas
                        .Include(r => r.ContaBancaria)
                        .Where(r =>
                            r.ContaBancaria!.GrupoId == grupoId
                            && r.MesReferencia >= mesInicial
                            && r.MesReferencia <= mesFinal
                        )
                        .ToListAsync();
        }

        public Task<List<RendimentoMensalConta>> ListarRendimentosMensaisRelatorioCategoriaAsync(int grupoId, string situacao, string tipo, DateTime dataInicial, DateTime dataFinal)
        {
            return Db.RendimentosMensaisContas
                        .Include(r => r.ContaBancaria)
                        #nullable disable
                        .ThenInclude(r => r.CategoriaRendimentoMensal)
                        #nullable enable
                        .Where(r =>
                            r.ContaBancaria!.GrupoId == grupoId
                            && r.ContaBancaria.CategoriaRendimentoMensalId != null
                            && r.ContaBancaria.CategoriaRendimentoMensal!.RelatorioTotal
                            && (situacao == "T" || situacao == "R")
                            && (tipo == "T" || tipo == "R")
                            && r.MesReferencia >= dataInicial
                            && r.MesReferencia <= dataFinal
                        )
                        .ToListAsync();
        }

        public async Task IncluirAsync(RendimentoMensalConta rendimentoMensalConta)
        {
            await Db.RendimentosMensaisContas.AddAsync(rendimentoMensalConta);
        }

        public Task AtualizarAsync(RendimentoMensalConta rendimentoMensalConta)
        {
            return Task.Run(() => Db.RendimentosMensaisContas.Update(rendimentoMensalConta));
        }

        public Task RemoverAsync(RendimentoMensalConta rendimentoMensalConta)
        {
            return Task.Run(() => Db.RendimentosMensaisContas.Remove(rendimentoMensalConta));
        }

        public async Task RemoverDaContaBancariaAsync(ContaBancaria contaBancaria)
        {
            var rendimentosMensais = await Db.RendimentosMensaisContas.Where(s => s.ContaBancariaId == contaBancaria.Id).ToListAsync();

            Db.RendimentosMensaisContas.RemoveRange(rendimentosMensais);
        }

        public async Task RemoverDoGrupoAsync(Grupo grupo)
        {
            var rendimentosMensais = await Db.RendimentosMensaisContas
                                                .Include(r => r.ContaBancaria)
                                                .Where(r => r.ContaBancaria!.GrupoId == grupo.Id)
                                                .ToListAsync();

            Db.RendimentosMensaisContas.RemoveRange(rendimentosMensais);
        }
    }
}