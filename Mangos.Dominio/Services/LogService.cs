using Mangos.Dominio.Interfaces;
using Mangos.Dominio.Interfaces.Repository;
using Mangos.Dominio.Settings;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Mangos.Dominio.Services
{
    public class LogService
    {
        private readonly LimpezaLogsSettings _limpezaLogsSettings;
        private readonly ILogRepository _logRepository;
        private readonly IUnitOfWork _unitOfWork;

        public LogService(LimpezaLogsSettings limpezaLogsSettings, ILogRepository logRepository, IUnitOfWork unitOfWork)
        {
            _limpezaLogsSettings = limpezaLogsSettings;
            _logRepository = logRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<int> ExecutarLimpezaLogs(int? diasExcluir = null)
        {
            if (diasExcluir == null)
                diasExcluir = _limpezaLogsSettings.NumeroDiasExcluirLogs;

            var dataMinimaManter = DateTime.Now.Date.AddDays(diasExcluir.Value * -1);

            var logsExcluir = await _logRepository.ListarLogsAnteriorAsync(dataMinimaManter);

            int quantidadeLogs = logsExcluir.Count();

            if (logsExcluir.Any())
            {
                await _logRepository.RemoverAsyunc(logsExcluir);
                await _unitOfWork.SaveChangesAsync();
            }

            return quantidadeLogs;
        }
    }
}