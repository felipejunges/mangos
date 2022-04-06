using System.ComponentModel.DataAnnotations;

namespace Mangos.Mvc.Models.ViewModels
{
    public class UsuarioInclusaoModel
    {
        [Display(Name = "E-mail")]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }

        [Display(Name = "Confirme o e-mail")]
        [DataType(DataType.EmailAddress)]
        public string? ConfirmeEmail { get; set; }

        public string? Nome { get; set; }

        [DataType(DataType.Password)]
        public string? Senha { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirme a senha")]
        public string? ConfirmeSenha { get; set; }
    }
}