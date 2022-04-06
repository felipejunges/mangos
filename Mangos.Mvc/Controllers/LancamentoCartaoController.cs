using Mangos.Dominio.Constants;
using Mangos.Dominio.Enums;
using Mangos.Dominio.Interfaces.Repository;
using Mangos.Dominio.Services;
using Mangos.Dominio.Services.User;
using Mangos.Mvc.Models.Mappers;
using Mangos.Mvc.Models.ViewModels;
using Mangos.Mvc.Models.ViewModels.ModelBuilders;
using Mangos.Mvc.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mangos.Mvc.Controllers
{
    [Authorize]
    public class LancamentoCartaoController : BaseController
    {
        private readonly DataKeeperService _dataKeeperService;
        private readonly ILancamentoCartaoRepository _lancamentoCartaoRepository;
        private readonly LancamentoCartaoService _lancamentoCartaoService;
        private readonly ICartaoCreditoRepository _cartaoCreditoRepository;
        private readonly ICategoriaRepository _categoriaRepository;
        private readonly LancamentoCartaoEdicaoModelBuilder _edicaoModelBuilder;

        public LancamentoCartaoController(DataKeeperService dataKeeperService, ILancamentoCartaoRepository lancamentoCartaoRepository, IUserResolverService userResolverService, LancamentoCartaoService lancamentoCartaoService, ICartaoCreditoRepository cartaoCreditoRepository, ICategoriaRepository categoriaRepository, LancamentoCartaoEdicaoModelBuilder edicaoModelBuilder) : base(dataKeeperService, userResolverService)
        {
            _dataKeeperService = dataKeeperService;
            _lancamentoCartaoRepository = lancamentoCartaoRepository;
            _lancamentoCartaoService = lancamentoCartaoService;
            _cartaoCreditoRepository = cartaoCreditoRepository;
            _categoriaRepository = categoriaRepository;
            _edicaoModelBuilder = edicaoModelBuilder;
        }

        public async Task<IActionResult> Index()
        {
            DateTime mesInicial = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            var lancamentosCartao = await _lancamentoCartaoRepository.ListarLancamentosCartaoAsync(_userResolverService.GrupoId, null, null, null, mesInicial, mesInicial);

            var model = new LancamentoCartaoIndexModel(
                mesInicial,
                CartaoCreditoMappers.ToListaModel(await _cartaoCreditoRepository.ListarCartoesCreditoAsync(_userResolverService.GrupoId, string.Empty, false)),
                LancamentoCartaoMapper.ToListaModel(lancamentosCartao)
            );

            return View(model);
        }

        public async Task<IActionResult> IndexLista(string tipoBusca, int? cartaoCreditoId = null, string? descricao = null, string? pessoa = null, string? mesReferencia = null, string? mesReferenciaInicial = null, string? mesReferenciaFinal = null)
        {
            DateTime? mesReferenciaInicialDate;
            DateTime? mesReferenciaFinalDate;

            if (tipoBusca == "S")
            {
                mesReferenciaInicialDate = mesReferencia != null ? (DateTime?)Convert.ToDateTime("01/" + mesReferencia) : null;
                mesReferenciaFinalDate = mesReferencia != null ? (DateTime?)(Convert.ToDateTime("01/" + mesReferencia).AddMonths(1).AddDays(-1)) : null;
            }
            else
            {
                mesReferenciaInicialDate = mesReferenciaInicial != null ? (DateTime?)Convert.ToDateTime("01/" + mesReferenciaInicial) : null;
                mesReferenciaFinalDate = mesReferenciaFinal != null ? (DateTime?)Convert.ToDateTime("01/" + mesReferenciaFinal) : null;
            }

            var lancamentosCartao = await _lancamentoCartaoRepository.ListarLancamentosCartaoAsync(_userResolverService.GrupoId, cartaoCreditoId, descricao, pessoa, mesReferenciaInicialDate, mesReferenciaFinalDate);

            var lancamentosCartaoModel = LancamentoCartaoMapper.ToListaModel(lancamentosCartao);

            return PartialView(lancamentosCartaoModel);
        }

        public async Task<IActionResult> Agrupados(Guid agrupador)
        {
            ViewData["Agrupador"] = agrupador.ToString();

            var lancamentosCartao = await _lancamentoCartaoRepository.ListarLancamentosCartaoAgrupadosAsync(_userResolverService.GrupoId, agrupador);

            var lancamentosCartaoModel = LancamentoCartaoMapper.ToListaModel(lancamentosCartao);

            return View(lancamentosCartaoModel);
        }

        public async Task<IActionResult> AgrupadosLista(Guid agrupador)
        {
            ViewData["Agrupador"] = agrupador.ToString();

            var lancamentosCartao = await _lancamentoCartaoRepository.ListarLancamentosCartaoAgrupadosAsync(_userResolverService.GrupoId, agrupador);

            var lancamentosCartaoModel = LancamentoCartaoMapper.ToListaModel(lancamentosCartao);

            return PartialView(lancamentosCartaoModel);
        }

        public async Task<IActionResult> Incluir()
        {
            var lancamentoCartaoInclusaoModel = new LancamentoCartaoInclusaoModel(
                grupoId: _userResolverService.GrupoId,
                tipoLancamento: TipoLancamentoCartao.Despesa,
                categorias: CategoriaMappers.ToListaModel(await _categoriaRepository.ListarCategoriasAsync(_userResolverService.GrupoId, string.Empty, TipoCategoriaPesquisa.Todos, false)),
                cartoesCredito: CartaoCreditoMappers.ToListaModel(await _cartaoCreditoRepository.ListarCartoesCreditoAsync(_userResolverService.GrupoId, string.Empty, false))
            );

            return View(lancamentoCartaoInclusaoModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Incluir(LancamentoCartaoInclusaoModel lancamentoCartaoInclusaoModel)
        {
            if (ModelState.IsValid)
            {
                if (lancamentoCartaoInclusaoModel.GrupoId != _userResolverService.GrupoId)
                    return BadRequest();

                var command = LancamentoCartaoMapper.ToIncluirCommand(lancamentoCartaoInclusaoModel);

                await _lancamentoCartaoService.IncluirAsync(command);

                _dataKeeperService.Invalidar(_userResolverService.ChaveSessao);

                return OkResult();
            }

            //
            lancamentoCartaoInclusaoModel.AtualizarCategoriasCartoesCredito(
                categorias: CategoriaMappers.ToListaModel(await _categoriaRepository.ListarCategoriasAsync(_userResolverService.GrupoId, string.Empty, TipoCategoriaPesquisa.Todos, false)),
                cartoesCredito: CartaoCreditoMappers.ToListaModel(await _cartaoCreditoRepository.ListarCartoesCreditoAsync(_userResolverService.GrupoId, string.Empty, false))
            );

            return View(lancamentoCartaoInclusaoModel);
        }

        public async Task<IActionResult> Editar(int id)
        {
            var lancamntoCartaoEdicaoModel = await _edicaoModelBuilder.CriarModelPeloIdAsync(id);

            if (lancamntoCartaoEdicaoModel is null)
                return NotFound();

            return View(lancamntoCartaoEdicaoModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(LancamentoCartaoEdicaoModel lancamntoCartaoEdicaoModel)
        {
            if (ModelState.IsValid)
            {
                var lancamentoCartao = await _edicaoModelBuilder.CriarEntidadeAsync(lancamntoCartaoEdicaoModel);

                if (lancamentoCartao is null)
                    return BadRequest();

                await _lancamentoCartaoService.AlterarAsync(lancamentoCartao);

                _dataKeeperService.Invalidar(_userResolverService.ChaveSessao);

                return OkResult();
            }

            //
            await _edicaoModelBuilder.AtualizarAsync(lancamntoCartaoEdicaoModel);

            return View(lancamntoCartaoEdicaoModel);
        }

        public async Task<IActionResult> FecharMes()
        {
            ViewData["CartoesCreditoLista"] = new SelectList(await _cartaoCreditoRepository.ListarCartoesCreditoAsync(_userResolverService.GrupoId, string.Empty, false), "Id", "Descricao", null);
            ViewData["CategoriasLista"] = new SelectList(await ObterCategoriasModel(TipoCategoriaPesquisa.Despesa), "Id", "Descricao", null);

            var fechamentoMesCartaoModel = new FechamentoMesCartaoModel() { GrupoId = _userResolverService.GrupoId };

            return View(fechamentoMesCartaoModel);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> FecharMes(FechamentoMesCartaoModel fechamentoMesCartaoModel)
        {
            if (ModelState.IsValid)
            {
                if (fechamentoMesCartaoModel.GrupoId != _userResolverService.GrupoId)
                    return BadRequest();

                await _lancamentoCartaoService.FecharMesAsync(
                        fechamentoMesCartaoModel.GrupoId,
                        fechamentoMesCartaoModel.CartaoCreditoId!.Value,
                        fechamentoMesCartaoModel.MesReferencia!.Value,
                        fechamentoMesCartaoModel.GerarLancamento,
                        fechamentoMesCartaoModel.CategoriaId);

                _dataKeeperService.Invalidar(_userResolverService.ChaveSessao);

                return OkResult();
            }

            //
            ViewData["CartoesCreditoLista"] = new SelectList(await _cartaoCreditoRepository.ListarCartoesCreditoAsync(_userResolverService.GrupoId, string.Empty, false), "Id", "Descricao");
            ViewData["CategoriasLista"] = new SelectList(await ObterCategoriasModel(TipoCategoriaPesquisa.Despesa), "Id", "Descricao");

            return View(fechamentoMesCartaoModel);
        }

        public async Task<IActionResult> ReabrirMes()
        {
            ViewData["CartoesCreditoLista"] = new SelectList(await _cartaoCreditoRepository.ListarCartoesCreditoAsync(_userResolverService.GrupoId, string.Empty, false), "Id", "Descricao", null);

            var reaberturaMesCartaoModel = new ReaberturaMesCartaoModel() { GrupoId = _userResolverService.GrupoId };

            return View(reaberturaMesCartaoModel);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> ReabrirMes(ReaberturaMesCartaoModel reaberturaMesCartaoModel)
        {
            if (ModelState.IsValid)
            {
                if (reaberturaMesCartaoModel.GrupoId != _userResolverService.GrupoId)
                    return BadRequest();

                await _lancamentoCartaoService.ReabrirMesAsync(
                        reaberturaMesCartaoModel.GrupoId,
                        reaberturaMesCartaoModel.CartaoCreditoId!.Value,
                        reaberturaMesCartaoModel.MesReferencia!.Value);

                _dataKeeperService.Invalidar(_userResolverService.ChaveSessao);

                return OkResult();
            }

            //
            ViewData["CartoesCreditoLista"] = new SelectList(await _cartaoCreditoRepository.ListarCartoesCreditoAsync(_userResolverService.GrupoId, string.Empty, false), "Id", "Descricao");

            return View(reaberturaMesCartaoModel);
        }

        public async Task<IActionResult> Excluir(int id)
        {
            var lancamentoCartao = await _lancamentoCartaoRepository.ObterLancamentoCartaoAsync(id);

            if (lancamentoCartao == null)
                return NotFound();

            if (lancamentoCartao.GrupoId != _userResolverService.GrupoId)
                return BadRequest();

            try
            {
                await _lancamentoCartaoService.RemoverAsync(lancamentoCartao);

                _dataKeeperService.Invalidar(_userResolverService.ChaveSessao);

                return SucessoResult("Lançamento de cartão excluído com sucesso.");
            }
            catch
            {
                return ErroResult("Erro ao excluir lançamento de cartão.");
            }
        }

        public async Task<IEnumerable<CategoriaListaModel>> ObterCategoriasModel(string tipo, int? categoriaId = null)
        {
            // Esse método, quando esses models forem migrados, irá pra dentro do ModelBuilder
            var categorias = await _categoriaRepository.ListarCategoriasAsync(_userResolverService.GrupoId, string.Empty, tipo, false, categoriaId);
            return CategoriaMappers.ToListaModel(categorias);
        }
    }
}