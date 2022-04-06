using Mangos.Dominio.Interfaces.Repository;
using Mangos.Dominio.Services;
using Mangos.Dominio.Services.User;
using Mangos.Mvc.Models;
using Mangos.Mvc.Models.ViewModels;
using Mangos.Mvc.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Mangos.Mvc.Controllers
{
    [Authorize(Roles = MangosClaimTypes.AdminRole)]
    public class LogController : BaseController
    {
        private readonly LogService _logService;
        private readonly ILogRepository _logRepository;

        public LogController(DataKeeperService dataKeeperService, IUserResolverService userResolverService, LogService logService, ILogRepository logRepository) : base(dataKeeperService, userResolverService)
        {
            _logService = logService;
            _logRepository = logRepository;
        }

        public async Task<IActionResult> Index()
        {
            var dataInicial = DateTime.Now.Date;
            var dataFinal = DateTime.Now.Date;

            ViewData["DataInicial"] = dataInicial.ToString("dd/MM/yyyy");
            ViewData["DataFinal"] = dataFinal.ToString("dd/MM/yyyy");

            var logs = await _logRepository.ListarLogsAsync(dataInicial, dataFinal);

            return View(logs);
        }

        public async Task<IActionResult> IndexLista(DateTime dataInicial, DateTime dataFinal)
        {
            var logs = await _logRepository.ListarLogsAsync(dataInicial, dataFinal);

            return PartialView(logs);
        }

        public async Task<IActionResult> Consulta(int id)
        {
            var log = await _logRepository.ObterLogAsync(id);

            if (log == null)
                return NotFound();

            var logDadosModel  =new LogConsultaModel(
                id: log.Id,
                dataHora: log.DataHora,
                logLevel: log.LogLevel,
                aplicacao: log.Aplicacao,
                categoryName: log.CategoryName,
                mensagem: log.Mensagem,
                exception: log.Exception
            );

            return View(logDadosModel);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Limpar([FromQuery] int? dias)
        {
            if (dias.HasValue && (!User.Identity!.IsAuthenticated || !User.IsInRole("Admin")))
                return ErroResult("Somente o administrador pode limpar os logs com período forçado");

            try
            {
                int logsExcluidos = await _logService.ExecutarLimpezaLogs(dias);

                return SucessoResult($"Limpeza dos logs executada com sucesso. {logsExcluidos} logs excluídos.");
            }
            catch (Exception ex)
            {
                return ErroResult($"Erro ao executar limpeza dos logs: {ex.Message}.");
            }
        }
    }
}