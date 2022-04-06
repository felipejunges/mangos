using System.ComponentModel.DataAnnotations;

namespace Mangos.Mvc.Models.ViewModels
{
    public class SessaoAcessoListaModel
    {
        public int Id { get; set; }

        [Display(Name = "Últ. requisição")]
        public string DataHoraAtualizacao { get; set; }

        [Display(Name = "Status")]
        public string Status { get; set; }

        [Display(Name = "Usuário")]
        public string Usuario { get; set; }

        public bool Persistente { get; set; }

        [Display(Name = "IP")]
        public string? Ip { get; set; }

        public string? Browser { get; set; }

        [Display(Name = "User agent")]
        public string? UserAgent { get; set; }

        [Display(Name = "Mobile")]
        public bool IsMobile { get; set; }

        public bool Deslogado { get; set; }

        public SessaoAcessoListaModel()
        {
            DataHoraAtualizacao = string.Empty;
            Status = string.Empty;
            Usuario = string.Empty;
        }

        public SessaoAcessoListaModel(int id, string dataHoraAtualizacao, string status, string usuario, bool persistente, string? ip, string? browser, string? userAgent, bool isMobile, bool deslogado)
        {
            Id = id;
            DataHoraAtualizacao = dataHoraAtualizacao;
            Status = status;
            Usuario = usuario;
            Persistente = persistente;
            Ip = ip;
            Browser = browser;
            UserAgent = userAgent;
            IsMobile = isMobile;
            Deslogado = deslogado;
        }
    }
}