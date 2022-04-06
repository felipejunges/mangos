using Mangos.Dominio.Enums;
using Mangos.Dominio.Services;
using Mangos.Dominio.Services.User;
using Mangos.Mvc.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Mangos.Mvc.Controllers.Consultas
{
    [Authorize]
    public class ConsultaVencimentoController : BaseController
    {
        private readonly ConsultaVencimentoService _consultaVencimentoService;
        private readonly DataKeeperService _dataKeeperService;

        public ConsultaVencimentoController(DataKeeperService dataKeeperService, IUserResolverService userResolverService, ConsultaVencimentoService consultaVencimentoService) : base(dataKeeperService, userResolverService)
        {
            this._consultaVencimentoService = consultaVencimentoService;
            this._dataKeeperService = dataKeeperService;
        }

        public async Task<IActionResult> Index()
        {
            var sessionData = await _dataKeeperService.GetData(_userResolverService.UsuarioId, _userResolverService.ChaveSessao);

            DateTime dataFinal = DateTime.Now.Date.AddDays(sessionData.DiasAlertaVencimentos);

            ViewData["DataFinal"] = dataFinal.ToString("dd/MM/yyyy");
            ViewData["AvisarReceitas"] = sessionData.TipoAlertaVencimentos == TipoAvisoVencimentosUsuario.Ambos || sessionData.TipoAlertaVencimentos == TipoAvisoVencimentosUsuario.Receitas;
            ViewData["AvisarDespesas"] = sessionData.TipoAlertaVencimentos == TipoAvisoVencimentosUsuario.Ambos || sessionData.TipoAlertaVencimentos == TipoAvisoVencimentosUsuario.Despesas;

            var vencimentos = await _consultaVencimentoService.ListarConsultaVencimentosAsync(_userResolverService.GrupoId, dataFinal);

            return View(vencimentos);
        }

        public async Task<IActionResult> IndexLista(string dataFinal)
        {
            var sessionData = await _dataKeeperService.GetData(_userResolverService.UsuarioId, _userResolverService.ChaveSessao);

            var dataFinalVencimentos = Convert.ToDateTime(dataFinal);

            var vencimentos = await _consultaVencimentoService.ListarConsultaVencimentosAsync(_userResolverService.GrupoId, dataFinalVencimentos);

            //
            ViewData["AvisarReceitas"] = sessionData.TipoAlertaVencimentos == TipoAvisoVencimentosUsuario.Ambos || sessionData.TipoAlertaVencimentos == TipoAvisoVencimentosUsuario.Receitas;
            ViewData["AvisarDespesas"] = sessionData.TipoAlertaVencimentos == TipoAvisoVencimentosUsuario.Ambos || sessionData.TipoAlertaVencimentos == TipoAvisoVencimentosUsuario.Despesas;

            return PartialView(vencimentos);
        }
    }
}