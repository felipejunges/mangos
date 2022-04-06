using Mangos.Dominio.Constants;
using Mangos.Dominio.Interfaces.Repository;
using Mangos.Dominio.Services;
using Mangos.Dominio.Services.User;
using Mangos.Mvc.Models.ViewModels;
using Mangos.Mvc.Models.ViewModels.ModelBuilders;
using Mangos.Mvc.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Mangos.Mvc.Controllers
{
    [Authorize]
    public class PessoaController : BaseController
    {
        private readonly ILogger<PessoaController> _logger;
        private readonly PessoaDadosModelBuilder _modelBuilder;
        private readonly PessoaService _pessoaService;
        private readonly IPessoaRepository _pessoaRepository;
        private readonly CategoriaService _categoriaService;
        private readonly ICategoriaRepository _categoriaRepository;

        public PessoaController(ILogger<PessoaController> logger, PessoaDadosModelBuilder modelBuilder, DataKeeperService dataKeeperService, IUserResolverService userResolverService, PessoaService pessoaService, IPessoaRepository pessoaRepository, CategoriaService categoriaService, ICategoriaRepository categoriaRepository) : base(dataKeeperService, userResolverService)
        {
            _logger = logger;
            _modelBuilder = modelBuilder;
            _pessoaService = pessoaService;
            _pessoaRepository = pessoaRepository;
            _categoriaService = categoriaService;
            _categoriaRepository = categoriaRepository;
        }

        public async Task<IActionResult> Index()
        {
            var pessoas = await _pessoaRepository.ListarPessoasPaginatedAsync(_userResolverService.GrupoId, null, TipoPessoaPesquisa.Todos, 1, ITENS_POR_PAGINA_PADRAO, false);

            return View(pessoas);
        }

        public async Task<IActionResult> IndexLista(string nome, int pagina, bool buscarInativos)
        {
            var pessoas = await _pessoaRepository.ListarPessoasPaginatedAsync(_userResolverService.GrupoId, nome, TipoPessoaPesquisa.Todos, pagina, ITENS_POR_PAGINA_PADRAO, buscarInativos);

            return PartialView(pessoas);
        }

        public async Task<IActionResult> ConsultaLancamentos(int id)
        {
            var lancamentos = await _pessoaService.ListarPessoaConsultaLancamentosAsync(_userResolverService.GrupoId, id);

            var pessoa = await _pessoaRepository.ObterPessoaAsync(id);

            if (pessoa is null)
                return BadRequest();

            //
            ViewData["PessoaId"] = id.ToString();
            ViewData["Pessoa"] = pessoa.Nome;

            return PartialView(lancamentos);
        }

        public async Task<IActionResult> ConsultaLancamentosLista(int pessoaId)
        {
            var lancamentos = await _pessoaService.ListarPessoaConsultaLancamentosAsync(_userResolverService.GrupoId, pessoaId);

            return PartialView(lancamentos);
        }

        public async Task<IActionResult> BuscaPessoas(string busca, string tipo)
        {
            var pessoas = await _pessoaRepository.ListarPessoasAsync(_userResolverService.GrupoId, busca, tipo, false);

            return Json(new
            {
                suggestions = pessoas.Select(p => new
                {
                    value = p.Nome,
                    data = new
                    {
                        pessoaId = p.Id.ToString(),
                        categoriaId =
                            tipo == TipoPessoaPesquisa.Cliente ? p.CategoriaPadraoReceitaId
                            : tipo == TipoPessoaPesquisa.Fornecedor ? p.CategoriaPadraoDespesaId
                            : null
                    }
                }).ToList()
            });
        }

        public async Task<IActionResult> Dados(int? id)
        {
            var pessoaDadosModel = await _modelBuilder.CriarModelPeloIdAsync(id);

            if (pessoaDadosModel is null)
                return NotFound();

            return View(pessoaDadosModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Dados(PessoaDadosModel pessoaDadosModel)
        {
            if (ModelState.IsValid)
            {
                var pessoa = await _modelBuilder.CriarEntidadeAsync(pessoaDadosModel);

                if (pessoa is null)
                    return BadRequest();

                await _pessoaService.PersistirAsync(pessoa);

                return OkResult();
            }

            //
            await _modelBuilder.AtualizarAsync(pessoaDadosModel);

            return View(pessoaDadosModel);
        }

        public async Task<IActionResult> Excluir(int id)
        {
            var pessoa = await _pessoaRepository.ObterPessoaAsync(id);

            if (pessoa == null)
                return NotFound();

            if (pessoa.GrupoId != _userResolverService.GrupoId)
                return BadRequest();

            try
            {
                await _pessoaService.RemoverAsync(pessoa);

                return SucessoResult("Pessoa excluída com sucesso.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao excluir a pessoa.");

                return ErroResult("Erro ao excluir a pessoa.");
            }
        }
    }
}