using Mangos.Dominio.Interfaces.Repository;
using Mangos.Dominio.Models.Relatorios;
using Mangos.Dominio.Services.Relatorios;
using Mangos.Dominio.Services.User;
using Mangos.Dominio.Types.TypeValidations;
using Mangos.Mvc.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Threading.Tasks;

namespace Mangos.Mvc.Controllers.Relatorios
{
    [Authorize]
    public class RelatorioExtratoContaController : BaseController
    {
        private readonly RelatorioExtratoContaService _relatorioExtratoContaService;
        private readonly IContaBancariaRepository _contaBancariaRepository;

        public RelatorioExtratoContaController(DataKeeperService dataKeeperService, IUserResolverService userResolverService, RelatorioExtratoContaService relatorioExtratoContaService, IContaBancariaRepository contaBancariaRepository) : base(dataKeeperService, userResolverService)
        {
            _relatorioExtratoContaService = relatorioExtratoContaService;
            _contaBancariaRepository = contaBancariaRepository;
        }

        public async Task<IActionResult> Index()
        {
            int? contaBancariaId;
            DateTime dataInicial;
            DateTime dataFinal;
            bool exibirTotais = false;

            RelatorioExtratoContaModel relatorio;

            if (Request.Query.ContainsKey("Conta") && !string.IsNullOrEmpty(Request.Query["Conta"].ToString()) && Request.Query.ContainsKey("Data") && TypeValidations.IsValidYearMonth(Request.Query["Data"].ToString()))
            {
                int ano = Convert.ToInt32(Request.Query["Data"].ToString().Substring(0, 4));
                int mes = Convert.ToInt32(Request.Query["Data"].ToString().Substring(4, 2));

                contaBancariaId = Convert.ToInt32(Request.Query["Conta"].ToString());
                dataInicial = new DateTime(ano, mes, 1);
                dataFinal = dataInicial.AddMonths(1).AddDays(-1);

                ViewData["MensagemEmpty"] = "Nenhum resultado encontrado";
                relatorio = await _relatorioExtratoContaService.ObterRelatorioExtratoContaAsync(_userResolverService.GrupoId, contaBancariaId.Value, dataInicial, dataFinal);
            }
            else
            {
                contaBancariaId = null;
                dataInicial = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                dataFinal = DateTime.Now.Date;

                ViewData["MensagemEmpty"] = "Selecione uma conta para gerar o extrato";
                relatorio = new RelatorioExtratoContaModel();
            }

            ViewData["ContaBancariaId"] = contaBancariaId;
            ViewData["DataInicial"] = dataInicial.ToString("dd/MM/yyyy");
            ViewData["DataFinal"] = dataFinal.ToString("dd/MM/yyyy");
            ViewData["ExibirTotais"] = exibirTotais;

            ViewData["ContasBancariasLista"] = new SelectList(await _contaBancariaRepository.ListaContasBancariasAsync(_userResolverService.GrupoId, string.Empty, false), "Id", "Descricao", contaBancariaId);

            return View(relatorio);
        }

        public async Task<IActionResult> IndexLista(int? contaBancariaId, DateTime dataInicial, DateTime dataFinal, bool exibirTotais = false)
        {
            if (contaBancariaId == null)
            {
                ViewData["MensagemEmpty"] = "Selecione uma conta para gerar o extrato";

                var relatorio = new RelatorioExtratoContaModel();
                return PartialView(relatorio);
            }
            else
            {
                ViewData["MensagemEmpty"] = "Nenhum resultado encontrado";
                ViewData["ExibirTotais"] = exibirTotais;

                var relatorio = await _relatorioExtratoContaService.ObterRelatorioExtratoContaAsync(_userResolverService.GrupoId, contaBancariaId.Value, dataInicial, dataFinal);

                return PartialView(relatorio);
            }
        }
    }
}