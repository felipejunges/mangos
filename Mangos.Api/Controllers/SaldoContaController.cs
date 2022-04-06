using Mangos.Dominio.Entities;
using Mangos.Dominio.Models;
using Mangos.Dominio.Interfaces.Repository;
using Mangos.Dominio.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Mangos.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SaldoContaController : ControllerBase
    {
        private readonly IUserResolverService _userResolverService;
        private readonly ISaldoContaBancariaRepository _saldoContaBancariaRepository;

        public SaldoContaController(IUserResolverService userResolverService, ISaldoContaBancariaRepository saldoContaBancariaRepository)
        {
            _userResolverService = userResolverService;
            _saldoContaBancariaRepository = saldoContaBancariaRepository;
        }

        /// <summary>
        /// Obtém os saldos das contas do grupo do usuário logado.
        /// </summary>
        /// <response code="200">Sucesso na obtenção da lista de saldos.</response>
        /// <response code="401">Usuário não autorizado.</response>
        [ProducesResponseType(typeof(IEnumerable<SaldoContaBancaria>), (int)HttpStatusCode.OK)]
        [HttpGet]
        [Authorize]
        public async Task<IEnumerable<SaldoContaBancaria>> Get()
        {
            DateTime dataInicial = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime dataFinal = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            var saldosContasBancarias = await _saldoContaBancariaRepository.ListarSaldosContasBancariasAsync(_userResolverService.GrupoId, null, dataInicial, dataFinal);

            return saldosContasBancarias;
        }
    }
}