using AcademiaDoZe.Application.DependencyInjection;
using AcademiaDoZe.Application.Enums;
namespace AcademiaDoZe.Presentation.AppMaui.Configuration
{
    public static class ConfigurationHelper
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            string connectionString = GetMySqlConnectionString();

            const EAppDatabaseType databaseType = EAppDatabaseType.MySql;
            // Configura a fábrica de repositórios com a string de conexão e tipo de banco
            services.AddSingleton(new RepositoryConfig
            {
                ConnectionString = connectionString,
                DatabaseType = databaseType
            });
            // configura os serviços da camada de aplicação
            services.AddApplicationServices();
        }

        private static string GetSQLConnectionString()
        {
            // dados conexão
            const string dbServer = "172.24.32.1";
            const string dbDatabase = "db_academia_do_ze";
            const string dbUser = "sa";
            const string dbPassword = "abcBolinhas12345";
            const string dbComplemento = "TrustServerCertificate=True;Encrypt=True;";
            // se for necessário indicar a porta, incluir junto em dbComplemento

            // Configurações de conexão
            return $"Server={dbServer};Database={dbDatabase};User Id={dbUser};Password={dbPassword};{dbComplemento}";

        }
        private static string GetMySqlConnectionString()
        {
           return "Server=localhost;Database=db_academia_do_ze;Uid=root;Pwd=abcBolinhas12345;";
        }

    }
}