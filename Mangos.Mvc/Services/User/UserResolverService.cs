using Mangos.Dominio.Services.User;
using Mangos.Mvc.Extensions;
using Mangos.Mvc.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;

namespace Mangos.Mvc.Services.User
{
    public class UserResolverService : IUserResolverService
    {
        private readonly ClaimsPrincipal _user;

        public bool Logado => _user.Identity?.IsAuthenticated ?? false;
        public int UsuarioId => Convert.ToInt32(_user.GetClaimValue(ClaimTypes.NameIdentifier, "0"));
        public int GrupoId => Convert.ToInt32(_user.GetClaimValue(MangosClaimTypes.GrupoLogado, "0"));
        public string ChaveSessao => _user.GetClaimValue(MangosClaimTypes.ChaveSessao);
        public string Nome => _user.GetClaimValue(ClaimTypes.Name);

        public UserResolverService(IHttpContextAccessor contextAccessor)
        {
            if (contextAccessor.HttpContext is null)
                throw new NullReferenceException("IHttpContextAccessor nulo");

            _user = contextAccessor.HttpContext.User;
        }

        public UserResolverService(ClaimsPrincipal user)
        {
            _user = user;
        }
    }
}