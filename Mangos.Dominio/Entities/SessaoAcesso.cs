using System;

namespace Mangos.Dominio.Entities
{
    public class SessaoAcesso
    {
        public int Id { get; set; }

        public int UsuarioId { get; set; }

        public string Chave { get; set; }

        public DateTime DataHoraCriacao { get; set; }

        public DateTime DataHoraAtualizacao { get; set; }

        public DateTime DataHoraExpiracao { get; set; }

        public bool Persistente { get; set; }

        public string? Ip { get; set; }

        public string? Browser { get; set; }

        public string? UserAgent { get; set; }

        public bool IsMobile { get; set; }

        public bool Logout { get; set; }

        public bool Deslogado => Logout || DataHoraExpiracao < DateTime.Now;

        public virtual Usuario? Usuario { get; set; }

        public SessaoAcesso(int id, int usuarioId, string chave, DateTime dataHoraCriacao, DateTime dataHoraAtualizacao, DateTime dataHoraExpiracao, bool persistente, string? ip, string? browser, string? userAgent, bool isMobile, bool logout)
        {
            Id = id;
            UsuarioId = usuarioId;
            Chave = chave;
            DataHoraCriacao = dataHoraCriacao;
            DataHoraAtualizacao = dataHoraAtualizacao;
            DataHoraExpiracao = dataHoraExpiracao;
            Persistente = persistente;
            Ip = ip;
            Browser = browser;
            UserAgent = userAgent;
            IsMobile = isMobile;
            Logout = logout;
        }
    }
}