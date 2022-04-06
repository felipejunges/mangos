using Mangos.Mvc.Models;
using Mangos.Mvc.Services;
using Mangos.Mvc.Services.User;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Mangos.Mvc.Utils
{
    public class CookieUpdateValidator
    {
        public static TimeSpan INTERVALO_ATUALIZACAO_SESSAO = TimeSpan.FromMinutes(1);

        public static async Task Validate(CookieValidatePrincipalContext context)
        {
            var dataKeeper = context.HttpContext.RequestServices.GetRequiredService<DataKeeperService>();

            var usuarioLogado = context.Principal;

            if (!usuarioLogado?.Identity?.IsAuthenticated ?? false)
            {
                context.RejectPrincipal();
                return;
            }

            // Neste momento, o Context injetado no UserResolverService ainda não está alimentado,
            //   por isso é necessário passar o Usuário recebido do contexto do Cookie.
            var userResolverService = new UserResolverService(usuarioLogado!);

            var data = await dataKeeper.GetData(userResolverService.UsuarioId, userResolverService.ChaveSessao);

            var dataHoraExpiracao = context.Properties?.ExpiresUtc?.LocalDateTime;

            await VerificarDeveAtualizarSessaoAcesso(data, context, dataHoraExpiracao, userResolverService.UsuarioId, userResolverService.ChaveSessao);
        }

        private static async Task VerificarDeveAtualizarSessaoAcesso(SessionData data, CookieValidatePrincipalContext context, DateTime? dataHoraExpiracao, int usuarioId, string chaveSessao)
        {
            if (dataHoraExpiracao == null)
                return;

            if (data.VerificarDeveAtualizarSessao(INTERVALO_ATUALIZACAO_SESSAO, dataHoraExpiracao.Value))
            {
                var sessaoAcessoMVCService = context.HttpContext.RequestServices.GetRequiredService<SessaoAcessoMVCService>();

                if (await sessaoAcessoMVCService.VerificarSessaoEstaDeslogadaAsync(usuarioId, chaveSessao))
                {
                    context.RejectPrincipal();
                    await context.HttpContext.SignOutAsync();
                }
                else if (data.VerificarSessaoEhNova())
                    await sessaoAcessoMVCService.AtualizarSessaoAcessoBrowser(usuarioId, chaveSessao, dataHoraExpiracao.Value);
                else
                    await sessaoAcessoMVCService.AtualizarSessaoAcesso(usuarioId, chaveSessao, dataHoraExpiracao.Value);

                data.MarcarAtualizacaoSessaoAcesso(dataHoraExpiracao.Value);
            }
        }
    }
}