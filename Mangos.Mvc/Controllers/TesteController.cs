using Mangos.Dominio.Extensions;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Mangos.Mvc.Controllers
{
    public class TesteController : Controller
    {
        public IActionResult Index()
        {
            var horas = @$"
DateTime.Now:                  {DateTime.Now:dd/MM/yyyy HH:mm:ss zzz}
DateTime.UtcNow:               {DateTime.UtcNow:dd/MM/yyyy HH:mm:ss zzz}
DateTime.UtcNow.BrasiliaToUtc: {DateTime.UtcNow.BrasiliaToUtc():dd/MM/yyyy HH:mm:ss zzz}
DateTime.UtcNow.UtcToBrasilia: {DateTime.UtcNow.UtcToBrasilia():dd/MM/yyyy HH:mm:ss zzz}

DateTimeWithZone.Now:    {DateTimeWithZone.Br(DateTime.Now).LocalTime:dd/MM/yyyy HH:mm:ss zzz}
DateTimeWithZone.UtcNow: {DateTimeWithZone.Br(DateTime.UtcNow).LocalTime:dd/MM/yyyy HH:mm:ss zzz}
";

            var tzs = TimeZoneInfo.GetSystemTimeZones();

            foreach(var tz in tzs)
            {
                horas += @$"
{tz.Id} - {tz.DisplayName} - {tz.StandardName}";
            }

            return Content(horas);
        }
    }
}