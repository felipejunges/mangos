using Mangos.Dominio.Entities;
using Mangos.Dominio.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mangos.Infra.Repository
{
    public class MetaMovimentacaoRepository : IMetaMovimentacaoRepository
    {
        private readonly MangosDb Db;

        public MetaMovimentacaoRepository(MangosDb db)
        {
            Db = db;
        }

        public Task<MetaMovimentacao?> ObterMetaMovimentacaoAsync(int id)
        {
            return Db.MetasMovimentacao.Where(m => m.Id == id).FirstOrDefaultAsync();
        }

        public Task<List<MetaMovimentacao>> ListarMetasMovimentacaoAsync(int grupoId, DateTime mesInicial, DateTime mesFinal)
        {
            return Db.MetasMovimentacao
                    .Where(m =>
                        m.GrupoId == grupoId
                        && (
                            (m.MesInicial >= mesInicial && m.MesInicial <= mesFinal)
                            || (m.MesFinal >= mesInicial && m.MesFinal <= mesFinal)
                            || (mesInicial >= m.MesInicial && mesInicial <= m.MesFinal)
                            || (mesFinal >= m.MesInicial && mesFinal <= m.MesFinal)
                        )
                    )
                    .OrderBy(m => m.MesInicial)
                    .ThenBy(m => m.MesFinal)
                    .ToListAsync();
        }

        public async Task IncluirAsync(MetaMovimentacao metaMovimentacao)
        {
            await Db.MetasMovimentacao.AddAsync(metaMovimentacao);
        }

        public Task AlterarAsync(MetaMovimentacao metaMovimentacao)
        {
            return Task.Run(() => Db.MetasMovimentacao.Update(metaMovimentacao));
        }

        public Task RemoverAsync(MetaMovimentacao metaMovimentacao)
        {
            return Task.Run(() => Db.MetasMovimentacao.Remove(metaMovimentacao));
        }

        public async Task RemoverDoGrupoAsync(Grupo grupo)
        {
            var metasMovimentacao = await Db.MetasMovimentacao.Where(s => s.GrupoId == grupo.Id).ToListAsync();

            Db.MetasMovimentacao.RemoveRange(metasMovimentacao);
        }
    }
}