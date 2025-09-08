// Nícolas Bastos

using System;
using System.Linq;
using System.Threading.Tasks;
using AcademiaDoZe.Infrastructure.Tests;
using Academia_do_Zé.Entities;
using Academia_do_Zé.Repositories;
using AcademiaDoZe.infrastructure.Repositories;
using Xunit;

namespace AcademiaDoZe.Infrastructure.Tests
{
    public class AlunoInfrastructureTests : TestBase
    {
        private string GerarCpfUnico()
        {
            // Gera um CPF aleatório para evitar conflitos nos testes
            var rand = new Random();
            return string.Concat(Enumerable.Range(0, 11).Select(_ => rand.Next(0, 9).ToString()));
        }

        private DateOnly DataNascimentoValida()
        {
            // Garante pelo menos 12 anos completos
            var hoje = DateTime.Today;
            return new DateOnly(hoje.Year - 13, hoje.Month, hoje.Day);
        }

        [Fact]
        public async Task Aluno_Adicionar()
        {
            var repoLogradouro = new LogradouroRepository(ConnectionString, DatabaseType);
            var logradouro = await repoLogradouro.ObterPorId(1);
            Assert.NotNull(logradouro);

            var arquivo = Academia_do_Zé.ValueObjects.Arquivo.Criar(new byte[] { 1, 2, 3 }, "jpeg");
            var cpf = GerarCpfUnico();

            var repoAluno = new AlunoRepository(ConnectionString, DatabaseType);
            var cpfExiste = await repoAluno.CpfJaExiste(cpf);
            Assert.False(cpfExiste);

            var aluno = Aluno.Criar(
                nome: "Aluno Teste",
                cpf: cpf,
                dataNascimento: DataNascimentoValida(),
                telefone: "11999999999",
                email: "aluno@teste.com",
                endereco: logradouro,
                numero: "123",
                complemento: "Apto 1",
                senha: "SenhaTeste123",
                foto: arquivo
            );

            var alunoInserido = await repoAluno.Adicionar(aluno);
            Assert.NotNull(alunoInserido);
            Assert.True(alunoInserido.Id > 0);

            // Limpeza: remove o aluno inserido
            await repoAluno.Remover(alunoInserido.Id);
        }

        [Fact]
        public async Task Aluno_ObterPorCpf_Atualizar()
        {
            var repoLogradouro = new LogradouroRepository(ConnectionString, DatabaseType);
            var logradouro = await repoLogradouro.ObterPorId(1);
            Assert.NotNull(logradouro);

            var arquivo = Academia_do_Zé.ValueObjects.Arquivo.Criar(new byte[] { 1, 2, 3 }, "jpeg");
            var cpf = GerarCpfUnico();

            var repoAluno = new AlunoRepository(ConnectionString, DatabaseType);

            var aluno = Aluno.Criar(
                nome: "Aluno Teste",
                cpf: cpf,
                dataNascimento: DataNascimentoValida(),
                telefone: "11999999999",
                email: "aluno@teste.com",
                endereco: logradouro,
                numero: "123",
                complemento: "Apto 1",
                senha: "SenhaTeste123",
                foto: arquivo
            );
            var alunoInserido = await repoAluno.Adicionar(aluno);

            var alunoExistente = await repoAluno.ObterPorCpf(cpf);
            Assert.NotNull(alunoExistente);

            var novoArquivo = Academia_do_Zé.ValueObjects.Arquivo.Criar(new byte[] { 4, 5, 6 }, "jpeg");
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
                foto: novoArquivo
            );

            var idProperty = typeof(Entity).GetProperty("Id");
            idProperty?.SetValue(alunoAtualizado, alunoExistente.Id);

            var resultadoAtualizacao = await repoAluno.Atualizar(alunoAtualizado);
            Assert.NotNull(resultadoAtualizacao);
            Assert.Equal("Aluno Editado", resultadoAtualizacao.Nome);

            // Limpeza
            await repoAluno.Remover(alunoExistente.Id);
        }

        [Fact]
        public async Task Aluno_ObterPorCpf_TrocarSenha()
        {
            var repoLogradouro = new LogradouroRepository(ConnectionString, DatabaseType);
            var logradouro = await repoLogradouro.ObterPorId(1);
            Assert.NotNull(logradouro);

            var arquivo = Academia_do_Zé.ValueObjects.Arquivo.Criar(new byte[] { 1, 2, 3 }, "jpeg");
            var cpf = GerarCpfUnico();

            var repoAluno = new AlunoRepository(ConnectionString, DatabaseType);

            var aluno = Aluno.Criar(
                nome: "Aluno Teste",
                cpf: cpf,
                dataNascimento: DataNascimentoValida(),
                telefone: "11999999999",
                email: "aluno@teste.com",
                endereco: logradouro,
                numero: "123",
                complemento: "Apto 1",
                senha: "SenhaTeste123",
                foto: arquivo
            );
            var alunoInserido = await repoAluno.Adicionar(aluno);

            var alunoExistente = await repoAluno.ObterPorCpf(cpf);
            Assert.NotNull(alunoExistente);

            var novaSenha = "NovaSenhaAluno123";
            var resultadoTrocaSenha = await repoAluno.TrocarSenha(alunoExistente.Id, novaSenha);
            Assert.True(resultadoTrocaSenha);

            var alunoAtualizado = await repoAluno.ObterPorId(alunoExistente.Id);
            Assert.NotNull(alunoAtualizado);
            Assert.Equal(novaSenha, alunoAtualizado.Senha);

            // Limpeza
            await repoAluno.Remover(alunoExistente.Id);
        }

        [Fact]
        public async Task Aluno_ObterPorCpf_Remover_ObterPorId()
        {
            var repoLogradouro = new LogradouroRepository(ConnectionString, DatabaseType);
            var logradouro = await repoLogradouro.ObterPorId(1);
            Assert.NotNull(logradouro);

            var arquivo = Academia_do_Zé.ValueObjects.Arquivo.Criar(new byte[] { 1, 2, 3 }, "jpeg");
            var cpf = GerarCpfUnico();

            var repoAluno = new AlunoRepository(ConnectionString, DatabaseType);

            var aluno = Aluno.Criar(
                nome: "Aluno Teste",
                cpf: cpf,
                dataNascimento: DataNascimentoValida(),
                telefone: "11999999999",
                email: "aluno@teste.com",
                endereco: logradouro,
                numero: "123",
                complemento: "Apto 1",
                senha: "SenhaTeste123",
                foto: arquivo
            );
            var alunoInserido = await repoAluno.Adicionar(aluno);

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
