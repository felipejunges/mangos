using Mangos.Dominio.Interfaces.Repository;
using Mangos.Dominio.Services;
using Mangos.Dominio.Services.User;
using Mangos.Dominio.Settings;
using Mangos.Mvc.Models;
using Mangos.Mvc.Models.Mappers;
using Mangos.Mvc.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Mangos.Mvc.Controllers
{
    [Authorize(Roles = MangosClaimTypes.AdminRole)]
    public class SessaoAcessoController : BaseController
    {
        private readonly SessaoAcessoService _sessaoAcessoService;
        private readonly ISessaoAcessoRepository _sessaoAcessoRepository;
        private readonly ExpiracaoLoginSettings _expiracaoLoginSettings;

        public SessaoAcessoController(SessaoAcessoService sessaoAcessoService, ISessaoAcessoRepository sessaoAcessoRepository, ExpiracaoLoginSettings expiracaoLoginSettings, DataKeeperService dataKeeperService, IUserResolverService userResolverService) : base(dataKeeperService, userResolverService)
        {
            _sessaoAcessoService = sessaoAcessoService;
            _sessaoAcessoRepository = sessaoAcessoRepository;
            _expiracaoLoginSettings = expiracaoLoginSettings;
        }

        public async Task<IActionResult> Index()
        {
            DateTime dataInicial = DateTime.Now.AddSeconds(this._expiracaoLoginSettings.Persistente * -1).Date;
            DateTime dataFinal = DateTime.Now.Date;
            bool buscarSessoesDeslogadas = false;

            ViewData["DataInicial"] = dataInicial.ToString("dd/MM/yyyy");
            ViewData["DataFinal"] = dataFinal.ToString("dd/MM/yyyy");
            ViewData["BuscarSessoesDeslogadas"] = buscarSessoesDeslogadas;

            var sessoesAcesso = await _sessaoAcessoRepository.ListarSessoesAcessoAsync(dataInicial, dataFinal, buscarSessoesDeslogadas);

            var sessoesAcessoModel = SessaoAcessoMappers.ToListaModel(sessoesAcesso);

            return View(sessoesAcessoModel);
        }

        public async Task<IActionResult> IndexLista(DateTime dataInicial, DateTime dataFinal, bool buscarSessoesDeslogadas)
        {
            var sessoesAcesso = await _sessaoAcessoRepository.ListarSessoesAcessoAsync(dataInicial, dataFinal, buscarSessoesDeslogadas);

            var sessoesAcessoModel = SessaoAcessoMappers.ToListaModel(sessoesAcesso);

            return PartialView(sessoesAcessoModel);
        }

        public async Task<IActionResult> Deslogar(int id)
        {
            if (await _sessaoAcessoService.DeslogarAsync(id))
                return SucessoResult("Sessão deslogada com sucesso.");
            else
                return ErroResult("Sessão não finalizada: registro inexistente ou já deslogado.");
        }
    }
}