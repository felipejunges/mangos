using System;

namespace Mangos.Api.Models
{
    public class JwtToken
    {
        public string Token { get; private set; }

        public string RefreshToken { get; private set; }

        public DateTime Create { get; private set; }

        public DateTime Expiration { get; private set; }

        public DateTime RefreshExpiration { get; private set; }

        public JwtToken(string token, string refreshToken, DateTime create, DateTime expiration, DateTime refreshExpiration)
        {
            Token = token;
            RefreshToken = refreshToken;
            Create = create;
            Expiration = expiration;
            RefreshExpiration = refreshExpiration;
        }
    }
}