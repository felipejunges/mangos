using System.Net.Mail;
using System.Threading.Tasks;

namespace Mangos.Dominio.Services.Interface
{
    public interface IEmailService
    {
        Task<bool> EnviarAsync(MailMessage mailMessage);
    }
}