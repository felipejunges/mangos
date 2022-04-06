using FluentValidation;
using Mangos.Dominio.Interfaces;
using Mangos.Dominio.Interfaces.Repository;
using Mangos.Dominio.Services;
using Mangos.Dominio.Services.Relatorios;
using Mangos.Dominio.Settings;
using Mangos.Dominio.Validators;
using Mangos.Infra.Repository;
using Mangos.Infra.Uow;
using Mangos.Mvc.Models.GerenciarConta;
using Mangos.Mvc.Models.Validators;
using Mangos.Mvc.Models.ViewModels;
using Mangos.Mvc.Models.ViewModels.ModelBuilders;
using Mangos.Mvc.Services;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace Mangos.Mvc.Configuration
{
    public static class DependencyInjection
    {
        private static Assembly DomainAssembly => AppDomain.CurrentDomain.Load("Mangos.Dominio");

        public static IServiceCollection DeclareRepositorys(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<ICartaoCreditoRepository, CartaoCreditoRepository>();
            services.AddScoped<ICategoriaRepository, CategoriaRepository>();
            services.AddScoped<IContaBancariaRepository, ContaBancariaRepository>();
            services.AddScoped<IDispositivoConectadoRepository, DispositivoConectadoRepository>();
            services.AddScoped<IGrupoRepository, GrupoRepository>();
            services.AddScoped<ILancamentoCartaoRepository, LancamentoCartaoRepository>();
            services.AddScoped<ILancamentoFixoRepository, LancamentoFixoRepository>();
            services.AddScoped<ILancamentoRepository, LancamentoRepository>();
            services.AddScoped<ILogRepository, LogRepository>();
            services.AddScoped<IMetaMovimentacaoRepository, MetaMovimentacaoRepository>();
            services.AddScoped<IMetaSaldoRepository, MetaSaldoRepository>();
            services.AddScoped<IPessoaCoordenadaRepository, PessoaCoordenadaRepository>();
            services.AddScoped<IPessoaRepository, PessoaRepository>();
            services.AddScoped<IRendimentoMensalContaRepository, RendimentoMensalContaRepository>();
            services.AddScoped<ISaldoContaBancariaRepository, SaldoContaBancariaRepository>();
            services.AddScoped<ISessaoAcessoRepository, SessaoAcessoRepository>();
            services.AddScoped<ITransferenciaContaRepository, TransferenciaContaRepository>();
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();

            return services;
        }

        public static IServiceCollection DeclareServices(this IServiceCollection services)
        {
            services.AddScoped<DataKeeperService>();
            services.AddScoped<LoginService>();
            services.AddScoped<SessaoAcessoMVCService>();
            services.AddScoped<DashboardService>();
            services.AddScoped<ChartService>();

            services.AddScoped<CartaoCreditoService>();
            services.AddScoped<CategoriaService>();
            services.AddScoped<ConsultaVencimentoService>();
            services.AddScoped<ContaBancariaService>();
            services.AddScoped<GrupoService>();
            services.AddScoped<LancamentoCartaoService>();
            services.AddScoped<LancamentoFixoService>();
            services.AddScoped<LancamentoService>();
            services.AddScoped<LogService>();
            services.AddScoped<MetaMovimentacaoService>();
            services.AddScoped<MetaSaldoService>();
            services.AddScoped<NotificacaoService>();
            services.AddScoped<PessoaCoordenadaService>();
            services.AddScoped<PessoaService>();
            services.AddScoped<RendimentoMensalContaService>();
            services.AddScoped<SaldoContaBancariaService>();
            services.AddScoped<SessaoAcessoService>();
            services.AddScoped<TransferenciaContaService>();
            services.AddScoped<TrocaSenhaUsuarioService>();
            services.AddScoped<UsuarioService>();

            return services;
        }

        public static IServiceCollection DeclareRelatoriosServices(this IServiceCollection services)
        {
            services.AddScoped<RelatorioCartaoCreditoService>();
            services.AddScoped<RelatorioCategoriaService>();
            services.AddScoped<RelatorioExtratoContaService>();
            services.AddScoped<RelatorioProjecaoRealizacaoService>();
            services.AddScoped<RelatorioProjecaoSaldoService>();

            return services;
        }

        public static IServiceCollection DeclareValidators(this IServiceCollection services)
        {
            services.AddTransient<IValidator<CartaoCreditoDadosModel>, CartaoCreditoDadosValidator>();
            services.AddTransient<IValidator<CategoriaDadosModel>, CategoriaDadosValidator>();
            services.AddTransient<IValidator<ContaBancariaDadosModel>, ContaBancariaDadosValidator>();
            services.AddTransient<IValidator<DespesaRapidaViewModel>, DespesaRapidaModelValidator>();
            services.AddTransient<IValidator<EsqueciSenhaModel>, EsqueciSenhaValidator>();
            services.AddTransient<IValidator<FechamentoMesCartaoModel>, FechamentoMesCartaoValidator>();
            services.AddTransient<IValidator<GerenciarConfiguracoesGrupoModel>, GerenciarConfiguracoesGrupoValidator>();
            services.AddTransient<IValidator<GerenciarDadosCadastraisModel>, GerenciarDadosCadastraisValidator>();
            services.AddTransient<IValidator<GrupoDadosModel>, GrupoDadosValidator>();
            services.AddTransient<IValidator<LancamentoCartaoEdicaoModel>, LancamentoCartaoEdicaoValitor>();
            services.AddTransient<IValidator<LancamentoCartaoInclusaoModel>, LancamentoCartaoInclusaoValidator>();
            services.AddTransient<IValidator<LancamentoEdicaoModel>, LancamentoEdicaoValidator>();
            services.AddTransient<IValidator<LancamentoFixoDadosModel>, LancamentoFixoDadosValidator>();
            services.AddTransient<IValidator<LancamentoInclusaoModel>, LancamentoInclusaoValidator>();
            services.AddTransient<IValidator<LancamentoPagamentoModel>, LancamentoPagamentoValidator>();
            services.AddTransient<IValidator<LancamentoPagoInclusaoModel>, LancamentoPagoInclusaoValidator>();
            services.AddTransient<IValidator<MetaMovimentacaoDadosModel>, MetaMovimentacaoDadosValidator>();
            services.AddTransient<IValidator<MetaSaldoDadosModel>, MetaSaldoDadosValidator>();
            services.AddTransient<IValidator<PessoaCoordenadaDadosModel>, PessoaCoordenadaDadosValidator>();
            services.AddTransient<IValidator<PessoaDadosModel>, PessoaDadosValidator>();
            services.AddTransient<IValidator<ReaberturaMesCartaoModel>, ReaberturaMesCartaoValidator>();
            services.AddTransient<IValidator<RendimentoMensalContaDadosModel>, RendimentoMensalContaDadosValidator>();
            services.AddTransient<IValidator<TransferenciaContaDadosModel>, TransferenciaContaDadosValidator>();
            services.AddTransient<IValidator<UsuarioInclusaoModel>, UsuarioInclusaoValidator>();
            services.AddTransient<IValidator<UsuarioDadosModel>, UsuarioDadosValidator>();

            services.AddTransient<IValidator<TrocarSenhaModel>, TrocarSenhaValidator>();

            return services;
        }

        public static IServiceCollection DeclareModelBuilders(this IServiceCollection services)
        {
            services
                .AddScoped<CartaoCreditoDadosModelBuilder>()
                .AddScoped<CategoriaDadosModelBuilder>()
                .AddScoped<ContaBancariaDadosModelBuilder>()
                .AddScoped<GrupoDadosModelBuilder>()
                .AddScoped<LancamentoCartaoEdicaoModelBuilder>()
                .AddScoped<LancamentoEdicaoModelBinder>()
                .AddScoped<LancamentoFixoDadosModelBuilder>()
                .AddScoped<MetaMovimentacaoDadosModelBinder>()
                .AddScoped<MetaSaldoDadosModelBinder>()
                .AddScoped<PessoaCoordenadaDadosModelBinder>()
                .AddScoped<PessoaDadosModelBuilder>()
                .AddScoped<RendimentoMensalContaDadosModelBinder>()
                .AddScoped<TransferenciaContaDadosModelBuilder>()
                .AddScoped<UsuarioDadosModelBinder>();

            return services;
        }

        public static void AddMediator(this IServiceCollection services)
        {
            services.AddMediatR(DomainAssembly);

            AssemblyScanner
                .FindValidatorsInAssembly(DomainAssembly)
                .ForEach(result => services.AddScoped(result.InterfaceType, result.ValidatorType));

            //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        }

        public static IServiceCollection DeclareSettings(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            var emailSettings = configuration.GetSection("Services:EmailService").Get<EmailSettings>();
            var expiracaoLoginSettings = configuration.GetSection("ExpiracaoLogin").Get<ExpiracaoLoginSettings>();
            var geoLocationSettings = configuration.GetSection("GeoLocation").Get<GeoLocationSettings>();
            var limpezaLogsSettings = configuration.GetSection("LimpezaLogs").Get<LimpezaLogsSettings>();

            services
                .AddSingleton(emailSettings)
                .AddSingleton(expiracaoLoginSettings)
                .AddSingleton(geoLocationSettings)
                .AddSingleton(limpezaLogsSettings);

            return services;
        }
    }
}