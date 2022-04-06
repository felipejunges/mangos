using Mangos.Dominio.Entities;
using Mangos.Dominio.Interfaces.Repository;
using Mangos.Dominio.Services;
using Mangos.Dominio.Services.User;
using Mangos.Mvc.Models.Mappers;
using Mangos.Mvc.Models.ViewModels;
using Mangos.Mvc.Models.ViewModels.ModelBuilders;
using Mangos.Mvc.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Mangos.Mvc.Controllers
{
    [Authorize]
    public class CartaoCreditoController : BaseController
    {
        private readonly CartaoCreditoService _cartaoCreditoService;
        private readonly ICartaoCreditoRepository _cartaoCreditoRepository;
        private readonly ILancamentoCartaoRepository _lancamentoCartaoRepository;
        private readonly CartaoCreditoDadosModelBuilder _modelBuilder;

        public CartaoCreditoController(DataKeeperService dataKeeperService, IUserResolverService userResolverService, CartaoCreditoService cartaoCreditoService, ICartaoCreditoRepository cartaoCreditoRepository, ILancamentoCartaoRepository lancamentoCartaoRepository, CartaoCreditoDadosModelBuilder modelBuilder) : base(dataKeeperService, userResolverService)
        {
            _cartaoCreditoService = cartaoCreditoService;
            _cartaoCreditoRepository = cartaoCreditoRepository;
            _lancamentoCartaoRepository = lancamentoCartaoRepository;
            _modelBuilder = modelBuilder;
        }

        public async Task<IActionResult> Index()
        {
            var cartoesCredito = await _cartaoCreditoRepository.ListarCartoesCreditoAsync(_userResolverService.GrupoId, string.Empty, false);

            var cartoesCreditoModel = CartaoCreditoMappers.ToListaModel(cartoesCredito);

            return View(cartoesCreditoModel);
        }

        public async Task<IActionResult> IndexLista(string? descricao = null, bool buscarInativos = false)
        {
            var cartoesCredito = await _cartaoCreditoRepository.ListarCartoesCreditoAsync(_userResolverService.GrupoId, descricao, buscarInativos);

            var cartoesCreditoModel = CartaoCreditoMappers.ToListaModel(cartoesCredito);

            return PartialView(cartoesCreditoModel);
        }

        public async Task<IActionResult> GetDadosParaFechamento(int id)
        {
            var cartaoCredito = await _cartaoCreditoRepository.ObterCartaoCreditoAsync(id);

            if (cartaoCredito != null && cartaoCredito.GrupoId == _userResolverService.GrupoId)
            {
                var dataProximoLancamentoFechar = await _lancamentoCartaoRepository.ObterDataPrimeiroLancamentoAbertoAsync(id);

                return Json(new
                {
                    categoriaId = cartaoCredito.CategoriaId.ToString(),
                    dataFechamento = dataProximoLancamentoFechar != null ? dataProximoLancamentoFechar.Value.ToString("MM/yyyy") : DateTime.Now.ToString("MM/yyyy"),
                    gerarLancamento = cartaoCredito.GerarLancamentoFecharMes
                });
            }

            return NotFound();
        }

        public async Task<IActionResult> GetDadosParaReabertura(int id)
        {
            var cartaoCredito = await _cartaoCreditoRepository.ObterCartaoCreditoAsync(id);

            if (cartaoCredito != null && cartaoCredito.GrupoId == _userResolverService.GrupoId)
            {
                var dataProximoLancamentoFechar = await _lancamentoCartaoRepository.ObterDataUltimoLancamentoGeradoAsync(id);

                return Json(new
                {
                    dataFechamento = dataProximoLancamentoFechar != null ? dataProximoLancamentoFechar.Value.ToString("MM/yyyy") : DateTime.Now.ToString("MM/yyyy")
                });
            }

            return NotFound();
        }

        public async Task<IActionResult> Dados(int? id)
        {
            CartaoCreditoDadosModel? cartaoCreditoDadosModel = await _modelBuilder.CriarModelPeloIdAsync(id);

            if (cartaoCreditoDadosModel == null)
                return NotFound();

            return View(cartaoCreditoDadosModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Dados(CartaoCreditoDadosModel cartaoCreditoDadosModel)
        {
            if (ModelState.IsValid)
            {
                CartaoCredito? cartaoCredito = await _modelBuilder.CriarEntidadeAsync(cartaoCreditoDadosModel);

                if (cartaoCredito is null)
                    return BadRequest();

                await _cartaoCreditoService.PersistirAsync(cartaoCredito);

                return OkResult();
            }

            //
            await _modelBuilder.AtualizarAsync(cartaoCreditoDadosModel);

            return View(cartaoCreditoDadosModel);
        }

        public async Task<IActionResult> Excluir(int id)
        {
            var cartaoCredito = await _cartaoCreditoRepository.ObterCartaoCreditoAsync(id);

            if (cartaoCredito == null)
                return NotFound();

            if (cartaoCredito.GrupoId != _userResolverService.GrupoId)
                return BadRequest();

            try
            {
                await _cartaoCreditoService.RemoverAsync(cartaoCredito);

                return SucessoResult("Cartão de crédito excluído com sucesso.");
            }
            catch
            {
                return ErroResult("Erro ao excluir cartão de crédito.");
            }
        }
    }
}