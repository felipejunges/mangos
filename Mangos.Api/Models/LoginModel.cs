using System.ComponentModel.DataAnnotations;

namespace Mangos.Api.Models
{
    public class LoginModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Senha { get; set; }

        public LoginModel()
        {
            Email = string.Empty;
            Senha = string.Empty;
        }
    }
}