using System.ComponentModel.DataAnnotations;

namespace Mangos.Mvc.Models
{
    public class LoginModel
    {
        [Required]
        [Display(Name = "E-mail")]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        public string? Senha { get; set; }

        [Display(Name = "Lembrar-me")]
        public bool LembrarMe { get; set; }
    }
}