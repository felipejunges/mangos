using Mangos.Dominio.Interfaces;
using Mangos.Dominio.Interfaces.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace Mangos.Logger
{
    public static class MangosLoggerFactoryExtensions
    {
        public static void UseMangosLogger(this IApplicationBuilder app, string aplicacao, IConfigurationSection config)
        {
            var serviceProvider = app.ApplicationServices;
            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();

            var scope = serviceProvider.CreateScope();

            ILogRepository repositoryFactory()
            {
                var repository = scope!.ServiceProvider.GetRequiredService<ILogRepository>();
                return repository;
            }

            IUnitOfWork unitOfWorkFactory()
            {
                var unitOfWork = scope!.ServiceProvider.GetRequiredService<IUnitOfWork>();
                return unitOfWork;
            }

            loggerFactory.AddProvider(new MangosLoggerProvider(CreateFilter(), repositoryFactory, unitOfWorkFactory, aplicacao));
        }

        private static Func<string, LogLevel, bool> CreateFilter()
        {
            // TODO: seria bacana buscar a configuração (variável config) para determinar o MinLevel
            return (category, logLevel) =>
            {
                return category.Contains(nameof(Mangos)) ? logLevel >= LogLevel.Information : logLevel >= LogLevel.Warning;
            };
        }
    }
}