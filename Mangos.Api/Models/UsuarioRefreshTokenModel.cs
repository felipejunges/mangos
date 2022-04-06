namespace Mangos.Api.Models
{
    public class UsuarioRefreshTokenModel
    {
        public string AuthenticationToken { get; set; }

        public string RefreshToken { get; set; }

        public UsuarioRefreshTokenModel()
        {
            AuthenticationToken = string.Empty;
            RefreshToken = string.Empty;
        }
    }
}