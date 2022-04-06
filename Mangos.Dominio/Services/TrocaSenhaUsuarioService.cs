using Mangos.Dominio.Entities;
using Mangos.Dominio.Interfaces;
using Mangos.Dominio.Interfaces.Repository;
using Mangos.Dominio.Services.Interface;
using Mangos.Dominio.Utils;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Mangos.Dominio.Services
{
    public class TrocaSenhaUsuarioService
    {
        private readonly IEmailService _emailService;
        private readonly ILogger<TrocaSenhaUsuarioService> _logger;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IUnitOfWork _unitOfWork;

        public TrocaSenhaUsuarioService(IEmailService emailService, ILogger<TrocaSenhaUsuarioService> logger, IUsuarioRepository usuarioRepository, IUnitOfWork unitOfWork)
        {
            _emailService = emailService;
            _logger = logger;
            _usuarioRepository = usuarioRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> EnviaEmailTrocaSenha(Usuario usuario)
        {
            string textoEmail = $"Olá!<br /><br />Você usou a ferramenta \"Esqueci minha senha\" do Mangos - Sistema Financeiro.<br /><br /><b>Caso não tenha sido você, por favor, desconsidere este e-mail.</b><br /><br />Caso tenha sido você, por favor, acesse o seguinte link para criar uma nova senha:<br />https://mangos.inf.br/Login/TrocarSenha/{usuario.TokenSenha}";

            try
            {
                var emailRetorno = MailMessageFactory.Create(usuario.Email, usuario.Nome, "Mangos | Esqueci minha senha", textoEmail, null);

                await _emailService.EnviarAsync(emailRetorno);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao enviar e-mail do token para troca da senha do usuário {NomeUsuario} ({EmailUsuario}).", usuario.Nome, usuario.Email);

                return false;
            }
        }

        public async Task<NotificationResult> SetarTokenRandomicoAsync(string? email)
        {
            var result = new NotificationResult();

            Usuario? usuario = null;

            if (email is not null)
                usuario = await _usuarioRepository.ObterUsuarioAtivoPeloEmailAsync(email);

            if (usuario == null)
            {
                result.AddNotification("O e-mail informado é inválido.");
                return result;
            }

            if (result.IsValid)
            {
                string token = Cripto.SHA1Encrypt(StringUtils.GeraStringRandomica(8).ToLower() + Guid.NewGuid() + usuario.Email).Substring(0, 12).ToLower();

                usuario.SetarToken(token);

                await _usuarioRepository.AlterarAsync(usuario);
                await _unitOfWork.SaveChangesAsync();

                if (!await EnviaEmailTrocaSenha(usuario))
                {
                    result.AddNotification("Ocorreu um erro ao tentar enviar o e-mail com a nova senha.");
                }
            }

            return result;
        }
    }
}