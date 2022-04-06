using Mangos.Api.Configuration;
using Mangos.Api.Converters;
using Mangos.Api.Services;
using Mangos.Api.Services.User;
using Mangos.Dominio.Services.User;
using Mangos.Infra;
using Mangos.Logger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Mangos.Api
{
    public class Startup
    {
        private readonly string _allowSpecificOrigins = "AllowSpecificOrigins";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<MangosDb>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("Mangos.Infra")));

            services.DeclareServices()
                    .DeclareRepositorys()
                    .DeclareSettings(Configuration)
                    .ConfigureJwtAuths(Configuration);

            services.AddHttpContextAccessor();
            services.AddScoped<IUserResolverService, UserResolverService>();

            services.AddCors(options =>
            {
                options.AddPolicy(_allowSpecificOrigins,
                builder =>
                {
                    builder.AllowAnyHeader();
                    builder.AllowAnyMethod();
                    builder.AllowAnyOrigin();
                });
            });

            services.AddControllers().AddJsonOptions(opt =>
            {
                opt.JsonSerializerOptions.Converters.Add(new DateTimeConverter());
            });

            services.AddSwagger();
            services.AddHealthChecks().AddCheck<HealthCheck>("mangos_health_check");
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHttpsRedirection();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Mangos.Api v1"));

            app.UseRouting();
            app.UseCors(_allowSpecificOrigins);

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMangosLogger(nameof(Api), Configuration.GetSection("Logging"));

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health");
            });
        }
    }
}