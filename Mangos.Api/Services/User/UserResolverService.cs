using Mangos.Api.Extensions;
using Mangos.Api.Models;
using Mangos.Dominio.Services.User;
using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;

namespace Mangos.Api.Services.User
{
    public class UserResolverService : IUserResolverService
    {
        private readonly HttpContext _context;

        public bool Logado => _context.User.Identity?.IsAuthenticated ?? false;
        public int UsuarioId => int.Parse(_context.User.Identity?.Name ?? "0");
        public int GrupoId => Convert.ToInt32(_context.User.GetClaimValue(MangosApiClaimTypes.GrupoIdLogado, "0"));
        public string Nome => _context.User.GetClaimValue(ClaimTypes.GivenName, "");
        public string ChaveSessao => _context.User.GetClaimValue(ClaimTypes.NameIdentifier);

        public UserResolverService(IHttpContextAccessor contextAccessor)
        {
            if (contextAccessor.HttpContext is null)
                throw new NullReferenceException("IHttpContextAccessor nulo");

            _context = contextAccessor.HttpContext;
        }
    }
}