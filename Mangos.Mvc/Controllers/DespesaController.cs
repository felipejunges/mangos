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
using System.Threading.Tasks;

namespace Mangos.Mvc.Controllers
{
    [Authorize]
    public class DespesaController : BaseController
    {
        private readonly DataKeeperService _dataKeeperService;
        private readonly LancamentoService _lancamentoService;
        private readonly ILancamentoRepository _lancamentoRepository;
        private readonly IContaBancariaRepository _contaBancariaRepository;
        private readonly ICategoriaRepository _categoriaRepository;
        private readonly LancamentoEdicaoModelBinder _edicaoModelBinder;

        public DespesaController(DataKeeperService dataKeeperService, IUserResolverService userResolverService, LancamentoService lancamentoService, ILancamentoRepository lancamentoRepository, IContaBancariaRepository contaBancariaRepository, ICategoriaRepository categoriaRepository, LancamentoEdicaoModelBinder edicaoModelBinder) : base(dataKeeperService, userResolverService)
        {
            _dataKeeperService = dataKeeperService;
            _lancamentoService = lancamentoService;
            _lancamentoRepository = lancamentoRepository;
            _contaBancariaRepository = contaBancariaRepository;
            _categoriaRepository = categoriaRepository;
            _edicaoModelBinder = edicaoModelBinder;
        }

        public async Task<IActionResult> Index()
        {
            string tipoPesquisa = "P";
            var tipoData = LancamentoTipoDataPesquisa.Vencimento;
            DateTime dataInicial = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime dataFinal = dataInicial.AddMonths(1).AddDays(-1);

            ViewData["TipoPesquisa"] = tipoPesquisa;
            ViewData["Mes"] = dataInicial.ToString("MM/yyyy");
            ViewData["DataInicial"] = dataInicial.ToString("dd/MM/yyyy");
            ViewData["DataFinal"] = dataFinal.ToString("dd/MM/yyyy");
            ViewData["TipoData"] = tipoData.ToString();
            ViewData["DataAlertaVencimentos"] = DateTime.Now.Date.AddDays((await _dataKeeperService.GetData(_userResolverService.UsuarioId, _userResolverService.ChaveSessao)).DiasAlertaVencimentos);

            var lancamentos = await _lancamentoRepository.ListarLancamentosAsync(_userResolverService.GrupoId, TipoLancamento.Despesa, string.Empty, tipoPesquisa, string.Empty, dataInicial, dataFinal, tipoData);

            var lancamentosModel = LancamentoMappers.ToListaModel(lancamentos);

            return View(lancamentosModel);
        }

        public async Task<IActionResult> IndexLista(string tipoBusca, string descricao, string tipoPesquisa, string pesquisa, string mes, DateTime? dataInicial, DateTime? dataFinal, LancamentoTipoDataPesquisa tipoData)
        {
            if (tipoBusca == "S")
            {
                dataInicial = mes != null ? Convert.ToDateTime("01/" + mes) : DateTime.Now.Date;
                dataFinal = mes != null ? Convert.ToDateTime("01/" + mes).AddMonths(1).AddDays(-1) : DateTime.Now.Date;
            }

            ViewData["TipoPesquisa"] = tipoPesquisa;
            ViewData["TipoData"] = tipoData;
            ViewData["DataAlertaVencimentos"] = DateTime.Now.Date.AddDays((await _dataKeeperService.GetData(_userResolverService.UsuarioId, _userResolverService.ChaveSessao)).DiasAlertaVencimentos);

            var lancamentos = await _lancamentoRepository.ListarLancamentosAsync(_userResolverService.GrupoId, TipoLancamento.Despesa, descricao, tipoPesquisa, pesquisa, dataInicial!.Value, dataFinal!.Value, tipoData);

            var lancamentosModel = LancamentoMappers.ToListaModel(lancamentos);

            return PartialView(lancamentosModel);
        }

        public async Task<IActionResult> Agrupadas(Guid agrupador)
        {
            ViewData["Agrupador"] = agrupador.ToString();
            ViewData["DataAlertaVencimentos"] = DateTime.Now.Date.AddDays((await _dataKeeperService.GetData(_userResolverService.UsuarioId, _userResolverService.ChaveSessao)).DiasAlertaVencimentos);

            var lancamentos = await _lancamentoRepository.ListarLancamentosPeloAgrupadorAsync(_userResolverService.GrupoId, TipoLancamento.Despesa, agrupador);

            var lancamentosModel = LancamentoMappers.ToListaModel(lancamentos);

            return View(lancamentosModel);
        }

        public async Task<IActionResult> AgrupadasLista(Guid agrupador)
        {
            ViewData["DataAlertaVencimentos"] = DateTime.Now.Date.AddDays((await _dataKeeperService.GetData(_userResolverService.UsuarioId, _userResolverService.ChaveSessao)).DiasAlertaVencimentos);

            var lancamentos = await _lancamentoRepository.ListarLancamentosPeloAgrupadorAsync(_userResolverService.GrupoId, TipoLancamento.Despesa, agrupador);

            var lancamentosModel = LancamentoMappers.ToListaModel(lancamentos);

            return PartialView(lancamentosModel);
        }

        public async Task<IActionResult> Incluir()
        {
            var lancamentoInclusaoModel = new LancamentoInclusaoModel(
                grupoId: _userResolverService.GrupoId,
                tipoParcelamento: TipoParcelamentoLancamento.Parcelar,
                categorias: CategoriaMappers.ToListaModel(await _categoriaRepository.ListarCategoriasAsync(_userResolverService.GrupoId, string.Empty, TipoCategoriaPesquisa.Despesa, false)),
                contasBancarias: ContaBancariaMappers.ToListaModel(await _contaBancariaRepository.ListaContasBancariasAsync(_userResolverService.GrupoId, string.Empty, false))
            );

            return View(lancamentoInclusaoModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Incluir(LancamentoInclusaoModel lancamentoInclusaoModel)
        {
            if (ModelState.IsValid)
            {
                if (lancamentoInclusaoModel.GrupoId != _userResolverService.GrupoId)
                    return BadRequest();

                var command = LancamentoMappers.ToIncluirCommand(lancamentoInclusaoModel, TipoLancamento.Despesa);

                await _lancamentoService.IncluirCompletaAsync(command);

                _dataKeeperService.Invalidar(_userResolverService.ChaveSessao);

                return OkResult();
            }

            //
            lancamentoInclusaoModel.AtualizarCategoriasCartoesCredito(
                categorias: CategoriaMappers.ToListaModel(await _categoriaRepository.ListarCategoriasAsync(_userResolverService.GrupoId, string.Empty, TipoCategoriaPesquisa.Despesa, false, lancamentoInclusaoModel.CategoriaId)),
                contasBancarias: ContaBancariaMappers.ToListaModel(await _contaBancariaRepository.ListaContasBancariasAsync(_userResolverService.GrupoId, string.Empty, false, lancamentoInclusaoModel.ContaBancariaId))
            );

            return View(lancamentoInclusaoModel);
        }

        public async Task<IActionResult> IncluirPaga()
        {
            var lancamentoInclusaoModel = new LancamentoPagoInclusaoModel(
                _userResolverService.GrupoId,
                categorias: CategoriaMappers.ToListaModel(await _categoriaRepository.ListarCategoriasAsync(_userResolverService.GrupoId, string.Empty, TipoCategoriaPesquisa.Despesa, false)),
                contasBancarias: ContaBancariaMappers.ToListaModel(await _contaBancariaRepository.ListaContasBancariasAsync(_userResolverService.GrupoId, string.Empty, false))
            );

            return View(lancamentoInclusaoModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IncluirPaga(LancamentoPagoInclusaoModel lancamentoInclusaoModel)
        {
            if (ModelState.IsValid)
            {
                if (lancamentoInclusaoModel.GrupoId != _userResolverService.GrupoId)
                    return BadRequest();

                var command = LancamentoMappers.ToIncluirPagoCommand(lancamentoInclusaoModel, TipoLancamento.Despesa);

                await _lancamentoService.IncluirPagaAsync(command);

                _dataKeeperService.Invalidar(_userResolverService.ChaveSessao);

                return OkResult();
            }

            //
            lancamentoInclusaoModel.AtualizarCategoriasCartoesCredito(
                categorias: CategoriaMappers.ToListaModel(await _categoriaRepository.ListarCategoriasAsync(_userResolverService.GrupoId, string.Empty, TipoCategoriaPesquisa.Despesa, false, lancamentoInclusaoModel.CategoriaId)),
                contasBancarias: ContaBancariaMappers.ToListaModel(await _contaBancariaRepository.ListaContasBancariasAsync(_userResolverService.GrupoId, string.Empty, false, lancamentoInclusaoModel.ContaBancariaId))
            );

            return View(lancamentoInclusaoModel);
        }

        public async Task<IActionResult> Editar(int id)
        {
            var lancamentoEdicaoModel = await _edicaoModelBinder.CriarModelPeloIdAsync(id);

            if (lancamentoEdicaoModel == null)
                return NotFound();

            return View(lancamentoEdicaoModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(LancamentoEdicaoModel lancamentoEdicaoModel)
        {
            if (ModelState.IsValid)
            {
                var lancamento = await _edicaoModelBinder.CriarEntidadeAsync(lancamentoEdicaoModel);

                if (lancamento is null)
                    return BadRequest();

                await _lancamentoService.AlterarAsync(lancamento);

                _dataKeeperService.Invalidar(_userResolverService.ChaveSessao);

                return OkResult();
            }

            //
            await _edicaoModelBinder.AtualizarAsync(lancamentoEdicaoModel);

            return View(lancamentoEdicaoModel);
        }

        public async Task<IActionResult> Pagar(int id)
        {
            ViewData["ContasBancariasLista"] = new SelectList(await _contaBancariaRepository.ListaContasBancariasAsync(_userResolverService.GrupoId, string.Empty, false), "Id", "Descricao", null);

            var lancamentoPagamentoModel = new LancamentoPagamentoModel() { Id = id, GrupoId = _userResolverService.GrupoId };
            lancamentoPagamentoModel.SetValidationHash();

            return View(lancamentoPagamentoModel);
        }

        [HttpPost]
        public async Task<IActionResult> Pagar(LancamentoPagamentoModel lancamentoPagamentoModel)
        {
            if (ModelState.IsValid)
            {
                if (lancamentoPagamentoModel.GrupoId != _userResolverService.GrupoId)
                    return BadRequest();

                if (!lancamentoPagamentoModel.CheckValidationHash())
                    return BadRequest();

                await _lancamentoService.PagarAsync(
                    lancamentoPagamentoModel.Id,
                    lancamentoPagamentoModel.ContaBancariaId,
                    lancamentoPagamentoModel.DataPagamento!.Value);

                _dataKeeperService.Invalidar(_userResolverService.ChaveSessao);

                return OkResult();
            }

            //
            ViewData["ContasBancariasLista"] = new SelectList(await _contaBancariaRepository.ListaContasBancariasAsync(_userResolverService.GrupoId, string.Empty, false, lancamentoPagamentoModel.ContaBancariaId), "Id", "Descricao", null);

            return View(lancamentoPagamentoModel);
        }

        public async Task<IActionResult> Excluir(int id)
        {
            var lancamento = await _lancamentoRepository.ObterLancamentoAsync(id);

            if (lancamento == null)
                return NotFound();

            if (lancamento.GrupoId != _userResolverService.GrupoId)
                return BadRequest();

            try
            {
                await _lancamentoService.RemoverAsync(lancamento);

                _dataKeeperService.Invalidar(_userResolverService.ChaveSessao);

                return SucessoResult("Despesa excluída com sucesso.");
            }
            catch
            {
                return ErroResult("Erro ao excluir despesa.");
            }
        }
    }
}