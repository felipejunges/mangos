using Mangos.Dominio.Entities;
using Mangos.Dominio.Interfaces.Repository;
using Mangos.Dominio.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Mangos.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartaoCreditoController : Controller
    {
        private readonly IUserResolverService _userResolverService;
        private readonly ICartaoCreditoRepository _cartaoCreditoRepository;

        public CartaoCreditoController(IUserResolverService userResolverService, ICartaoCreditoRepository cartaoCreditoRepository)
        {
            _userResolverService = userResolverService;
            _cartaoCreditoRepository = cartaoCreditoRepository;
        }

        /// <summary>
        /// Obtém os cartões de crédito do grupo do usuário logado.
        /// </summary>
        /// <response code="200">Sucesso na obtenção da lista de cartões de crédito.</response>
        /// <response code="401">Usuário não autorizado.</response>
        [ProducesResponseType(typeof(IEnumerable<CartaoCredito>), (int)HttpStatusCode.OK)]
        [HttpGet]
        [Authorize]
        public async Task<IEnumerable<CartaoCredito>> Get()
        {
            var cartoesCredito = await _cartaoCreditoRepository.ListarCartoesCreditoAsync(_userResolverService.GrupoId, null, false);

            return cartoesCredito;
        }
    }
}