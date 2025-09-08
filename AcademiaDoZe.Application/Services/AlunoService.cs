using AcademiaDoZe.Application.DTOs;
using AcademiaDoZe.Application.Interfaces;
using AcademiaDoZe.Application.Mappings;
using Academia_do_Zé.Repositories;

//Nicolas Bastos

namespace AcademiaDoZe.Application.Services
{
    public class AlunoService : IAlunoService
    {
        private readonly IAlunoRepository _alunoRepository;

        public AlunoService(IAlunoRepository alunoRepository)
        {
            _alunoRepository = alunoRepository;
        }

        public async Task<AlunoDTO> ObterPorIdAsync(int id)
        {
            var aluno = await _alunoRepository.ObterPorId(id);
            return aluno?.ToDto()!;
        }

        public async Task<IEnumerable<AlunoDTO>> ObterTodosAsync()
        {
            var alunos = await _alunoRepository.ObterTodos();
            return alunos.Select(a => a.ToDto());
        }

        public async Task<AlunoDTO> AdicionarAsync(AlunoDTO dto)
        {
            var aluno = dto.ToEntity();
            var adicionado = await _alunoRepository.Adicionar(aluno);
            return adicionado.ToDto();
        }

        public async Task<AlunoDTO> AtualizarAsync(AlunoDTO dto)
        {
            var aluno = dto.ToEntity();
            var atualizado = await _alunoRepository.Atualizar(aluno);
            return atualizado.ToDto();
        }

        public async Task<bool> RemoverAsync(int id)
        {
            return await _alunoRepository.Remover(id);
        }

        public async Task<AlunoDTO> ObterPorCpfAsync(string cpf)
        {
            var aluno = await _alunoRepository.ObterPorCpf(cpf);
            return aluno?.ToDto()!;
        }

        public async Task<bool> CpfJaExisteAsync(string cpf, int? id = null)
        {
            return await _alunoRepository.CpfJaExiste(cpf, id);
        }

        public async Task<bool> TrocarSenhaAsync(int id, string novaSenha)
        {
            return await _alunoRepository.TrocarSenha(id, novaSenha);
        }
    }
}
