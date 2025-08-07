using System;
using Academia_do_Zé.Entities;
using Academia_do_Zé.Enums;
using Academia_do_Zé.Exceptions;
using Academia_do_Zé.ValueObjects;
using Xunit;

namespace AcademiaDoZe.Domain.Tests
{
    public class ColaboradorDomainTests
    {
        private Logradouro GetValidLogradouro() => Logradouro.Criar("12345678", "Rua A", "Centro", "Cidade", "SP", "Brasil");
        private Arquivo GetValidArquivo() => Arquivo.Criar(new byte[1], ".jpg");

        [Fact]
        public void CriarColaborador_ComDadosValidos_DeveCriarObjeto()
        {
            // Arrange
            var nome = " Maria da Silva ";
            var cpf = "123.456.789-01";
            var dataNascimento = DateOnly.FromDateTime(DateTime.Today.AddYears(-30));
            var telefone = "11999999999";
            var email = " maria@email.com ";
            var endereco = GetValidLogradouro();
            var numero = " 123 ";
            var complemento = " Apto 1 ";
            var senha = " Senha@1 ";
            var foto = GetValidArquivo();
            var dataAdmissao = DateOnly.FromDateTime(DateTime.Today.AddYears(-1));
            var tipo = EColaboradorTipo.Atendente;
            var vinculo = EColaboradorVinculo.CLT;

            // Act
            var colaborador = Colaborador.Criar(nome, cpf, dataNascimento, telefone, email, endereco, numero, complemento, senha, foto, dataAdmissao, tipo, vinculo);

            // Assert
            Assert.NotNull(colaborador);
            Assert.Equal("Maria da Silva", colaborador.Nome);
            Assert.Equal("12345678901", colaborador.Cpf);
            Assert.Equal("11999999999", colaborador.Telefone);
            Assert.Equal("maria@email.com", colaborador.Email);
            Assert.Equal("123", colaborador.Numero);
            Assert.Equal("Apto 1", colaborador.Complemento);
            Assert.Equal("Senha@1", colaborador.Senha);
        }

        [Fact]
        public void CriarColaborador_ComNomeVazio_DeveLancarExcecao()
        {
            // Arrange
            var cpf = "12345678901";
            var dataNascimento = DateOnly.FromDateTime(DateTime.Today.AddYears(-30));
            var telefone = "11999999999";
            var email = "email@email.com";
            var endereco = GetValidLogradouro();
            var numero = "123";
            var complemento = "Apto 1";
            var senha = "Senha@1";
            var foto = GetValidArquivo();
            var dataAdmissao = DateOnly.FromDateTime(DateTime.Today.AddYears(-1));
            var tipo = EColaboradorTipo.Atendente;
            var vinculo = EColaboradorVinculo.CLT;

            // Act & Assert
            var ex = Assert.Throws<DomainException>(() =>
                Colaborador.Criar("", cpf, dataNascimento, telefone, email, endereco, numero, complemento, senha, foto, dataAdmissao, tipo, vinculo)
            );
            Assert.Equal("NOME_OBRIGATORIO", ex.Message);
        }

        [Fact]
        public void CriarColaborador_ComCpfInvalido_DeveLancarExcecao()
        {
            // Arrange
            var nome = "Maria";
            var cpf = "123";
            var dataNascimento = DateOnly.FromDateTime(DateTime.Today.AddYears(-30));
            var telefone = "11999999999";
            var email = "email@email.com";
            var endereco = GetValidLogradouro();
            var numero = "123";
            var complemento = "Apto 1";
            var senha = "Senha@1";
            var foto = GetValidArquivo();
            var dataAdmissao = DateOnly.FromDateTime(DateTime.Today.AddYears(-1));
            var tipo = EColaboradorTipo.Atendente;
            var vinculo = EColaboradorVinculo.CLT;

            // Act & Assert
            var ex = Assert.Throws<DomainException>(() =>
                Colaborador.Criar(nome, cpf, dataNascimento, telefone, email, endereco, numero, complemento, senha, foto, dataAdmissao, tipo, vinculo)
            );
            Assert.Equal("CPF_DIGITOS", ex.Message);
        }

        [Fact]
        public void CriarColaborador_ComDataAdmissaoNoFuturo_DeveLancarExcecao()
        {
            // Arrange
            var nome = "Maria";
            var cpf = "12345678901";
            var dataNascimento = DateOnly.FromDateTime(DateTime.Today.AddYears(-30));
            var telefone = "11999999999";
            var email = "email@email.com";
            var endereco = GetValidLogradouro();
            var numero = "123";
            var complemento = "Apto 1";
            var senha = "Senha@1";
            var foto = GetValidArquivo();
            var dataAdmissao = DateOnly.FromDateTime(DateTime.Today.AddDays(1));
            var tipo = EColaboradorTipo.Atendente;
            var vinculo = EColaboradorVinculo.CLT;

            // Act & Assert
            var ex = Assert.Throws<DomainException>(() =>
                Colaborador.Criar(nome, cpf, dataNascimento, telefone, email, endereco, numero, complemento, senha, foto, dataAdmissao, tipo, vinculo)
            );
            Assert.Equal("DATA_ADMISSAO_MAIOR_ATUAL", ex.Message);
        }

        [Fact]
        public void CriarColaborador_AdministradorComVinculoCLT_DeveLancarExcecao()
        {
            // Arrange
            var nome = "Maria";
            var cpf = "12345678901";
            var dataNascimento = DateOnly.FromDateTime(DateTime.Today.AddYears(-30));
            var telefone = "11999999999";
            var email = "email@email.com";
            var endereco = GetValidLogradouro();
            var numero = "123";
            var complemento = "Apto 1";
            var senha = "Senha@1";
            var foto = GetValidArquivo();
            var dataAdmissao = DateOnly.FromDateTime(DateTime.Today.AddYears(-1));
            var tipo = EColaboradorTipo.Administrador;
            var vinculo = EColaboradorVinculo.CLT;

            // Act & Assert
            var ex = Assert.Throws<DomainException>(() =>
                Colaborador.Criar(nome, cpf, dataNascimento, telefone, email, endereco, numero, complemento, senha, foto, dataAdmissao, tipo, vinculo)
            );
            Assert.Equal("ADMINISTRADOR_CLT_INVALIDO", ex.Message);
        }
    }
}
