﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Academia_do_Zé.Entities;
using Academia_do_Zé.Exceptions;
using Academia_do_Zé.ValueObjects;
using Xunit;

//Nícolas Bastos

namespace AcademiaDoZe.Domain.Tests
{
    public class AlunoDomainTests
    {
        // Padrão AAA (Arrange, Act, Assert)
        // Arrange (Organizar): Preparamos tudo que o teste precisa.
        private Logradouro GetValidLogradouro() => Logradouro.Criar("12345678", "Rua A", "Centro", "Cidade", "SP", "Brasil");
        private Arquivo GetValidArquivo() => Arquivo.Criar(new byte[1], ".jpg");
        [Fact] // [Fact] é um atributo que marca este método como um teste para o xUnit.
        public void CriarAluno_ComDadosValidos_DeveCriarObjeto() // Padrão de Nomenclatura: MetodoTestado_Cenario_ResultadoEsperado
        {
            // Arrange
            var nome = "João da Silva"; var cpf = "12345678901"; var dataNascimento = DateOnly.FromDateTime(DateTime.Today.AddYears(-20)); var telefone = "11999999999";
            var email = "joao@email.com"; var logradouro = GetValidLogradouro(); var numero = "123"; var complemento = "Apto 1"; var senha = "Senha@1"; var foto = GetValidArquivo();
            // Act
            var aluno = Aluno.Criar(nome, cpf, dataNascimento, telefone, email, logradouro, numero, complemento, senha, foto);
            // Assert
            Assert.NotNull(aluno);
        }
        [Fact]
        public void CriarAluno_ComNomeVazio_DeveLancarExcecao()
        {
            // Arrange
            var cpf = "12345678901"; var dataNascimento = DateOnly.FromDateTime(DateTime.Today.AddYears(-20)); var telefone = "11999999999";
            var email = "joao@email.com"; var logradouro = GetValidLogradouro(); var numero = "123"; var complemento = "Apto 1"; var senha = "Senha@123"; var foto = GetValidArquivo();
            // Act & Assert
            var ex = Assert.Throws<DomainException>(() =>
            Aluno.Criar(
            "",
            cpf,
            dataNascimento,
            telefone,
            email,
            logradouro,
            numero,
            complemento,
            senha,
            foto
            ));
            Assert.Equal("NOME_OBRIGATORIO", ex.Message);
        }
    }
}
