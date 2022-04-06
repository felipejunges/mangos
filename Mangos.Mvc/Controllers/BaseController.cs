using Mangos.Dominio.Extensions;
using Mangos.Dominio.Services.User;
using Mangos.Mvc.Services;
using Mangos.Mvc.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Mangos.Mvc.Controllers
{
    public class BaseController : Controller
    {
        protected const int ITENS_POR_PAGINA_PADRAO = 15;
        private readonly DataKeeperService _dataKeeperService;

        protected readonly IUserResolverService _userResolverService;

        public BaseController(DataKeeperService dataKeeperService, IUserResolverService userResolverService)
        {
            _dataKeeperService = dataKeeperService;
            _userResolverService = userResolverService;
        }

        private static object? GetValor(string nomePropriedade, object obj)
        {
            PropertyInfo? prop = obj.GetType().GetProperty(nomePropriedade);
            return prop?.GetValue(obj, null);
        }

        protected ActionResult OkResult()
        {
            return Content("OK");
        }

        protected ActionResult SucessoResult(string mensagem)
        {
            return MangoResult(true, mensagem, null);
        }

        protected ActionResult ErroResult(string mensagem, Exception? exception = null)
        {
            return MangoResult(false, mensagem, exception);
        }

        protected ActionResult MangoResult(bool sucesso, string mensagem, Exception? exception = null)
        {
            return Json(new { sucesso, mensagem, excecao = exception?.Message });
        }

        protected ActionResult NewtonJsonResult(object dados)
        {
            return Content(JsonConvert.SerializeObject(dados));
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            base.OnActionExecuting(context);

            if (User.Identity!.IsAuthenticated)
            {
                var data = await _dataKeeperService.GetData(_userResolverService.UsuarioId, _userResolverService.ChaveSessao);

                var tempoExpiracao = data.DataHoraExpiracaoSessaoAcesso != null
                    ? (data.DataHoraExpiracaoSessaoAcesso.Value.RemoveMilliseconds() - DateTime.Now.RemoveMilliseconds()).TotalSeconds.ToString("f0")
                    : "0";

                ViewData["NomeUsuario"] = data.NomeUsuario;
                ViewData["TempoExpiracao"] = tempoExpiracao;
                ViewData["NotificacoesTopo"] = data.Notificacoes;
                ViewData["TempoSessionData"] = data.DataHoraCriacao.ToTimeDiffDuplo();
            }
            else
            {
                ViewData["NomeUsuario"] = string.Empty;
                ViewData["TempoExpiracao"] = "0";
                ViewData["NotificacoesTopo"] = null;
                ViewData["TempoSessionData"] = string.Empty;
            }

            ViewData["DataHoraServidor"] = DateTime.Now.ToString("dd/MM/yyyy HH:mm");

            await next();
        }
    }
}