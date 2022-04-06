using Mangos.Dominio.Entities;
using Mangos.Dominio.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mangos.Infra.Repository
{
    public class MetaSaldoRepository : IMetaSaldoRepository
    {
        private readonly MangosDb Db;

        public MetaSaldoRepository(MangosDb db)
        {
            Db = db;
        }

        public Task<MetaSaldo?> ObterMetaSaldoAsync(int id)
        {
            return Db.MetasSaldo.Where(m => m.Id == id).FirstOrDefaultAsync();
        }

        public Task<List<MetaSaldo>> ListarMetasSaldoAsync(int grupoId, DateTime mesInicial, DateTime mesFinal)
        {
            return Db.MetasSaldo
                        .Where(o =>
                            o.GrupoId == grupoId
                            && o.Mes >= mesInicial
                            && o.Mes <= mesFinal
                        )
                        .ToListAsync();
        }

        public async Task IncluirAsync(MetaSaldo metaSaldo)
        {
            await Db.MetasSaldo.AddAsync(metaSaldo);
        }

        public Task AlterarAsync(MetaSaldo metaSaldo)
        {
            return Task.Run(() => Db.MetasSaldo.Update(metaSaldo));
        }

        public Task RemoverAsync(MetaSaldo metaSaldo)
        {
            return Task.Run(() => Db.MetasSaldo.Remove(metaSaldo));
        }

        public async Task RemoverDoGrupoAsync(Grupo grupo)
        {
            var metasSaldo = await Db.MetasSaldo.Where(s => s.GrupoId == grupo.Id).ToListAsync();

            Db.MetasSaldo.RemoveRange(metasSaldo);
        }
    }
}