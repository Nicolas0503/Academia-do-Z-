// Nicolas Bastos
using AcademiaDoZe.Application.DTOs;
using AcademiaDoZe.Application.Enums;
using AcademiaDoZe.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace AcademiaDoZe.Application.Tests
{
    public class LogradouroApplicationTests
    {
        const string connectionString = "Server=localhost;Database=db_academia_do_ze;Uid=root;Pwd=abcBolinhas12345;";
        const EAppDatabaseType databaseType = EAppDatabaseType.MySql;

        [Fact(Timeout = 60000)]
        public async Task LogradouroService_Integracao_Adicionar_Obter_Atualizar_Remover()
        {
            var services = DependencyInjection.ConfigureServices(connectionString, databaseType);
            var provider = DependencyInjection.BuildServiceProvider(services);
            var logradouroService = provider.GetRequiredService<ILogradouroService>();

            // Gera um CEP aleatório para evitar duplicidade
            var random = new Random();
            var cepAleatorio = random.Next(10000000, 99999999).ToString();

            var dto = new LogradouroDTO
            {
                Cep = cepAleatorio,
                Nome = "Rua Teste",
                Bairro = "Centro",
                Cidade = "São Paulo",
                Estado = "SP",
                Pais = "Brasil"
            };
            LogradouroDTO? criado = null;

            try
            {
                // Adicionar
                criado = await logradouroService.AdicionarAsync(dto);
                Assert.NotNull(criado);
                Assert.True(criado!.Id > 0);

                // Obter por Id
                var obtido = await logradouroService.ObterPorIdAsync(criado.Id);
                Assert.NotNull(obtido);
                Assert.Equal(criado.Id, obtido!.Id);

                // Atualizar
                var atualizar = new LogradouroDTO
                {
                    Id = criado.Id,
                    Cep = random.Next(10000000, 99999999).ToString(), // Novo CEP aleatório para atualização
                    Nome = "Rua Atualizada",
                    Bairro = "Bairro Novo",
                    Cidade = "Rio de Janeiro",
                    Estado = "RJ",
                    Pais = "Brasil"
                };
                var atualizado = await logradouroService.AtualizarAsync(atualizar);
                Assert.NotNull(atualizado);
                Assert.Equal("Rua Atualizada", atualizado.Nome);

                // Remover
                var removido = await logradouroService.RemoverAsync(criado.Id);
                Assert.True(removido);

                // Conferir remoção
                var aposRemocao = await logradouroService.ObterPorIdAsync(criado.Id);
                Assert.Null(aposRemocao);
            }
            finally
            {
                if (criado is not null)
                {
                    try { await logradouroService.RemoverAsync(criado.Id); } catch { }
                }
            }
        }
    }
}
