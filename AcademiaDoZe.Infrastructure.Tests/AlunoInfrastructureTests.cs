using System.Threading.Tasks;
using AcademiaDoZe.Infrastructure.Tests;
using Academia_do_Zé.Entities;
using Academia_do_Zé.Repositories;
using AcademiaDoZe.infrastructure.Repositories;
using Xunit;

//Nícolas Bstos

namespace AcademiaDoZe.Infrastructure.Tests
{
    public class AlunoInfrastructureTests : TestBase
    {
        [Fact]
        public async Task Aluno_Adicionar()
        {
            var repoLogradouro = new LogradouroRepository(ConnectionString, DatabaseType);
            var logradouro = await repoLogradouro.ObterPorId(1);
            Assert.NotNull(logradouro);

            var arquivo = Academia_do_Zé.ValueObjects.Arquivo.Criar(new byte[] { 1, 2, 3 }, "jpg");
            var cpf = "98765432100";

            var repoAluno = new AlunoRepository(ConnectionString, DatabaseType);
            var cpfExiste = await repoAluno.CpfJaExiste(cpf);
            Assert.False(cpfExiste);

            var aluno = Aluno.Criar(
                nome: "Aluno Teste",
                cpf: cpf,
                dataNascimento: new DateOnly(2000, 1, 1), // Maior de 12 anos
                telefone: "11999999999",
                email: "aluno@teste.com",
                endereco: logradouro,
                numero: "123",
                complemento: "Apto 1",
                senha: "SenhaTeste123", // Senha com maiúscula
                foto: arquivo
            );

            var alunoInserido = await repoAluno.Adicionar(aluno);
            Assert.NotNull(alunoInserido);
            Assert.True(alunoInserido.Id > 0);
        }

        [Fact]
        public async Task Aluno_ObterPorCpf_Atualizar()
        {
            var cpf = "98765432100";
            var repoAluno = new AlunoRepository(ConnectionString, DatabaseType);
            var alunoExistente = await repoAluno.ObterPorCpf(cpf);
            Assert.NotNull(alunoExistente);

            var arquivo = Academia_do_Zé.ValueObjects.Arquivo.Criar(new byte[] { 4, 5, 6 }, "jpg");
            var alunoAtualizado = Aluno.Criar(
                nome: "Aluno Editado",
                cpf: alunoExistente.Cpf,
                dataNascimento: alunoExistente.DataNascimento,
                telefone: alunoExistente.Telefone,
                email: alunoExistente.Email,
                endereco: alunoExistente.Endereco,
                numero: alunoExistente.Numero,
                complemento: alunoExistente.Complemento,
                senha: alunoExistente.Senha,
                foto: arquivo
            );

            var idProperty = typeof(Entity).GetProperty("Id");
            idProperty?.SetValue(alunoAtualizado, alunoExistente.Id);

            var resultadoAtualizacao = await repoAluno.Atualizar(alunoAtualizado);
            Assert.NotNull(resultadoAtualizacao);
            Assert.Equal("Aluno Editado", resultadoAtualizacao.Nome);
        }

        [Fact]
        public async Task Aluno_ObterPorCpf_TrocarSenha()
        {
            var cpf = "98765432100";
            var repoAluno = new AlunoRepository(ConnectionString, DatabaseType);
            var alunoExistente = await repoAluno.ObterPorCpf(cpf);
            Assert.NotNull(alunoExistente);

            var novaSenha = "NovaSenhaAluno123"; // Senha com maiúscula
            var resultadoTrocaSenha = await repoAluno.TrocarSenha(alunoExistente.Id, novaSenha);
            Assert.True(resultadoTrocaSenha);

            var alunoAtualizado = await repoAluno.ObterPorId(alunoExistente.Id);
            Assert.NotNull(alunoAtualizado);
            Assert.Equal(novaSenha, alunoAtualizado.Senha);
        }

        [Fact]
        public async Task Aluno_ObterPorCpf_Remover_ObterPorId()
        {
            var cpf = "98765432100";
            var repoAluno = new AlunoRepository(ConnectionString, DatabaseType);
            var alunoExistente = await repoAluno.ObterPorCpf(cpf);
            Assert.NotNull(alunoExistente);

            var resultadoRemover = await repoAluno.Remover(alunoExistente.Id);
            Assert.True(resultadoRemover);

            var resultadoRemovido = await repoAluno.ObterPorId(alunoExistente.Id);
            Assert.Null(resultadoRemovido);
        }

        [Fact]
        public async Task Aluno_ObterTodos()
        {
            var repoAluno = new AlunoRepository(ConnectionString, DatabaseType);
            var resultado = await repoAluno.ObterTodos();
            Assert.NotNull(resultado);
            Assert.True(resultado.Any());
        }
    }
}
