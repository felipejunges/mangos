using Mangos.Dominio.Interfaces.Repository;
using Mangos.Dominio.Services;
using Mangos.Dominio.Services.User;
using Mangos.Mvc.Models.ViewModels;
using Mangos.Mvc.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Mangos.Mvc.Controllers
{
    [Authorize]
    public class GeoController : BaseController
    {
        private readonly PessoaCoordenadaService _pessoaCoordenadaService;
        private readonly IPessoaCoordenadaRepository _pessoaCoordenadaRepository;

        public GeoController(DataKeeperService dataKeeperService, IUserResolverService userResolverService, PessoaCoordenadaService pessoaCoordenadaService, IPessoaCoordenadaRepository pessoaCoordenadaRepository) : base(dataKeeperService, userResolverService)
        {
            _pessoaCoordenadaService = pessoaCoordenadaService;
            _pessoaCoordenadaRepository = pessoaCoordenadaRepository;
        }

        public IActionResult Geo()
        {
            var pessoas = Enumerable.Empty<PessoaGeoModel>();

            return View(pessoas);
        }

        public async Task<IActionResult> GeoLista(double latitude, double longitude)
        {
            var coordenadas = await _pessoaCoordenadaRepository.ListarPessoasCoordenadasPorGrupoAsync(_userResolverService.GrupoId);

            var coordenadasModel = coordenadas.Select(c => new PessoaGeoModel(c, latitude, longitude)).ToList();

            return PartialView(coordenadasModel);
        }

        public async Task<IActionResult> BuscaFornecedorMaisProximo(double latitude, double longitude)
        {
            var pessoa = await _pessoaCoordenadaService.ObterFornecedorDespesaRapidaGeoAsync(_userResolverService.GrupoId, latitude, longitude);

            return Json(pessoa);
        }

        public async Task<IActionResult> MapaFornecedores()
        {
            var coordenadas = await _pessoaCoordenadaRepository.ListarPessoasCoordenadasPorGrupoAsync(_userResolverService.GrupoId);

            var coordenadasModel = coordenadas.Select(c => new PessoaGeoModel(c)).ToList();

            return View(coordenadasModel);
        }
    }
}