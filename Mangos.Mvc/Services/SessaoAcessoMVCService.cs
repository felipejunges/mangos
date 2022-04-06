using Mangos.Dominio.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using Wangkanai.Detection.Services;

namespace Mangos.Mvc.Services
{
    public class SessaoAcessoMVCService
    {
        private readonly HttpContext _context;
        private readonly IDetectionService _detectionService;
        private readonly SessaoAcessoService _sessaoAcessoService;

        public SessaoAcessoMVCService(IHttpContextAccessor context, SessaoAcessoService sessaoAcessoService, IDetectionService detectionService)
        {
            _context = context.HttpContext!;
            _sessaoAcessoService = sessaoAcessoService;
            _detectionService = detectionService;
        }

        public async Task IncluirSessaoAcesso(int usuarioId, string chave, bool persistente, DateTime dataHoraExpiracao)
        {
            var (isMobile, browser, userAgent, ipAddress) = BuscarDadosBrowser();

            await _sessaoAcessoService.IncluirAcesso(usuarioId, chave, persistente, dataHoraExpiracao, isMobile, browser, userAgent, ipAddress);
        }

        public async Task AtualizarSessaoAcesso(int usuarioId, string chave, DateTime dataHoraExpiracao)
        {
            await _sessaoAcessoService.AtualizarAcesso(usuarioId, chave, dataHoraExpiracao);
        }

        public async Task AtualizarSessaoAcessoBrowser(int usuarioId, string chave, DateTime dataHoraExpiracao)
        {
            var (_, browser, userAgent, ipAddress) = BuscarDadosBrowser();

            await _sessaoAcessoService.AtualizarAcessoBrowser(usuarioId, chave, ipAddress, browser, userAgent, dataHoraExpiracao);
        }

        public async Task AtualizarLogoutSessaoAcesso(int usuarioId, string chave)
        {
            await _sessaoAcessoService.AtualizarLogoutAcesso(usuarioId, chave);
        }

        public (bool isMobile, string browser, string userAgent, string ipAddress) BuscarDadosBrowser()
        {
            bool isMobile = _detectionService.Device.Type != Wangkanai.Detection.Models.Device.Desktop;
            var browser = _detectionService.Browser.Name.ToString() + " " + this._detectionService.Browser.Version.Major.ToString();
            var userAgent = _detectionService.UserAgent.ToString();
            var ipAddress = _context.Request.HttpContext.Connection.RemoteIpAddress?.ToString() ?? "0.0.0.0";

            return (isMobile, browser, userAgent, ipAddress);
        }

        public Task<bool> VerificarSessaoEstaDeslogadaAsync(int usuarioId, string chave)
        {
            return _sessaoAcessoService.VerificarSessaoEstaDeslogadaAsync(usuarioId, chave);
        }
    }
}