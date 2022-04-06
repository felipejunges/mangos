using Mangos.Api.Configuration;
using Mangos.Api.Models;
using Mangos.Dominio.Services;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Principal;

namespace Mangos.Api.Services
{
    public class JwtService
    {
        private readonly SigningConfigurations _signingConfigurations;
        private readonly TokenConfigurations _tokenConfigurations;

        public JwtService(SigningConfigurations signingConfigurations, TokenConfigurations tokenConfigurations, DispositivoConectadoService dispositivoConectadoService)
        {
            _signingConfigurations = signingConfigurations;
            _tokenConfigurations = tokenConfigurations;
        }

        public JwtToken GenerateJwtToken(string email, string nome, string identificador, int usuarioId, int grupoId, DateTime refreshExpiration)
        {
            ClaimsIdentity identity = new ClaimsIdentity(
                    new GenericIdentity(usuarioId.ToString()),
                    new[] {
                        new Claim(ClaimTypes.NameIdentifier, identificador),
                        new Claim(ClaimTypes.Email, email),
                        new Claim(ClaimTypes.GivenName, nome),
                        new Claim(MangosApiClaimTypes.GrupoIdLogado, grupoId.ToString()),
                        new Claim(ClaimTypes.Role, "Admin")
                    }
                );

            var dataCriacao = DateTime.Now;
            var dataExpiracao = dataCriacao + TimeSpan.FromHours(_tokenConfigurations.TempoToken);

            var handler = new JwtSecurityTokenHandler();
            var securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _tokenConfigurations.Issuer,
                Audience = _tokenConfigurations.Audience,
                SigningCredentials = _signingConfigurations.SigningCredentials,
                Subject = identity,
                NotBefore = dataCriacao,
                Expires = dataExpiracao
            });
            var token = handler.WriteToken(securityToken);

            var refreshToken = GenerateRefreshToken();

            return new JwtToken(token, refreshToken, dataCriacao, dataExpiracao, refreshExpiration);
        }

        private static string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters()
            {
                ValidateAudience = false, //you might want to validate the audience and issuer depending on your use case
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _signingConfigurations.Key,
                ValidateLifetime = false //here we are saying that we don't care about the token's expiration date
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;

            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }
    }
}