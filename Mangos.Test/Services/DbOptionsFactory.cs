using Mangos.Infra;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Mangos.Test.Services
{
    public class DbOptionsFactory
    {
        static DbOptionsFactory()
        {
            Configuration = new ConfigurationBuilder()
                                    .AddJsonFile("appsettings.json")
                                    .Build();

            string connectionString = Configuration.GetConnectionString("DefaultConnection");

            DbContextOptions = new DbContextOptionsBuilder<MangosDb>()
                .UseSqlServer(connectionString)
                .Options;

            DbMemoryContextOptions = new DbContextOptionsBuilder<MangosDb>()
                .UseInMemoryDatabase(databaseName: "Teste")
                .Options;
        }

        public static DbContextOptions<MangosDb> DbContextOptions { get; }
        public static DbContextOptions<MangosDb> DbMemoryContextOptions { get; }
        public static IConfiguration Configuration { get; }
    }
}