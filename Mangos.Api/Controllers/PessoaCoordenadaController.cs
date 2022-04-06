using Mangos.Dominio.Models;
using Mangos.Dominio.Models.Messages;
using Mangos.Dominio.Services;
using Mangos.Dominio.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace Mangos.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PessoaCoordenadaController : ControllerBase
    {
        private readonly IUserResolverService _userResolverService;
        private readonly PessoaCoordenadaService _pessoaCoordenadaService;

        public PessoaCoordenadaController(PessoaCoordenadaService pessoaCoordenadaService, IUserResolverService userResolverService)
        {
            _userResolverService = userResolverService;
            _pessoaCoordenadaService = pessoaCoordenadaService;
        }

        /// <summary>
        /// Obtém os dados do fornecedor para despesa rápida a partir das coordenadas recebidas.
        /// </summary>
        /// <response code="200">Sucesso na obtenção dos dados da pessoa pelas coordenadas.</response>
        /// <response code="204">Nenhuma pessoa localizada pelas coordenadas recebidas.</response>
        /// <response code="401">Usuário não autorizado.</response>
        [ProducesResponseType(typeof(FornecedorDespesaRapidaGeoModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<FornecedorDespesaRapidaGeoModel>> Get([FromQuery] PessoaCoordenadaRequest request)
        {
            var pessoa = await _pessoaCoordenadaService.ObterFornecedorDespesaRapidaGeoAsync(_userResolverService.GrupoId, request.Latitude, request.Longitude);

            if (pessoa == null)
                return NoContent();

            return pessoa;
        }
    }
}