using Mangos.Dominio.Services.User;
using Mangos.Mvc.Models;
using Mangos.Mvc.Services;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Mangos.Mvc.Controllers
{
    public class ErrorsController : BaseController
    {
        public ErrorsController(DataKeeperService dataKeeperService, IUserResolverService userResolverService) : base(dataKeeperService, userResolverService)
        {
        }

        [Route("Error")]
        public IActionResult Error()
        {
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
            var exception = context?.Error; // Your exception
            var code = 500; // Internal Server Error by default

            //if (exception is MyNotFoundException) code = 404; // Not Found
            //else if (exception is MyUnauthException) code = 401; // Unauthorized
            //else if (exception is MyException) code = 400; // Bad Request

            Response.StatusCode = code; // You can use HttpStatusCode enum instead

            if (exception is null)
                return View();

            return View(new MangoError(exception)); // Your error model
        }
    }
}