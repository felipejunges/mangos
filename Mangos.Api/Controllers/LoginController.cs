using Mangos.Api.Models;
using Mangos.Api.Security;
using Mangos.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace Mangos.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        /// <summary>
        /// Gera um token de autenticação para o usuário informado.
        /// </summary>
        /// <response code="200">Autenticação bem sucedida.</response>
        /// <response code="400">Erro na autenticação.</response>
        [AllowAnonymous]
        [HttpPost]
        [Throttle(Name = "LoginPost", TempoCache = 60)]
        [EnableCors("AllowSpecificOrigins")]
        [ProducesResponseType(typeof(JwtToken), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<JwtToken>> Login([FromBody] LoginModel loginModel, [FromServices] LoginService loginService)
        {
            var jwtToken = await loginService.ObterLoginToken(loginModel);

            if (jwtToken != null)
                return Ok(jwtToken);

            return BadRequest(new ApiError("Falha ao autenticar"));
        }

        /// <summary>
        /// Atualiza o token de autenticação a partir do RefreshToken recebido anteriormente.
        /// </summary>
        /// <response code="200">Atualização bem sucedida, novo token gerado.</response>
        /// <response code="400">Erro na atualização do token.</response>
        [HttpPut]
        [AllowAnonymous]
        [Throttle(Name = "LoginPut", TempoCache = 60)]
        [EnableCors("AllowSpecificOrigins")]
        [ProducesResponseType(typeof(JwtToken), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> RefreshToken([FromBody] UsuarioRefreshTokenModel usuarioModel, [FromServices] LoginService loginService)
        {
            var jwtToken = await loginService.ObterNovoTokenPeloRefresh(usuarioModel);

            if (jwtToken != null)
            {
                return Ok(jwtToken);
            }
            else
            {
                return BadRequest(new ApiError("Token inválido."));
            }
        }
    }
}