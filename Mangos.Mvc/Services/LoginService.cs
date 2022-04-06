using Mangos.Dominio.Entities;
using Mangos.Dominio.Services;
using Mangos.Dominio.Services.User;
using Mangos.Dominio.Settings;
using Mangos.Dominio.Utils;
using Mangos.Mvc.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Mangos.Mvc.Services
{
    public class LoginService
    {
        private readonly ILogger<LoginService> _logger;
        private readonly SessaoAcessoMVCService _sessaoAcessoMVCService;
        private readonly ExpiracaoLoginSettings _expiracaoLoginSettings;
        private readonly UsuarioService _usuarioService;
        private readonly DataKeeperService _dataKeeperService;
        private readonly HttpContext _context;
        private readonly IUserResolverService _userResolverService;

        public LoginService(ILogger<LoginService> logger, SessaoAcessoMVCService sessaoAcessoMVCService, ExpiracaoLoginSettings expiracaoLoginSettings, UsuarioService usuarioService, DataKeeperService dataKeeperService, IHttpContextAccessor contextAccessor, IUserResolverService userResolverService)
        {
            _logger = logger;
            _sessaoAcessoMVCService = sessaoAcessoMVCService;
            _expiracaoLoginSettings = expiracaoLoginSettings;
            _usuarioService = usuarioService;
            _dataKeeperService = dataKeeperService;
            _context = contextAccessor.HttpContext!;
            _userResolverService = userResolverService;
        }

        public async Task<NotificationResult> LogarUsuario(LoginModel model)
        {
            var result = new NotificationResult();

            Usuario? usuario = null;

            if (model.Email is not null && model.Senha is not null)
                usuario = await _usuarioService.ObterUsuarioPeloLoginSenhaAsync(model.Email, model.Senha);

            if (usuario == null)
            {
                _logger.LogWarning("E-mail e/ou senha inválido(s). E-mail: {Email}.", model.Email);
                result.AddNotification("E-mail e/ou senha inválido(s)");
            }
            else
            {
                await _usuarioService.ZerarTokenSenha(usuario);

                await LogarUsuario(usuario, model.LembrarMe);
            }

            return result;
        }

        public async Task LogarUsuario(Usuario usuario, bool persistente)
        {
            int segundosExpiracao = persistente ? this._expiracaoLoginSettings.Persistente : this._expiracaoLoginSettings.Normal;
            DateTime dataHoraExpiracao = DateTime.UtcNow.AddSeconds(segundosExpiracao);

            string chave = Guid.NewGuid().ToString();

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Name, usuario.Nome),
                new Claim(ClaimTypes.Email, usuario.Email),
                new Claim(MangosClaimTypes.ChaveSessao, chave),
                new Claim(MangosClaimTypes.GrupoLogado, usuario.GrupoId.ToString())
            };

            if (usuario.Admin)
                claims.Add(new Claim(ClaimTypes.Role, MangosClaimTypes.AdminRole));

            //
            await _sessaoAcessoMVCService.IncluirSessaoAcesso(usuario.Id, chave, persistente, dataHoraExpiracao);

            //
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await _context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity), new AuthenticationProperties()
            {
                IsPersistent = persistente,
                ExpiresUtc = dataHoraExpiracao
            });
        }

        public async Task DeslogarUsuario()
        {
            await _sessaoAcessoMVCService.AtualizarLogoutSessaoAcesso(_userResolverService.UsuarioId, _userResolverService.ChaveSessao);

            _dataKeeperService.Invalidar(_userResolverService.ChaveSessao);

            await _context.SignOutAsync();
        }
    }
}