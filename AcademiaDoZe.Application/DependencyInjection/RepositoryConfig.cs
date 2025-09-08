using AcademiaDoZe.Application.Enums;

//Nicolas Bastos
namespace AcademiaDoZe.Application.DependencyInjection
{
    public class RepositoryConfig
    {
        public required string ConnectionString { get; set; }
        public required EAppDatabaseType DatabaseType { get; set; }
    }
}