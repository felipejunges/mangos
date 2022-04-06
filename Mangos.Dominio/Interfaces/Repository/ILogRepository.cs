using Mangos.Dominio.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mangos.Dominio.Interfaces.Repository
{
    public interface ILogRepository
    {
        Task<Log?> ObterLogAsync(int id);
        Task<List<Log>> ListarLogsAsync(DateTime dataInicial, DateTime dataFinal);
        Task<List<Log>> ListarLogsAnteriorAsync(DateTime dataMinima);
        Task IncluirLogAsync(Log log);
        Task RemoverAsyunc(IList<Log> log);
    }
}