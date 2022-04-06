using Mangos.Dominio.Entities;
using Mangos.Dominio.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mangos.Infra.Repository
{
    public class LogRepository : ILogRepository
    {
        private readonly MangosDb Db;

        public LogRepository(MangosDb db)
        {
            Db = db;
        }

        public Task<Log?> ObterLogAsync(int id)
        {
            return Db.Logs
                    .Where(l => l.Id == id)
                    .FirstOrDefaultAsync();
        }

        public Task<List<Log>> ListarLogsAsync(DateTime dataInicial, DateTime dataFinal)
        {
            return Db.Logs
                    .Where(l =>
                        l.DataHora >= dataInicial
                        && l.DataHora < dataFinal.AddDays(1)
                    )
                    .OrderBy(l => l.DataHora)
                    .ToListAsync();
        }

        public Task<List<Log>> ListarLogsAnteriorAsync(DateTime dataMinima)
        {
            return Db.Logs
                    .Where(l => l.DataHora < dataMinima)
                    .OrderBy(l => l.DataHora)
                    .ToListAsync();
        }

        public async Task IncluirLogAsync(Log log)
        {
            await Db.Logs.AddAsync(log);
        }

        public async Task RemoverAsyunc(IList<Log> log)
        {
            await Task.Run(() => Db.Logs.RemoveRange(log));
        }
    }
}