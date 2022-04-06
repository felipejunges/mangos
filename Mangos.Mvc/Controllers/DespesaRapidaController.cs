using Mangos.Dominio.Commands.DespesasRapidas;
using Mangos.Dominio.Interfaces.Repository;
using Mangos.Dominio.Services;
using Mangos.Dominio.Services.User;
using Mangos.Mvc.Models.Mappers;
using Mangos.Mvc.Models.ViewModels;
using Mangos.Mvc.Services;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Mangos.Mvc.Controllers
{
    [Authorize]
    public class DespesaRapidaController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly LancamentoService _lancamentoService;
        private readonly ContaBancariaService _contaBancariaService;
        private readonly ICartaoCreditoRepository _cartaoCreditoRepository;
        private readonly IContaBancariaRepository _contaBancariaRepository;

        public DespesaRapidaController(DataKeeperService dataKeeperService, IUserResolverService userResolverService, IMediator mediator, LancamentoService lancamentoService, ContaBancariaService contaBancariaService, ICartaoCreditoRepository cartaoCreditoRepository, IContaBancariaRepository contaBancariaRepository) : base(dataKeeperService, userResolverService)
        {
            _mediator = mediator;
            _lancamentoService = lancamentoService;
            _contaBancariaService = contaBancariaService;
            _cartaoCreditoRepository = cartaoCreditoRepository;
            _contaBancariaRepository = contaBancariaRepository;
        }

        public async Task<IActionResult> Incluir()
        {
            var cartoesCredito = await _cartaoCreditoRepository.ListarCartoesCreditoAsync(_userResolverService.GrupoId, string.Empty, false);
            var contasBancarias = await _contaBancariaRepository.ListaContasBancariasAsync(_userResolverService.GrupoId, string.Empty, false);

            return View(new DespesaRapidaViewModel()
            {
                Valor = 0,
                ContasBancarias = ContaBancariaMappers.ToListaModel(contasBancarias),
                CartoesCredito = CartaoCreditoMappers.ToListaModel(cartoesCredito)
            });
        }

        [IgnoreAntiforgeryToken]
        [HttpPost]
        public async Task<IActionResult> Incluir(DespesaRapidaViewModel despesaRapidaModel)
        {
            if (ModelState.IsValid)
            {
                var request = MapCommand(despesaRapidaModel);

                // TODO: como validar o Command antes?
                var validation = new IncluirDespesaRapidaCommandValidator().Validate(request);

                if (validation.IsValid)
                {
                    await _mediator.Send(request);

                    string nomeController = despesaRapidaModel.CartaoCreditoId == null ? nameof(DespesaController) : nameof(LancamentoCartaoController);
                    return RedirectToAction("Index", nomeController[0..^10]);
                }
                else
                {
                    // TODO: alimentar o ModelState com os erros?

                    ModelState.AddModelError("", "Erro no Validation");
                }
            }

            //
            var cartoesCredito = await _cartaoCreditoRepository.ListarCartoesCreditoAsync(_userResolverService.GrupoId, string.Empty, false);
            var contasBancarias = await _contaBancariaRepository.ListaContasBancariasAsync(_userResolverService.GrupoId, string.Empty, false);

            despesaRapidaModel.ContasBancarias = ContaBancariaMappers.ToListaModel(contasBancarias);
            despesaRapidaModel.CartoesCredito = CartaoCreditoMappers.ToListaModel(cartoesCredito);

            return View(despesaRapidaModel);
        }

        // TODO: AutoMapper (ou não!)
        private IncluirDespesaRapidaCommand MapCommand(DespesaRapidaViewModel despesaRapidaModel) =>
            new IncluirDespesaRapidaCommand(
                despesaRapidaModel.PessoaId,
                despesaRapidaModel.Descricao ?? string.Empty, // TODO: verificar se não é melhor permitir o Command com o campo String nullable
                despesaRapidaModel.Valor,
                despesaRapidaModel.ContaBancariaId,
                despesaRapidaModel.CartaoCreditoId,
                despesaRapidaModel.AtualizarCoordenadas,
                despesaRapidaModel.PessoaCoordenadaIdAtualizar,
                despesaRapidaModel.Latitude,
                despesaRapidaModel.Longitude);
    }
}