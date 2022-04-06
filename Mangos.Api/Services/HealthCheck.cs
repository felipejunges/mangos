using Mangos.Dominio.Settings;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Mangos.Api.Services
{
    public class HealthCheck : IHealthCheck
    {
        private readonly HealthSettings _settings;

        public HealthCheck(HealthSettings settings)
        {
            _settings = settings;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                using (var connection = new SqlConnection(_settings.ConnectionString))
                {
                    await connection.OpenAsync();

                    if (connection.State == ConnectionState.Open)
                        await connection.CloseAsync();
                }

                return HealthCheckResult.Healthy();
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy("Falha na conexão ao Sql Server", ex);
            }
        }
    }
}