namespace Mangos.Dominio.Settings
{
    public class HealthSettings
    {
        public string ConnectionString { get; set; }

        public HealthSettings()
        {
            ConnectionString = string.Empty;
        }

        public HealthSettings(string connectionString)
        {
            ConnectionString = connectionString;
        }
    }
}