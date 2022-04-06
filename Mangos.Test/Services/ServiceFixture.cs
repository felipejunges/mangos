using Mangos.Dominio.Services;
using Mangos.Dominio.Settings;
using Mangos.Infra;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;

namespace Mangos.Test.Services
{
    public class ServiceFixture : IDisposable
    {
        public ServiceFixture()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            var configuration = builder.Build();

            var serviceCollection = new ServiceCollection();

            if (Convert.ToBoolean(configuration.GetSection("UseInMemoryDb").Value))
                serviceCollection.AddDbContext<MangosDb>(options => options.UseInMemoryDatabase(databaseName: "Mangos"));
            else
                serviceCollection.AddDbContext<MangosDb>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("Mangos.Infra")));

            serviceCollection
                .AddScoped<ContaBancariaService>()
                .AddScoped<LancamentoCartaoService>()
                .AddScoped<LancamentoFixoService>()
                .AddScoped<LancamentoService>()
                .AddScoped<PessoaCoordenadaService>()
                .AddScoped<SaldoContaBancariaService>();
                // .AddScoped<IRepository<CartaoCredito>, Repository<CartaoCredito>>()
                // .AddScoped<IRepository<ContaBancaria>, Repository<ContaBancaria>>()
                // .AddScoped<IRepository<Grupo>, Repository<Grupo>>()
                // .AddScoped<IRepository<Lancamento>, Repository<Lancamento>>()
                // .AddScoped<IRepository<LancamentoCartao>, Repository<LancamentoCartao>>()
                // .AddScoped<IRepository<LancamentoFixo>, Repository<LancamentoFixo>>()
                // .AddScoped<IRepository<Pessoa>, Repository<Pessoa>>()
                // .AddScoped<IRepository<PessoaCoordenada>, Repository<PessoaCoordenada>>()
                // .AddScoped<IRepository<RendimentoMensalConta>, Repository<RendimentoMensalConta>>()
                // .AddScoped<IRepository<SaldoContaBancaria>, Repository<SaldoContaBancaria>>()
                // .AddScoped<IRepository<TransferenciaConta>, Repository<TransferenciaConta>>();

            var geoLocationSettings = configuration.GetSection("GeoLocation").Get<GeoLocationSettings>();

            serviceCollection
                .AddSingleton(geoLocationSettings);

            ServiceProvider = serviceCollection.BuildServiceProvider();
        }

        public ServiceProvider ServiceProvider { get; private set; }

        public void Dispose() { }
    }
}