using Mangos.Api.Configuration;
using Mangos.Api.Models;
using Mangos.Dominio.Entities;
using Mangos.Dominio.Interfaces.Repository;
using Mangos.Dominio.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Mangos.Api.Services
{
    public class LoginService
    {
        private readonly ILogger<LoginService> _logger;
        private readonly JwtService _jwtService;
        private readonly UsuarioService _usuarioService;
        private readonly DispositivoConectadoService _dispositivoConectadoService;
        private readonly IDispositivoConectadoRepository _dispositivoConectadoRepository;
        private readonly TokenConfigurations _tokenConfigurations;

        public LoginService(ILogger<LoginService> logger, JwtService jwtService, UsuarioService usuarioService, DispositivoConectadoService dispositivoConectadoService, IDispositivoConectadoRepository dispositivoConectadoRepository, TokenConfigurations tokenConfigurations)
        {
            _logger = logger;
            _jwtService = jwtService;
            _usuarioService = usuarioService;
            _dispositivoConectadoService = dispositivoConectadoService;
            _dispositivoConectadoRepository = dispositivoConectadoRepository;
            _tokenConfigurations = tokenConfigurations;
        }

        public async Task<JwtToken?> ObterLoginToken(LoginModel loginModel)
        {
            var usuario = await _usuarioService.ObterUsuarioPeloLoginSenhaAsync(loginModel.Email, loginModel.Senha);

            if (usuario != null)
            {
                var jwtToken = await CriarToken(usuario);

                return jwtToken;
            }
            else
            {
                _logger.LogWarning("E-mail e/ou senha inválido(s). E-mail: {Email}.", loginModel.Email);

                return null;
            }
        }

        public async Task<JwtToken?> ObterNovoTokenPeloRefresh(UsuarioRefreshTokenModel usuarioModel)
        {
            var jwtToken = await AtualizarToken(usuarioModel.AuthenticationToken, usuarioModel.RefreshToken);

            if (jwtToken != null)
                return jwtToken;
            else
            {
                return null;
            }
        }

        private async Task<JwtToken> CriarToken(Usuario usuario)
        {
            string identificador = Guid.NewGuid().ToString("N");

            var dataHoraExpiracaoRefreshToken = DateTime.Now.AddHours(_tokenConfigurations.TempoRefreshToken);

            var jwtToken = _jwtService.GenerateJwtToken(usuario.Email, usuario.Nome, identificador, usuario.Id, usuario.GrupoId, dataHoraExpiracaoRefreshToken);

            await IncluirDispositivoConectado(usuario.Id, identificador, jwtToken.RefreshToken, dataHoraExpiracaoRefreshToken);

            return jwtToken;
        }

        public async Task<JwtToken?> AtualizarToken(string token, string refreshToken)
        {
            var principal = _jwtService.GetPrincipalFromExpiredToken(token);

            var usuarioId = int.Parse(principal.Identity?.Name ?? "0");
            var identificador = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var email = principal.FindFirst(ClaimTypes.Email)?.Value;
            var nome = principal.FindFirst(ClaimTypes.GivenName)?.Value;
            var grupoId = int.Parse(principal.FindFirst(MangosApiClaimTypes.GrupoIdLogado)?.Value ?? "0");

            if (identificador is null || email is null || nome is null)
            {
                return null;
            }

            var dispositivoConectado = await _dispositivoConectadoRepository.ObterDispositivoConectadoAsync(identificador, usuarioId);
            if (dispositivoConectado == null || dispositivoConectado.RefreshToken != refreshToken || dispositivoConectado.EstaExpirada())
            {
                _logger.LogWarning("Token inválido para Refresh. E-mail: {Email}.", email);

                return null;
            }

            var dataHoraExpiracaoRefreshToken = DateTime.Now.AddHours(_tokenConfigurations.TempoRefreshToken);

            var jwtToken = _jwtService.GenerateJwtToken(email, nome, identificador, usuarioId, grupoId, dataHoraExpiracaoRefreshToken);

            await AlterarDispositivoConectado(usuarioId, identificador, jwtToken.RefreshToken, dataHoraExpiracaoRefreshToken);

            return jwtToken;
        }

        private async Task IncluirDispositivoConectado(int usuarioId, string identificador, string refreshToken, DateTime dataHoraExpiracao)
        {
            string ip = "TODO"; // TODO: api - setar
            string sistema = "TODO";

            await _dispositivoConectadoService.Incluir(usuarioId, identificador, refreshToken, dataHoraExpiracao, ip, sistema);
        }

        private async Task AlterarDispositivoConectado(int usuarioId, string identificador, string refreshToken, DateTime dataHoraExpiracao)
        {
            await _dispositivoConectadoService.Alterar(usuarioId, identificador, refreshToken, dataHoraExpiracao);
        }
    }
}