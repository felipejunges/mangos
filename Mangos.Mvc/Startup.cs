using FluentValidation.AspNetCore;
using Mangos.Dominio.Services;
using Mangos.Dominio.Services.Interface;
using Mangos.Dominio.Services.User;
using Mangos.Dominio.Utils;
using Mangos.Infra;
using Mangos.Logger;
using Mangos.Mvc.Configuration;
using Mangos.Mvc.Services.User;
using Mangos.Mvc.Utils;
using Mangos.Mvc.Utils.Providers;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace Mangos.Mvc
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment WebHostEnvironment { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            Configuration = configuration;
            WebHostEnvironment = webHostEnvironment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            if (Convert.ToBoolean(Configuration.GetSection("UseInMemoryDb").Value))
                services.AddDbContext<MangosDb>(options => options.UseInMemoryDatabase(databaseName: "Mangos"));
            else
                services.AddDbContext<MangosDb>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("Mangos.Infra")));

            services.AddSingleton<IEmailService, EmailService>();

            services.AddMediator();
            services.AddHttpContextAccessor();
            services.AddScoped<IUserResolverService, UserResolverService>();

            services.DeclareServices()
                    .DeclareRelatoriosServices()
                    .DeclareRepositorys()
                    .DeclareSettings(Configuration, WebHostEnvironment)
                    .DeclareValidators()
                    .DeclareModelBuilders();

            services
                .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
                {
                    options.LoginPath = "/Login/Login";
                    options.LogoutPath = "/Login/Logout";
                    options.Events.OnValidatePrincipal = CookieUpdateValidator.Validate;
                });

            // Necessário para o Wangkanai v5
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(10);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            services.AddResponseCaching();

            services.AddDataProtection()
                .SetApplicationName("Mangos Financeiro")
                .PersistKeysToFileSystem(new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + @"\Etc\Keys"))
                .SetDefaultKeyLifetime(TimeSpan.FromDays(90));

            services.AddDetection();

            services.AddMvc(options => { options.ValueProviderFactories.Insert(0, new MangosQueryStringValueProviderFactory()); })
                .AddFluentValidation();

            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var supportedCultures = new List<CultureInfo> { new CultureInfo("pt-BR") };
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(culture: "pt-BR", uiCulture: "pt-BR"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");

                app.UseHsts();

                app.UseHttpsRedirection();
            }

            app.UseStaticFiles();

            app.UseDetection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseResponseCaching();

            app.UseMangosLogger(nameof(Mvc), Configuration.GetSection("Logging"));

            // Necessário para o Wangkanai v5
            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}