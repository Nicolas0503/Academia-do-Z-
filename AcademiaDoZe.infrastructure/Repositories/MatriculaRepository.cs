using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Academia_do_Zé.Entities;
using Academia_do_Zé.Enums;
using Academia_do_Zé.Repositories;
using Academia_do_Zé.ValueObjects;
using AcademiaDoZe.infrastructure.Data;

//Nícolas Bastos

namespace AcademiaDoZe.infrastructure.Repositories
{
    public class MatriculaRepository : BaseRepository<Matricula>, IMatriculaRepository
    {
        public MatriculaRepository(string connectionString, DatabaseType databaseType) : base(connectionString, databaseType) { }

        public override async Task<Matricula> Adicionar(Matricula entity)
        {
            try
            {
                await using var connection = await GetOpenConnectionAsync();
                string query = _databaseType == DatabaseType.SqlServer
                    ? $"INSERT INTO {TableName} (aluno_id, plano, data_inicio, data_fim, objetivo, restricao_medica, laudo_medico, obs_restricao) OUTPUT INSERTED.id_matricula VALUES (@AlunoId, @Plano, @DataInicio, @DataFim, @Objetivo, @RestricoesMedicas, @LaudoMedico, @ObservacoesRestricoes);"
                    : $"INSERT INTO {TableName} (aluno_id, plano, data_inicio, data_fim, objetivo, restricao_medica, laudo_medico, obs_restricao) VALUES (@AlunoId, @Plano, @DataInicio, @DataFim, @Objetivo, @RestricoesMedicas, @LaudoMedico, @ObservacoesRestricoes); SELECT LAST_INSERT_ID();";
                await using var command = DbProvider.CreateCommand(query, connection);
                command.Parameters.Add(DbProvider.CreateParameter("@AlunoId", entity.AlunoMatricula.Id, DbType.Int32, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@Plano", (int)entity.Plano, DbType.Int32, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@DataInicio", entity.DataInicio, DbType.Date, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@DataFim", entity.DataFim, DbType.Date, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@Objetivo", entity.Objetivo, DbType.String, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@RestricoesMedicas", (int)entity.RestricoesMedicas, DbType.Int32, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@LaudoMedico", (object)entity.LaudoMedico?.Conteudo ?? DBNull.Value, DbType.Binary, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@ObservacoesRestricoes", entity.ObservacoesRestricoes ?? string.Empty, DbType.String, _databaseType));
                var id = await command.ExecuteScalarAsync();
                if (id != null && id != DBNull.Value)
                {
                    var idProperty = typeof(Entity).GetProperty("Id");
                    idProperty?.SetValue(entity, Convert.ToInt32(id));
                }
                return entity;
            }
            catch (DbException ex) { throw new InvalidOperationException($"Erro ao adicionar matrícula: {ex.Message}", ex); }
        }

        public override async Task<Matricula> Atualizar(Matricula entity)
        {
            try
            {
                await using var connection = await GetOpenConnectionAsync();
                string query = $"UPDATE {TableName} SET aluno_id = @AlunoId, plano = @Plano, data_inicio = @DataInicio, data_fim = @DataFim, objetivo = @Objetivo, restricao_medica = @RestricoesMedicas, laudo_medico = @LaudoMedico, obs_restricao = @ObservacoesRestricoes WHERE id_matricula = @Id";
                await using var command = DbProvider.CreateCommand(query, connection);
                command.Parameters.Add(DbProvider.CreateParameter("@Id", entity.Id, DbType.Int32, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@AlunoId", entity.AlunoMatricula.Id, DbType.Int32, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@Plano", (int)entity.Plano, DbType.Int32, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@DataInicio", entity.DataInicio, DbType.Date, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@DataFim", entity.DataFim, DbType.Date, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@Objetivo", entity.Objetivo, DbType.String, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@RestricoesMedicas", (int)entity.RestricoesMedicas, DbType.Int32, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@LaudoMedico", (object)entity.LaudoMedico?.Conteudo ?? DBNull.Value, DbType.Binary, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@ObservacoesRestricoes", entity.ObservacoesRestricoes ?? string.Empty, DbType.String, _databaseType));
                int rowsAffected = await command.ExecuteNonQueryAsync();
                if (rowsAffected == 0)
                {
                    throw new InvalidOperationException($"Nenhuma matrícula encontrada com o ID {entity.Id} para atualização.");
                }
                return entity;
            }
            catch (DbException ex)
            {
                throw new InvalidOperationException($"Erro ao atualizar matrícula com ID {entity.Id}: {ex.Message}", ex);
            }
        }

        public async Task<IEnumerable<Matricula>> ObterPorAluno(int alunoId)
        {
            try
            {
                await using var connection = await GetOpenConnectionAsync();
                string query = $"SELECT * FROM {TableName} WHERE aluno_id = @AlunoId";
                await using var command = DbProvider.CreateCommand(query, connection);
                command.Parameters.Add(DbProvider.CreateParameter("@AlunoId", alunoId, DbType.Int32, _databaseType));
                using var reader = await command.ExecuteReaderAsync();
                var lista = new List<Matricula>();
                while (await reader.ReadAsync())
                {
                    lista.Add(await MapAsync(reader));
                }
                return lista;
            }
            catch (DbException ex) { throw new InvalidOperationException($"Erro ao obter matrículas do aluno {alunoId}: {ex.Message}", ex); }
        }

        public async Task<IEnumerable<Matricula>> ObterAtivas(int idAluno = 0)
        {
            try
            {
                await using var connection = await GetOpenConnectionAsync();
                string query = $"SELECT * FROM {TableName} WHERE data_fim >= {(_databaseType == DatabaseType.SqlServer ? "GETDATE()" :
                "CURRENT_DATE()")} {(idAluno > 0 ? "AND aluno_id = @id" : "")} ";
                await using var command = DbProvider.CreateCommand(query, connection);
                if (idAluno > 0)
                {
                    command.Parameters.Add(DbProvider.CreateParameter("@id", idAluno, DbType.Int32, _databaseType));
                }
                using var reader = await command.ExecuteReaderAsync();
                var matriculas = new List<Matricula>();
                while (await reader.ReadAsync())
                {
                    matriculas.Add(await MapAsync(reader));
                }
                return matriculas;
            }
            catch (DbException ex)
            {
                throw new InvalidOperationException($"Erro ao obter matrículas ativas: {ex.Message}", ex);
            }
        }

        public async Task<IEnumerable<Matricula>> ObterVencendoEmDias(int dias)
        {
            try
            {
                await using var connection = await GetOpenConnectionAsync();
                var dataLimite = DateOnly.FromDateTime(DateTime.Today.AddDays(dias));
                string query = $"SELECT * FROM {TableName} WHERE data_fim <= @DataLimite AND data_fim >= @Hoje";
                await using var command = DbProvider.CreateCommand(query, connection);
                command.Parameters.Add(DbProvider.CreateParameter("@DataLimite", dataLimite, DbType.Date, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@Hoje", DateOnly.FromDateTime(DateTime.Today), DbType.Date, _databaseType));
                using var reader = await command.ExecuteReaderAsync();
                var lista = new List<Matricula>();
                while (await reader.ReadAsync())
                {
                    lista.Add(await MapAsync(reader));
                }
                return lista;
            }
            catch (DbException ex) { throw new InvalidOperationException($"Erro ao obter matrículas vencendo em {dias} dias: {ex.Message}", ex); }
        }

        protected override async Task<Matricula> MapAsync(DbDataReader reader)
        {
            try
            {
                var alunoId = Convert.ToInt32(reader["aluno_id"]);
                var alunoRepository = new AlunoRepository(_connectionString, _databaseType);
                var aluno = await alunoRepository.ObterPorId(alunoId) ?? throw new InvalidOperationException($"Aluno com ID {alunoId} não encontrado.");
                var matricula = Matricula.Criar(
                    alunoMatricula: aluno,
                    plano: (EMatriculaPlano)Convert.ToInt32(reader["plano"]),
                    dataInicio: DateOnly.FromDateTime(Convert.ToDateTime(reader["data_inicio"])),
                    dataFim: DateOnly.FromDateTime(Convert.ToDateTime(reader["data_fim"])),
                    objetivo: reader["objetivo"].ToString()!,
                    restricoesMedicas: (EMatriculaRestricoes)Convert.ToInt32(reader["restricao_medica"]),
                    laudoMedico: reader["laudo_medico"] is DBNull ? null : Arquivo.Criar((byte[])reader["laudo_medico"], "pdf"),
                    observacoesRestricoes: reader["obs_restricao"]?.ToString() ?? string.Empty
                );
                var idProperty = typeof(Entity).GetProperty("Id");
                idProperty?.SetValue(matricula, Convert.ToInt32(reader["id_matricula"]));
                return matricula;
            }
            catch (DbException ex) { throw new InvalidOperationException($"Erro ao mapear dados da matrícula: {ex.Message}", ex); }
        }
    }
}
