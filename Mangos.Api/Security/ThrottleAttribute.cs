using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading;

namespace Mangos.Api.Security
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ThrottleAttribute : ActionFilterAttribute
    {
        public string Name { get; set; } = "Default";
        public int TempoCache { get; set; } = 15;
        public int StepSegundos { get; set; } = 1;

        private static readonly MemoryCache Cache = new MemoryCache(new MemoryCacheOptions());

        public override void OnActionExecuting(ActionExecutingContext c)
        {
            var key = string.Concat(Name, "-", c.HttpContext.Request.HttpContext.Connection.RemoteIpAddress);

            var tentativas = Cache.GetOrCreate(key, entry =>
            {
                entry.SetSlidingExpiration(TimeSpan.FromSeconds(TempoCache));

                return 0;
            });

            if (tentativas > 0)
                Thread.Sleep(tentativas * 1000 * StepSegundos);

            tentativas++;

            Cache.Set(key, tentativas);
        }
    }
}