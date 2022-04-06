using Mangos.Dominio.Entities;
using Mangos.Dominio.Interfaces;
using Mangos.Dominio.Interfaces.Repository;
using System;
using System.Threading.Tasks;

namespace Mangos.Dominio.Services
{
    public class SessaoAcessoService
    {
        private readonly ISessaoAcessoRepository _sessaoAcessoRepository;
        private readonly IUnitOfWork _unitOfWork;

        public SessaoAcessoService(ISessaoAcessoRepository sessaoAcessoRepository, IUnitOfWork unitOfWork)
        {
            _sessaoAcessoRepository = sessaoAcessoRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> VerificarSessaoEstaDeslogadaAsync(int usuarioId, string chave)
        {
            var sessaoAcesso = await _sessaoAcessoRepository.ObterSessaoAcessoUsuarioSessaoAsync(usuarioId, chave);

            return sessaoAcesso == null || sessaoAcesso.Logout;
        }

        public async Task IncluirAcesso(int usuarioId, string chave, bool persistente, DateTime dataHoraExpiracao, bool isMobile, string browser, string userAgent, string ip)
        {
            var sessaoAcesso = await _sessaoAcessoRepository.ObterSessaoAcessoUsuarioSessaoAsync(usuarioId, chave);

            if (sessaoAcesso != null)
                return;

            sessaoAcesso = new SessaoAcesso(
                id: 0,
                usuarioId: usuarioId,
                chave: chave,
                dataHoraCriacao: DateTime.Now,
                dataHoraAtualizacao: DateTime.Now,
                dataHoraExpiracao: dataHoraExpiracao,
                persistente: persistente,
                ip: ip,
                browser: browser,
                userAgent: userAgent,
                isMobile: isMobile,
                logout: false
            );

            await _sessaoAcessoRepository.IncluirAsync(sessaoAcesso);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task AtualizarAcesso(int usuarioId, string chave, DateTime dataHoraExpiracao)
        {
            var sessaoAcesso = await _sessaoAcessoRepository.ObterSessaoAcessoUsuarioSessaoAsync(usuarioId, chave);

            if (sessaoAcesso == null)
                return;

            sessaoAcesso.DataHoraAtualizacao = DateTime.Now;
            sessaoAcesso.DataHoraExpiracao = dataHoraExpiracao;

            await _sessaoAcessoRepository.AlterarAsync(sessaoAcesso);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task AtualizarAcessoBrowser(int usuarioId, string chave, string ip, string browser, string userAgent, DateTime dataHoraExpiracao)
        {
            var sessaoAcesso = await _sessaoAcessoRepository.ObterSessaoAcessoUsuarioSessaoAsync(usuarioId, chave);

            if (sessaoAcesso == null)
                return;

            sessaoAcesso.DataHoraAtualizacao = DateTime.Now;
            sessaoAcesso.DataHoraExpiracao = dataHoraExpiracao;
            sessaoAcesso.Ip = ip;
            sessaoAcesso.Browser = browser;
            sessaoAcesso.UserAgent = userAgent;

            await _sessaoAcessoRepository.AlterarAsync(sessaoAcesso);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task AtualizarLogoutAcesso(int usuarioId, string chave)
        {
            var sessaoAcesso = await _sessaoAcessoRepository.ObterSessaoAcessoUsuarioSessaoAsync(usuarioId, chave);

            if (sessaoAcesso == null)
                return;

            sessaoAcesso.DataHoraAtualizacao = DateTime.Now;
            sessaoAcesso.Logout = true;

            await _sessaoAcessoRepository.AlterarAsync(sessaoAcesso);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> DeslogarAsync(int sessaoAcessoId)
        {
            var sessaoAcesso = await _sessaoAcessoRepository.ObterSessaoAcessoAsync(sessaoAcessoId);

            if (sessaoAcesso == null || sessaoAcesso.Deslogado)
                return false;

            sessaoAcesso.Logout = true;

            await _sessaoAcessoRepository.AlterarAsync(sessaoAcesso);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}