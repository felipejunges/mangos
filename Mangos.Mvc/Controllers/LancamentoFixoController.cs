using Mangos.Dominio.Interfaces.Repository;
using Mangos.Dominio.Services;
using Mangos.Dominio.Services.User;
using Mangos.Mvc.Models.Mappers;
using Mangos.Mvc.Models.ViewModels;
using Mangos.Mvc.Models.ViewModels.ModelBuilders;
using Mangos.Mvc.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Mangos.Mvc.Controllers
{
    [Authorize]
    public class LancamentoFixoController : BaseController
    {
        private readonly LancamentoFixoService _lancamentoFixoService;
        private readonly ILancamentoFixoRepository _lancamentoFixoRepository;
        private readonly LancamentoFixoDadosModelBuilder _modelBuilder;

        public LancamentoFixoController(DataKeeperService dataKeeperService, IUserResolverService userResolverService, LancamentoFixoService lancamentoFixoService, ILancamentoFixoRepository lancamentoFixoRepository, LancamentoFixoDadosModelBuilder modelBuilder) : base(dataKeeperService, userResolverService)
        {
            _lancamentoFixoService = lancamentoFixoService;
            _lancamentoFixoRepository = lancamentoFixoRepository;
            _modelBuilder = modelBuilder;
        }

        public async Task<IActionResult> Index()
        {
            var lancamentosFixos = await _lancamentoFixoRepository.ListarLancamentosFixosAsync(_userResolverService.GrupoId, string.Empty, false);

            var lancamentosFixosModel = LancamentoFixoMappers.ToListaModel(lancamentosFixos);

            return View(lancamentosFixosModel);
        }

        public async Task<IActionResult> IndexLista(string? descricao = null, bool buscarInativos = false)
        {
            var lancamentosFixos = await _lancamentoFixoRepository.ListarLancamentosFixosAsync(_userResolverService.GrupoId, descricao, buscarInativos);

            var lancamentosFixosModel = LancamentoFixoMappers.ToListaModel(lancamentosFixos);

            return PartialView(lancamentosFixosModel);
        }

        public async Task<IActionResult> ConsultaLancamentos(int id)
        {
            var lancamentos = await _lancamentoFixoService.ObterLancamentosGeradosPeloLancamentoFixo(_userResolverService.GrupoId, id);

            ViewData["LancamentoFixoId"] = id;

            return PartialView(lancamentos);
        }

        public async Task<IActionResult> ConsultaLancamentosLista(int lancamentoFixoId)
        {
            var lancamentos = await _lancamentoFixoService.ObterLancamentosGeradosPeloLancamentoFixo(_userResolverService.GrupoId, lancamentoFixoId);

            return PartialView(lancamentos);
        }

        public async Task<IActionResult> Relatorio()
        {
            var lancamentosFixos = await _lancamentoFixoRepository.ListarLancamentosFixosRelatorioAsync(_userResolverService.GrupoId);

            var lancamentosFixosModel = LancamentoFixoMappers.ToListaModel(lancamentosFixos);

            return View(lancamentosFixosModel);
        }

        public async Task<IActionResult> Dados(int? id)
        {
            var lancamentoFixoDadosModel = await _modelBuilder.CriarModelPeloIdAsync(id);

            if (lancamentoFixoDadosModel is null)
                return NotFound();

            return View(lancamentoFixoDadosModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Dados(LancamentoFixoDadosModel lancamentoFixoDadosModel)
        {
            if (ModelState.IsValid)
            {
                var lancamentoFixo = await _modelBuilder.CriarEntidadeAsync(lancamentoFixoDadosModel);

                if (lancamentoFixo is null)
                    return BadRequest();

                await _lancamentoFixoService.PersistirAsync(lancamentoFixo);

                return OkResult();
            }

            //
            await _modelBuilder.AtualizarAsync(lancamentoFixoDadosModel);

            return View(lancamentoFixoDadosModel);
        }

        [AllowAnonymous]
        public async Task<IActionResult> GerarTodos()
        {
            int registrosGerados = await _lancamentoFixoService.Gerar(null, null);

            return SucessoResult($"{registrosGerados} registros gerados");
        }

        public async Task<IActionResult> Gerar()
        {
            int registrosGerados = await _lancamentoFixoService.Gerar(_userResolverService.GrupoId, null);

            return SucessoResult($"{registrosGerados} registros gerados");
        }

        public async Task<IActionResult> GerarLancamento(int id)
        {
            int registrosGerados = await _lancamentoFixoService.Gerar(_userResolverService.GrupoId, id);

            return SucessoResult($"{registrosGerados} registros gerados");
        }

        public async Task<IActionResult> Excluir(int id)
        {
            var lancamentoFixo = await _lancamentoFixoRepository.ObterLancamentoFixoAsync(id);

            if (lancamentoFixo == null)
                return NotFound();

            if (lancamentoFixo.GrupoId != _userResolverService.GrupoId)
                return BadRequest();

            try
            {
                await _lancamentoFixoService.RemoverAsync(lancamentoFixo);

                return SucessoResult("Lançamento fixo excluído com sucesso.");
            }
            catch
            {
                return ErroResult("Erro ao excluir lançamento fixo.");
            }
        }
    }
}