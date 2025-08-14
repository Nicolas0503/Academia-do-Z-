using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Academia_do_Zé.Entities;
using Academia_do_Zé.Enums;
using Academia_do_Zé.Repositories;
using AcademiaDoZe.infrastructure.Data;

//Nícolas Bastos

namespace AcademiaDoZe.infrastructure.Repositories
{
    public class AcessoRepository : BaseRepository<Acesso>, IAcessoRepository
    {
        public AcessoRepository(string connectionString, DatabaseType databaseType) : base(connectionString, databaseType) { }

        public override async Task<Acesso> Adicionar(Acesso entity)
        {
            try
            {
                await using var connection = await GetOpenConnectionAsync();
                string query = _databaseType == DatabaseType.SqlServer
                    ? $"INSERT INTO {TableName} (tipo, pessoa_id, data_hora) OUTPUT INSERTED.id_acesso VALUES (@Tipo, @PessoaId, @DataHora);"
                    : $"INSERT INTO {TableName} (tipo, pessoa_id, data_hora) VALUES (@Tipo, @PessoaId, @DataHora); SELECT LAST_INSERT_ID();";
                await using var command = DbProvider.CreateCommand(query, connection);
                command.Parameters.Add(DbProvider.CreateParameter("@Tipo", (int)entity.Tipo, DbType.Int32, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@PessoaId", entity.AlunoColaborador.Id, DbType.Int32, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@DataHora", entity.DataHora, DbType.DateTime, _databaseType));
                var id = await command.ExecuteScalarAsync();
                if (id != null && id != DBNull.Value)
                {
                    var idProperty = typeof(Entity).GetProperty("Id");
                    idProperty?.SetValue(entity, Convert.ToInt32(id));
                }
                return entity;
            }
            catch (DbException ex) { throw new InvalidOperationException($"Erro ao adicionar acesso: {ex.Message}", ex); }
        }

        public override async Task<Acesso> Atualizar(Acesso entity)
        {
            try
            {
                await using var connection = await GetOpenConnectionAsync();
                string query = $"UPDATE {TableName} SET tipo = @Tipo, pessoa_id = @PessoaId, data_hora = @DataHora WHERE id_acesso = @Id";
                await using var command = DbProvider.CreateCommand(query, connection);
                command.Parameters.Add(DbProvider.CreateParameter("@Id", entity.Id, DbType.Int32, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@Tipo", (int)entity.Tipo, DbType.Int32, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@PessoaId", entity.AlunoColaborador.Id, DbType.Int32, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@DataHora", entity.DataHora, DbType.DateTime, _databaseType));
                int rowsAffected = await command.ExecuteNonQueryAsync();
                if (rowsAffected == 0)
                {
                    throw new InvalidOperationException($"Nenhum acesso encontrado com o ID {entity.Id} para atualização.");
                }
                return entity;
            }
            catch (DbException ex)
            {
                throw new InvalidOperationException($"Erro ao atualizar acesso com ID {entity.Id}: {ex.Message}", ex);
            }
        }

        protected override async Task<Acesso> MapAsync(DbDataReader reader)
        {
            try
            {
                var tipo = (EPessoaTipo)Convert.ToInt32(reader["tipo"]);
                var pessoaId = Convert.ToInt32(reader["pessoa_id"]);
                Pessoa pessoa;
                // Busca a pessoa pelo tipo
                if (tipo == EPessoaTipo.Aluno)
                {
                    var alunoRepository = new AlunoRepository(_connectionString, _databaseType);
                    pessoa = await alunoRepository.ObterPorId(pessoaId) ?? throw new InvalidOperationException($"Aluno com ID {pessoaId} não encontrado.");
                }
                else
                {
                    var colaboradorRepository = new ColaboradorRepository(_connectionString, _databaseType);
                    pessoa = await colaboradorRepository.ObterPorId(pessoaId) ?? throw new InvalidOperationException($"Colaborador com ID {pessoaId} não encontrado.");
                }
                var acesso = Acesso.Criar(
                    tipo: tipo,
                    pessoa: pessoa,
                    dataHora: Convert.ToDateTime(reader["data_hora"])
                );
                var idProperty = typeof(Entity).GetProperty("Id");
                idProperty?.SetValue(acesso, Convert.ToInt32(reader["id_acesso"]));
                return acesso;
            }
            catch (DbException ex) { throw new InvalidOperationException($"Erro ao mapear dados do acesso: {ex.Message}", ex); }
        }

        public async Task<IEnumerable<Acesso>> GetAcessosPorAlunoPeriodo(int? alunoId = null, DateOnly? inicio = null, DateOnly? fim = null)
        {
            try
            {
                await using var connection = await GetOpenConnectionAsync();
                var query = $"SELECT * FROM {TableName} WHERE tipo = @Tipo";
                if (alunoId.HasValue)
                    query += " AND pessoa_id = @AlunoId";
                if (inicio.HasValue)
                    query += " AND data_hora >= @Inicio";
                if (fim.HasValue)
                    query += " AND data_hora <= @Fim";
                await using var command = DbProvider.CreateCommand(query, connection);
                command.Parameters.Add(DbProvider.CreateParameter("@Tipo", (int)EPessoaTipo.Aluno, DbType.Int32, _databaseType));
                if (alunoId.HasValue)
                    command.Parameters.Add(DbProvider.CreateParameter("@AlunoId", alunoId.Value, DbType.Int32, _databaseType));
                if (inicio.HasValue)
                    command.Parameters.Add(DbProvider.CreateParameter("@Inicio", inicio.Value.ToDateTime(new TimeOnly(0, 0)), DbType.DateTime, _databaseType));
                if (fim.HasValue)
                    command.Parameters.Add(DbProvider.CreateParameter("@Fim", fim.Value.ToDateTime(new TimeOnly(23, 59)), DbType.DateTime, _databaseType));
                using var reader = await command.ExecuteReaderAsync();
                var lista = new List<Acesso>();
                while (await reader.ReadAsync())
                {
                    lista.Add(await MapAsync(reader));
                }
                return lista;
            }
            catch (DbException ex) { throw new InvalidOperationException($"Erro ao obter acessos por aluno/periodo: {ex.Message}", ex); }
        }

        public async Task<IEnumerable<Acesso>> GetAcessosPorColaboradorPeriodo(int? colaboradorId = null, DateOnly? inicio = null, DateOnly? fim = null)
        {
            try
            {
                await using var connection = await GetOpenConnectionAsync();
                var query = $"SELECT * FROM {TableName} WHERE tipo = @Tipo";
                if (colaboradorId.HasValue)
                    query += " AND pessoa_id = @ColaboradorId";
                if (inicio.HasValue)
                    query += " AND data_hora >= @Inicio";
                if (fim.HasValue)
                    query += " AND data_hora <= @Fim";
                await using var command = DbProvider.CreateCommand(query, connection);
                command.Parameters.Add(DbProvider.CreateParameter("@Tipo", (int)EPessoaTipo.Colaborador, DbType.Int32, _databaseType));
                if (colaboradorId.HasValue)
                    command.Parameters.Add(DbProvider.CreateParameter("@ColaboradorId", colaboradorId.Value, DbType.Int32, _databaseType));
                if (inicio.HasValue)
                    command.Parameters.Add(DbProvider.CreateParameter("@Inicio", inicio.Value.ToDateTime(new TimeOnly(0, 0)), DbType.DateTime, _databaseType));
                if (fim.HasValue)
                    command.Parameters.Add(DbProvider.CreateParameter("@Fim", fim.Value.ToDateTime(new TimeOnly(23, 59)), DbType.DateTime, _databaseType));
                using var reader = await command.ExecuteReaderAsync();
                var lista = new List<Acesso>();
                while (await reader.ReadAsync())
                {
                    lista.Add(await MapAsync(reader));
                }
                return lista;
            }
            catch (DbException ex) { throw new InvalidOperationException($"Erro ao obter acessos por colaborador/periodo: {ex.Message}", ex); }
        }

        public async Task<Dictionary<TimeOnly, int>> GetHorarioMaisProcuradoPorMes(int mes)
        {
            try
            {
                await using var connection = await GetOpenConnectionAsync();
                string query = $"SELECT data_hora FROM {TableName} WHERE MONTH(data_hora) = @Mes";
                await using var command = DbProvider.CreateCommand(query, connection);
                command.Parameters.Add(DbProvider.CreateParameter("@Mes", mes, DbType.Int32, _databaseType));
                using var reader = await command.ExecuteReaderAsync();
                var horarios = new Dictionary<TimeOnly, int>();
                while (await reader.ReadAsync())
                {
                    var hora = TimeOnly.FromDateTime(Convert.ToDateTime(reader["data_hora"]));
                    if (horarios.ContainsKey(hora))
                        horarios[hora]++;
                    else
                        horarios[hora] = 1;
                }
                return horarios;
            }
            catch (DbException ex) { throw new InvalidOperationException($"Erro ao obter horário mais procurado do mês: {ex.Message}", ex); }
        }

        public async Task<Dictionary<int, TimeSpan>> GetPermanenciaMediaPorMes(int mes)
        {
            // Implementação depende de como os registros de entrada/saída são feitos.
            // Aqui retorna um dicionário vazio para evitar erro de compilação.
            return new Dictionary<int, TimeSpan>();
        }

        public async Task<IEnumerable<Aluno>> GetAlunosSemAcessoNosUltimosDias(int dias)
        {
            // Implementação depende da relação entre alunos e acessos.
            // Aqui retorna uma lista vazia para evitar erro de compilação.
            return new List<Aluno>();
        }
    }
}
