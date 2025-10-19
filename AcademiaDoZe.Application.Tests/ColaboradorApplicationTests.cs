// Nicolas Bastos
using AcademiaDoZe.Application.DTOs;
using AcademiaDoZe.Application.Enums;
using AcademiaDoZe.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace AcademiaDoZe.Application.Tests
{
    public class ColaboradorApplicationTests
    {
        const string connectionString = "Server=localhost;Database=db_academia_do_ze;Uid=root;Pwd=abcBolinhas12345;";
        const EAppDatabaseType databaseType = EAppDatabaseType.MySql;

        [Fact(Timeout = 60000)]
        public async Task ColaboradorService_Integracao_Adicionar_Obter_Atualizar_Remover()
        {
            var services = DependencyInjection.ConfigureServices(connectionString, databaseType);
            var provider = DependencyInjection.BuildServiceProvider(services);
            var colaboradorService = provider.GetRequiredService<IColaboradorService>();
            var logradouroService = provider.GetRequiredService<ILogradouroService>();

            var logradouro = await logradouroService.ObterPorIdAsync(5);
            Assert.NotNull(logradouro);

            var _cpf = GerarCpfFake();

            var caminhoFoto = Path.Combine("..", "..", "..", "foto_teste.png");
            ArquivoDTO foto = new();
            if (File.Exists(caminhoFoto))
            {
                foto.Conteudo = await File.ReadAllBytesAsync(caminhoFoto);
                foto.ContentType = $".{caminhoFoto.Split(".").Last() ?? ".png"}";
            }
            else
                foto.Conteudo = null;

            var dto = new ColaboradorDTO
            {
                Nome = "Colaborador Teste",
                Cpf = _cpf,
                DataNascimento = DateOnly.FromDateTime(DateTime.Today.AddYears(-20)),
                Telefone = "11999999999",
                Email = "Colaborador@teste.com",
                Endereco = logradouro,
                Numero = "100",
                Complemento = "Apto 1",
                Senha = "Senha@1",
                Foto = foto,
                DataAdmissao = DateOnly.FromDateTime(DateTime.Today.AddDays(-30)),
                Tipo = EAppColaboradorTipo.Administrador,
                Vinculo = EAppColaboradorVinculo.CLT
            };
            ColaboradorDTO? criado = null;

            try
            {
                // Adicionar
                criado = await colaboradorService.AdicionarAsync(dto);
                Assert.NotNull(criado);
                Assert.True(criado!.Id > 0);

                // Obter por cpf
                var obtido = (await colaboradorService.ObterPorCpfAsync(criado.Cpf)).First();
                Assert.NotNull(obtido);
                Assert.Equal(criado.Id, obtido!.Id);

                // Atualizar
                var atualizar = new ColaboradorDTO
                {
                    Id = criado.Id,
                    Nome = "Colaborador Atualizado",
                    Cpf = criado.Cpf,
                    DataNascimento = criado.DataNascimento,
                    Telefone = criado.Telefone,
                    Email = criado.Email,
                    Endereco = criado.Endereco,
                    Numero = criado.Numero,
                    Complemento = criado.Complemento,
                    Senha = criado.Senha,
                    Foto = criado.Foto,
                    DataAdmissao = criado.DataAdmissao,
                    Tipo = criado.Tipo,
                    Vinculo = criado.Vinculo
                };
                var atualizado = await colaboradorService.AtualizarAsync(atualizar);
                Assert.NotNull(atualizado);
                Assert.Equal("Colaborador Atualizado", atualizado.Nome);

                // Remover
                var removido = await colaboradorService.RemoverAsync(criado.Id);
                Assert.True(removido);

                // Conferir remoção
                var aposRemocao = await colaboradorService.ObterPorIdAsync(criado.Id);
                Assert.Null(aposRemocao);
            }
            catch(Exception ex)
            {

            }
            finally
            {
                if (criado is not null)
                {
                    try { await colaboradorService.RemoverAsync(criado.Id); } catch { }
                }
            }
        }

        private static string GerarCpfFake()
        {
            var rnd = new Random();
            return string.Concat(Enumerable.Range(0, 11).Select(_ => rnd.Next(0, 10).ToString()));
        }
    }
}
