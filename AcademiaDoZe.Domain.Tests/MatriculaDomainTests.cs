using System;
using Academia_do_Zé.Entities;
using Academia_do_Zé.Enums;
using Academia_do_Zé.Exceptions;
using Academia_do_Zé.ValueObjects;
using Xunit;

//Nícolas Bastos

namespace AcademiaDoZe.Domain.Tests
{
    public class MatriculaDomainTests
    {
        private Logradouro GetValidLogradouro() => Logradouro.Criar("12345678", "Rua A", "Centro", "Cidade", "SP", "Brasil");
        private Arquivo GetValidArquivo() => Arquivo.Criar(new byte[1], ".jpg");
        private Aluno GetValidAluno(DateOnly? dataNascimento = null)
        {
            return Aluno.Criar(
                "João da Silva",
                "12345678901",
                dataNascimento ?? DateOnly.FromDateTime(DateTime.Today.AddYears(-20)),
                "11999999999",
                "joao@email.com",
                GetValidLogradouro(),
                "123",
                "Apto 1",
                "Senha@1",
                GetValidArquivo()
            );
        }

        [Fact]
        public void CriarMatricula_ComDadosValidos_DeveCriarObjeto()
        {
            // Arrange
            var aluno = GetValidAluno();
            var plano = EMatriculaPlano.Mensal;
            var dataInicio = DateOnly.FromDateTime(DateTime.Today);
            var dataFim = dataInicio.AddMonths(1);
            var objetivo = " Perder peso ";
            var restricoes = EMatriculaRestricoes.None;
            var laudo = GetValidArquivo();

            // Act
            var matricula = Matricula.Criar(aluno, plano, dataInicio, dataFim, objetivo, restricoes, laudo);

            // Assert
            Assert.NotNull(matricula);
        }

        [Fact]
        public void CriarMatricula_ComObjetivoComEspacos_DeveNormalizar()
        {
            // Arrange
            var aluno = GetValidAluno();
            var plano = EMatriculaPlano.Mensal;
            var dataInicio = DateOnly.FromDateTime(DateTime.Today);
            var dataFim = dataInicio.AddMonths(1);
            var objetivo = "  Ganhar massa  ";
            var restricoes = EMatriculaRestricoes.None;
            var laudo = GetValidArquivo();

            // Act
            var matricula = Matricula.Criar(aluno, plano, dataInicio, dataFim, objetivo, restricoes, laudo);

            // Assert
            Assert.Equal("Ganhar massa", matricula.Objetivo);
        }

        [Fact]
        public void CriarMatricula_ComAlunoNulo_DeveLancarExcecao()
        {
            // Arrange
            var plano = EMatriculaPlano.Mensal;
            var dataInicio = DateOnly.FromDateTime(DateTime.Today);
            var dataFim = dataInicio.AddMonths(1);
            var objetivo = "Perder peso";
            var restricoes = EMatriculaRestricoes.None;
            var laudo = GetValidArquivo();

            // Act & Assert
            var ex = Assert.Throws<DomainException>(() =>
                Matricula.Criar(null, plano, dataInicio, dataFim, objetivo, restricoes, laudo)
            );
            Assert.Equal("ALUNO_INVALIDO", ex.Message);
        }

        [Fact]
        public void CriarMatricula_MenorDe16SemLaudo_DeveLancarExcecao()
        {
            // Arrange
            var aluno = GetValidAluno(DateOnly.FromDateTime(DateTime.Today.AddYears(-15)));
            var plano = EMatriculaPlano.Mensal;
            var dataInicio = DateOnly.FromDateTime(DateTime.Today);
            var dataFim = dataInicio.AddMonths(1);
            var objetivo = "Perder peso";
            var restricoes = EMatriculaRestricoes.None;

            // Act & Assert
            var ex = Assert.Throws<DomainException>(() =>
                Matricula.Criar(aluno, plano, dataInicio, dataFim, objetivo, restricoes, null)
            );
            Assert.Equal("MENOR16_LAUDO_OBRIGATORIO", ex.Message);
        }

        [Fact]
        public void CriarMatricula_ComRestricaoSemLaudo_DeveLancarExcecao()
        {
            // Arrange
            var aluno = GetValidAluno();
            var plano = EMatriculaPlano.Mensal;
            var dataInicio = DateOnly.FromDateTime(DateTime.Today);
            var dataFim = dataInicio.AddMonths(1);
            var objetivo = "Perder peso";
            var restricoes = EMatriculaRestricoes.Diabetes;

            // Act & Assert
            var ex = Assert.Throws<DomainException>(() =>
                Matricula.Criar(aluno, plano, dataInicio, dataFim, objetivo, restricoes, null)
            );
            Assert.Equal("RESTRICOES_LAUDO_OBRIGATORIO", ex.Message);
        }

        [Fact]
        public void CriarMatricula_ComObjetivoVazio_DeveLancarExcecao()
        {
            // Arrange
            var aluno = GetValidAluno();
            var plano = EMatriculaPlano.Mensal;
            var dataInicio = DateOnly.FromDateTime(DateTime.Today);
            var dataFim = dataInicio.AddMonths(1);
            var restricoes = EMatriculaRestricoes.None;
            var laudo = GetValidArquivo();

            // Act & Assert
            var ex = Assert.Throws<DomainException>(() =>
                Matricula.Criar(aluno, plano, dataInicio, dataFim, "", restricoes, laudo)
            );
            Assert.Equal("OBJETIVO_OBRIGATORIO", ex.Message);
        }
    }
}
