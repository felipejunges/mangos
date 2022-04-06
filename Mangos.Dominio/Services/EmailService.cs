using Mangos.Dominio.Services.Interface;
using Mangos.Dominio.Settings;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Mangos.Dominio.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(EmailSettings emailSettings)
        {
            _emailSettings = emailSettings;
        }

        public async Task<bool> EnviarAsync(MailMessage mailMessage)
        {
            if (!_emailSettings.Ativo)
                return false;

            SmtpClient smtpClient = new SmtpClient(_emailSettings.ServidorSMTP);
            NetworkCredential credentials = new NetworkCredential(_emailSettings.UsuarioSMTP, _emailSettings.SenhaSMTP);
            smtpClient.Credentials = credentials;

            smtpClient.Timeout = _emailSettings.Timeout;
            smtpClient.Port = _emailSettings.PortaSMTP;
            smtpClient.EnableSsl = _emailSettings.EnableSSL;

            try
            {
                await smtpClient.SendMailAsync(mailMessage);

                return true;
            }
            catch
            {
                throw;
            }
        }
    }
}