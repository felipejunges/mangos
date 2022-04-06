using System.Net.Mail;

namespace Mangos.Dominio.Utils
{
    public class MailMessageFactory
    {
        private const string EMAIL_REMETENTE = "mangos@mangos.inf.br";
        private const string NOME_REMETENTE = "Mangos - Sistema Financeiro";

        public static MailMessage Create(string emailDestinatario, string nomeDestinatario, string assunto, string texto, string[]? anexos)
        {
            var mailMessage = new MailMessage();

            mailMessage.From = new MailAddress(EMAIL_REMETENTE, NOME_REMETENTE);
            mailMessage.ReplyToList.Add(mailMessage.From);
            mailMessage.To.Add(new MailAddress(emailDestinatario, nomeDestinatario));
            mailMessage.Subject = assunto;
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = texto;

            if (anexos != null)
            {
                foreach (string anexo in anexos)
                {
                    Attachment attAnexo = new Attachment(anexo);
                    mailMessage.Attachments.Add(attAnexo);
                }
            }

            return mailMessage;
        }
    }
}