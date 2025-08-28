using System.Threading.Tasks;
using AcademiaDoZe.Infrastructure.Tests;
using Academia_do_Zé.Entities;
using Academia_do_Zé.Enums;
using Academia_do_Zé.ValueObjects;
using AcademiaDoZe.infrastructure.Repositories;
using Xunit;

//Nícolas Bastos

namespace AcademiaDoZe.Infrastructure.Tests
{
    public class MatriculaInfrastructureTests : TestBase
    {
        [Fact]
        public async Task Matricula_Adicionar()
        {
            var repoAluno = new AlunoRepository(ConnectionString, DatabaseType);
            var aluno = await repoAluno.ObterPorId(1); // Use um ID válido existente

            var arquivo = Arquivo.Criar(new byte[] { 1, 2, 3 }, "pdf");
            var matricula = Matricula.Criar(
                alunoMatricula: aluno!,
                plano: EMatriculaPlano.Mensal,
                dataInicio: DateOnly.FromDateTime(DateTime.Today),
                dataFim: DateOnly.FromDateTime(DateTime.Today.AddMonths(1)),
                objetivo: "Ganhar massa",
                restricoesMedicas: EMatriculaRestricoes.None,
                laudoMedico: null,
                observacoesRestricoes: ""
            );

            var repoMatricula = new MatriculaRepository(ConnectionString, DatabaseType);
            var matriculaInserida = await repoMatricula.Adicionar(matricula);
            Assert.NotNull(matriculaInserida);
            Assert.True(matriculaInserida.Id > 0);
        }

        [Fact]
        public async Task Matricula_ObterPorAluno_Atualizar()
        {
            var repoMatricula = new MatriculaRepository(ConnectionString, DatabaseType);
            var matriculas = await repoMatricula.ObterPorAluno(1); // Use um ID válido existente
            Assert.NotNull(matriculas);
            var matriculaExistente = matriculas.FirstOrDefault();
            Assert.NotNull(matriculaExistente);

            var matriculaAtualizada = Matricula.Criar(
                alunoMatricula: matriculaExistente.AlunoMatricula,
                plano: EMatriculaPlano.Trimestral,
                dataInicio: matriculaExistente.DataInicio,
                dataFim: matriculaExistente.DataFim.AddMonths(2),
                objetivo: "Definição muscular",
                restricoesMedicas: EMatriculaRestricoes.None,
                laudoMedico: matriculaExistente.LaudoMedico,
                observacoesRestricoes: "Sem restrições"
            );

            var idProperty = typeof(Entity).GetProperty("Id");
            idProperty?.SetValue(matriculaAtualizada, matriculaExistente.Id);

            var resultadoAtualizacao = await repoMatricula.Atualizar(matriculaAtualizada);
            Assert.NotNull(resultadoAtualizacao);
            Assert.Equal("Definição muscular", resultadoAtualizacao.Objetivo);
        }

        [Fact]
        public async Task Matricula_ObterPorAluno_Remover_ObterPorId()
        {
            var repoMatricula = new MatriculaRepository(ConnectionString, DatabaseType);
            var matriculas = await repoMatricula.ObterPorAluno(1); // Use um ID válido existente
            Assert.NotNull(matriculas);
            var matriculaExistente = matriculas.FirstOrDefault();
            Assert.NotNull(matriculaExistente);

            var resultadoRemover = await repoMatricula.Remover(matriculaExistente.Id);
            Assert.True(resultadoRemover);

            var resultadoRemovido = await repoMatricula.ObterPorId(matriculaExistente.Id);
            Assert.Null(resultadoRemovido);
        }

        [Fact]
        public async Task Matricula_ObterAtivas()
        {
            var repoMatricula = new MatriculaRepository(ConnectionString, DatabaseType);
            var ativas = await repoMatricula.ObterAtivas();
            Assert.NotNull(ativas);
            Assert.True(ativas.Any());
        }

        [Fact]
        public async Task Matricula_ObterVencendoEmDias()
        {
            var repoMatricula = new MatriculaRepository(ConnectionString, DatabaseType);
            var vencendo = await repoMatricula.ObterVencendoEmDias(10);
            Assert.NotNull(vencendo);
        }
    }
}
