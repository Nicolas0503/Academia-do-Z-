using AcademiaDoZe.Application.DependencyInjection;
using AcademiaDoZe.Application.Enums;
using Microsoft.Extensions.DependencyInjection;
using Academia_do_Zé.Repositories;
using AcademiaDoZe.infrastructure.Repositories;
using AcademiaDoZe.infrastructure.Data;

//Nícolas Bastos

namespace AcademiaDoZe.Application.Tests
{
    public static class DependencyInjection
    {
        public static IServiceCollection ConfigureServices(string connectionString, EAppDatabaseType databaseType)
        {
            var services = new ServiceCollection();

            // Configura os serviços da camada de aplicação
            services.AddApplicationServices();

            // Configura a fábrica de repositórios com a string de conexão e tipo de banco
            services.AddSingleton(new RepositoryConfig { ConnectionString = connectionString, DatabaseType = databaseType });

            // Registro explícito dos repositórios necessários para os testes de integração usando factory
            services.AddScoped<IMatriculaRepository>(provider =>
            {
                var config = provider.GetRequiredService<RepositoryConfig>();
                return new MatriculaRepository(config.ConnectionString, (DatabaseType)config.DatabaseType);
            });
            services.AddScoped<IAlunoRepository>(provider =>
            {
                var config = provider.GetRequiredService<RepositoryConfig>();
                return new AlunoRepository(config.ConnectionString, (DatabaseType)config.DatabaseType);
            });
            services.AddScoped<ILogradouroRepository>(provider =>
            {
                var config = provider.GetRequiredService<RepositoryConfig>();
                return new LogradouroRepository(config.ConnectionString, (DatabaseType)config.DatabaseType);
            });
            services.AddScoped<IColaboradorRepository>(provider =>
            {
                var config = provider.GetRequiredService<RepositoryConfig>();
                return new ColaboradorRepository(config.ConnectionString, (DatabaseType)config.DatabaseType);
            });
            // Adicione outros repositórios se necessário

            return services;
        }

        public static IServiceProvider BuildServiceProvider(IServiceCollection services)
        {
            return services.BuildServiceProvider();
        }
    }
}
