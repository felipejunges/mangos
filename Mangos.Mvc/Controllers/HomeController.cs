using Mangos.Dominio.Services.User;
using Mangos.Mvc.Models.ViewModels;
using Mangos.Mvc.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Mangos.Mvc.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        private readonly DashboardService _dashboardService;

        public HomeController(DataKeeperService dataKeeperService, IUserResolverService userResolverService, DashboardService dashboardService) : base(dataKeeperService, userResolverService)
        {
            _dashboardService = dashboardService;
        }

        [ResponseCache(Duration = 60)]
        public async Task<IActionResult> Index()
        {
            var model = new DashboardViewModel(
                await _dashboardService.CriarChartResultadosMeses(_userResolverService.GrupoId),
                await _dashboardService.CriarChartSaldosConta(_userResolverService.GrupoId),
                await _dashboardService.CriarChartTotaisCategoria(_userResolverService.GrupoId)
            );

            return View(model);
        }
    }
}