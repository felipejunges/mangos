using Mangos.Dominio.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Mangos.Mvc.Models.GerenciarConta
{
    public class GerenciarAlertasVencimentosModel
    {
        public TipoAvisoVencimentosUsuario TipoAlertaVencimentos { get; set; }

        [Display(Name = "Dias alerta vencimentos")]
        public int DiasAlertaVencimentos { get; set; }

        public TipoAvisoVencimentosUsuario TipoEmailVencimentos { get; set; }

        [Display(Name = "Dias e-mail vencimentos")]
        public int DiasEmailVencimentos { get; set; }

        public Dictionary<string, string> TiposAvisoVencimentosUsuario { get; set; }

        public Dictionary<string, string> TiposEmailVencimentosUsuario { get; set; }

        public bool Sucesso { get; set; }

        public GerenciarAlertasVencimentosModel()
        {
            TiposAvisoVencimentosUsuario = new Dictionary<string, string>();
            TiposEmailVencimentosUsuario = new Dictionary<string, string>();
        }

        public GerenciarAlertasVencimentosModel(TipoAvisoVencimentosUsuario tipoAlertaVencimentos, int diasAlertaVencimentos, TipoAvisoVencimentosUsuario tipoEmailVencimentos, int diasEmailVencimentos)
        {
            TipoAlertaVencimentos = tipoAlertaVencimentos;
            DiasAlertaVencimentos = diasAlertaVencimentos;
            TipoEmailVencimentos = tipoEmailVencimentos;
            DiasEmailVencimentos = diasEmailVencimentos;
            TiposAvisoVencimentosUsuario = new Dictionary<string, string>();
            TiposEmailVencimentosUsuario = new Dictionary<string, string>();
            Sucesso = false;
        }

        public void AtualizarTipos(Dictionary<string, string> tiposAvisoVencimentosUsuario, Dictionary<string, string> tiposEmailVencimentosUsuario)
        {
            TiposAvisoVencimentosUsuario = tiposAvisoVencimentosUsuario;
            TiposEmailVencimentosUsuario = tiposEmailVencimentosUsuario;
        }
    }
}