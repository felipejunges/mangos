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
    public class ContaBancariaController : Controller
    {
        private readonly IUserResolverService _userResolverService;
        private readonly IContaBancariaRepository _contaBancariaRepository;

        public ContaBancariaController(IUserResolverService userResolverService, IContaBancariaRepository contaBancariaRepository)
        {
            _userResolverService = userResolverService;
            _contaBancariaRepository = contaBancariaRepository;
        }

        /// <summary>
        /// Obtém as contas bancárias do grupo do usuário logado.
        /// </summary>
        /// <response code="200">Sucesso na obtenção da lista de contas bancárias.</response>
        /// <response code="401">Usuário não autorizado.</response>
        [ProducesResponseType(typeof(IEnumerable<ContaBancaria>), (int)HttpStatusCode.OK)]
        [HttpGet]
        [Authorize]
        public async Task<IEnumerable<ContaBancaria>> Get()
        {
            var contasBancarias = await _contaBancariaRepository.ListaContasBancariasAsync(_userResolverService.GrupoId, null, false);

            return contasBancarias;
        }
    }
}