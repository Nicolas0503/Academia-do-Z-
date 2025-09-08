using System;
using Academia_do_Zé.Entities;
using Academia_do_Zé.Enums;
using Academia_do_Zé.Exceptions;
using Academia_do_Zé.ValueObjects;
using Xunit;


//Nícolas Bastos
namespace AcademiaDoZe.Domain.Tests
{
    public class AcessoDomainTests
    {
        private Aluno GetValidAluno() =>
            Aluno.Criar(
                "João da Silva",
                "12345678901",
                DateOnly.FromDateTime(DateTime.Today.AddYears(-20)),
                "11999999999",
                "joao@email.com",
                Logradouro.Criar(12345678, "Rua A", "Centro", "Cidade", "SP", "Brasil", ""),
                "123",
                "Apto 1",
                "Senha@1",
                Arquivo.Criar(new byte[1], ".jpg")
            );

        private Colaborador GetValidColaborador() =>
            Colaborador.Criar(
                "Maria da Silva",
                "12345678901",
                DateOnly.FromDateTime(DateTime.Today.AddYears(-30)),
                "11999999999",
                "maria@email.com",
                Logradouro.Criar(12345678, "Rua A", "Centro", "Cidade", "SP", "Brasil", ""),
                "123",
                "Apto 1",
                "Senha@1",
                Arquivo.Criar(new byte[1], ".jpg"),
                DateOnly.FromDateTime(DateTime.Today.AddYears(-1)),
                EColaboradorTipo.Atendente,
                EColaboradorVinculo.CLT
            );

        [Fact]
        public void CriarAcesso_ComDadosValidos_DeveCriarObjeto()
        {
            // Arrange
            var aluno = GetValidAluno();
            
            var dataHora = DateTime.Now.AddMinutes(1);
            var acesso = Acesso.Criar(EPessoaTipo.Aluno, aluno, dataHora);


            // Act
            

            // Assert
            Assert.NotNull(acesso);
            Assert.Equal(EPessoaTipo.Aluno, acesso.Tipo);
            Assert.Equal(aluno, acesso.AlunoColaborador);
            Assert.Equal(dataHora, acesso.DataHora);
        }


        [Fact]
        public void CriarAcesso_ComTipoInvalido_DeveLancarExcecao()
        {
            // Arrange
            var aluno = GetValidAluno();
            var dataHora = DateTime.Today.AddHours(8);

            // Act & Assert
            var ex = Assert.Throws<DomainException>(() =>
                Acesso.Criar((EPessoaTipo)99, aluno, dataHora)
            );
            Assert.Equal("TIPO_OBRIGATORIO", ex.Message);
        }

        [Fact]
        public void CriarAcesso_ComPessoaNula_DeveLancarExcecao()
        {
            // Arrange
            var dataHora = DateTime.Today.AddHours(8);

            // Act & Assert
            var ex = Assert.Throws<DomainException>(() =>
                Acesso.Criar(EPessoaTipo.Aluno, null, dataHora)
            );
            Assert.Equal("PESSOA_OBRIGATORIA", ex.Message);
        }

        [Fact]
        public void CriarAcesso_ComDataHoraNoPassado_DeveLancarExcecao()
        {
            // Arrange
            var aluno = GetValidAluno();
            var dataHora = DateTime.Now.AddMinutes(-1);

            // Act & Assert
            var ex = Assert.Throws<DomainException>(() =>
                Acesso.Criar(EPessoaTipo.Aluno, aluno, dataHora)
            );
            Assert.Equal("DATAHORA_INVALIDA", ex.Message);
        }

      
    }
}
