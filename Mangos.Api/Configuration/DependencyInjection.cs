using Mangos.Api.Services;
using Mangos.Dominio.Services;
using Mangos.Dominio.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;

namespace Mangos.Api.Configuration
{
    public static class DependencyInjection
    {
        public static IServiceCollection DeclareRepositorys(this IServiceCollection services)
        {
            // services
            //     .AddScoped<IRepository<CartaoCredito>, Repository<CartaoCredito>>()
            //     .AddScoped<IRepository<ContaBancaria>, Repository<ContaBancaria>>()
            //     .AddScoped<IRepository<DispositivoConectado>, Repository<DispositivoConectado>>()
            //     .AddScoped<IRepository<Lancamento>, Repository<Lancamento>>()
            //     .AddScoped<IRepository<LancamentoCartao>, Repository<LancamentoCartao>>()
            //     .AddScoped<IRepository<Log>, Repository<Log>>()
            //     .AddScoped<IRepository<Pessoa>, Repository<Pessoa>>()
            //     .AddScoped<IRepository<PessoaCoordenada>, Repository<PessoaCoordenada>>()
            //     .AddScoped<IRepository<RendimentoMensalConta>, Repository<RendimentoMensalConta>>()
            //     .AddScoped<IRepository<SaldoContaBancaria>, Repository<SaldoContaBancaria>>()
            //     .AddScoped<IRepository<TransferenciaConta>, Repository<TransferenciaConta>>()
            //     .AddScoped<IRepository<Usuario>, Repository<Usuario>>();

            return services;
        }

        public static IServiceCollection DeclareServices(this IServiceCollection services)
        {
            services
                .AddScoped<ContaBancariaService>()
                .AddScoped<DispositivoConectadoService>()
                .AddScoped<LoginService>()
                .AddScoped<JwtService>()
                .AddScoped<PessoaCoordenadaService>()
                .AddScoped<SaldoContaBancariaService>()
                .AddScoped<UsuarioService>();

            return services;
        }

        public static IServiceCollection DeclareSettings(this IServiceCollection services, IConfiguration configuration)
        {
            var geoLocationSettings = configuration.GetSection("GeoLocation").Get<GeoLocationSettings>();

            string connectionString = configuration.GetConnectionString("DefaultConnection");

            services
                .AddSingleton(geoLocationSettings)
                .AddSingleton(x => new HealthSettings(connectionString));
            
            return services;
        }

        public static IServiceCollection ConfigureJwtAuths(this IServiceCollection services, IConfiguration configuration)
        {
            var tokenConfigurations = new TokenConfigurations();
            new ConfigureFromConfigurationOptions<TokenConfigurations>(configuration.GetSection("TokenConfigurations")).Configure(tokenConfigurations);
            services.AddSingleton(tokenConfigurations);

            var signingConfigurations = new SigningConfigurations(tokenConfigurations.Key);
            services.AddSingleton(signingConfigurations);

            services.AddAuthentication(authOptions =>
            {
                authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                authOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(bearerOptions =>
            {
                var paramsValidation = bearerOptions.TokenValidationParameters;
                paramsValidation.IssuerSigningKey = signingConfigurations.Key;
                paramsValidation.ValidAudience = tokenConfigurations.Audience;
                paramsValidation.ValidIssuer = tokenConfigurations.Issuer;

                // Valida a assinatura de um token recebido
                paramsValidation.ValidateIssuerSigningKey = true;

                // Verifica se um token recebido ainda é válido
                paramsValidation.ValidateLifetime = true;

                // Tempo de tolerância para a expiração de um token (utilizado
                // caso haja problemas de sincronismo de horário entre diferentes
                // computadores envolvidos no processo de comunicação)
                paramsValidation.ClockSkew = TimeSpan.Zero;
            });

            services.AddAuthorization(auth =>
            {
                auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser().Build());
            });

            return services;
        }
    }
}