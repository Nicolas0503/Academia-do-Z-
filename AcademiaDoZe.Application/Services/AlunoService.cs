using Academia_do_Zé.Repositories;
using AcademiaDoZe.Application.DTOs;
using AcademiaDoZe.Application.Interfaces;
using AcademiaDoZe.Application.Mappings;
using AcademiaDoZe.infrastructure.Repositories;

//Nicolas Bastos
namespace AcademiaDoZe.Application.Services
{
    public class AlunoService : IAlunoService
    {
        // Func que cria instâncias do repositório quando necessário, garantindo freshness e evitando problemas de ciclo de vida
        private readonly Func<IAlunoRepository> _repoFactory;
        public AlunoService(Func<IAlunoRepository> repoFactory)
        {
            _repoFactory = repoFactory ?? throw new ArgumentNullException(nameof(repoFactory));
        }

        public async Task<AlunoDTO> AdicionarAsync(AlunoDTO alunoDto)
        {
            // Exemplo: validação de CPF único (ajuste conforme sua regra de negócio)
            var cpfExistente = await _repoFactory().ObterPorCpf(alunoDto.Cpf);
            if (cpfExistente != null)
            {
                throw new InvalidOperationException($"Aluno com ID {cpfExistente.id}, já cadastrado com o CPF {cpfExistente.Cpf}.");
            }
            var aluno = alunoDto.ToEntity();
            await _repoFactory().Adicionar(aluno);
            return aluno.ToDto();
        }

        public async Task<AlunoDTO> AtualizarAsync(AlunoDTO alunoDto)
        {
            var alunoExistente = await _repoFactory().ObterPorId(alunoDto.Id) ?? throw new KeyNotFoundException($"Aluno ID {alunoDto.Id} não encontrado.");

            // Exemplo: validação de CPF único ao atualizar
            if (!string.Equals(alunoExistente.Cpf, alunoDto.Cpf, StringComparison.OrdinalIgnoreCase))
            {
                var cpfExistente = await _repoFactory().ObterPorCpf(alunoDto.Cpf);
                if (cpfExistente != null && cpfExistente.id != alunoDto.Id)
                {
                    throw new InvalidOperationException($"Aluno com ID {cpfExistente.id}, já cadastrado com o CPF {cpfExistente.Cpf}.");
                }
            }
            var alunoAtualizado = alunoExistente.UpdateFromDto(alunoDto);
            await _repoFactory().Atualizar(alunoAtualizado);
            return alunoAtualizado.ToDto();
        }

        public async Task<AlunoDTO> ObterPorCpfAsync(string cpf)
        {
            var aluno = await _repoFactory().ObterPorCpf(cpf);
            return aluno != null ? aluno.ToDto() : null!;
        }


        public async Task<AlunoDTO> ObterPorIdAsync(int id)
        {
            var aluno = await _repoFactory().ObterPorId(id);
            return (aluno != null) ? aluno.ToDto() : null!;
        }

        public async Task<IEnumerable<AlunoDTO>> ObterTodosAsync()
        {
            var alunos = await _repoFactory().ObterTodos();
            return [.. alunos.Select(a => a.ToDto())];
        }

        public async Task<bool> RemoverAsync(int id)
        {
            var aluno = await _repoFactory().ObterPorId(id);
            if (aluno == null)
            {
                return false;
            }
            await _repoFactory().Remover(id);
            return true;
        }
    }
}
