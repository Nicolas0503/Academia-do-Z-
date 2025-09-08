using AcademiaDoZe.Application.DTOs;
using AcademiaDoZe.Application.Interfaces;
using AcademiaDoZe.Application.Mappings;
using Academia_do_Zé.Repositories;

namespace AcademiaDoZe.Application.Services
{
    public class MatriculaService : IMatriculaService
    {
        private readonly IMatriculaRepository _matriculaRepository;

        public MatriculaService(IMatriculaRepository matriculaRepository)
        {
            _matriculaRepository = matriculaRepository;
        }

        public async Task<MatriculaDTO> ObterPorIdAsync(int id)
        {
            var matricula = await _matriculaRepository.ObterPorId(id);
            return matricula?.ToDto()!;
        }

        public async Task<IEnumerable<MatriculaDTO>> ObterTodasAsync()
        {
            var matriculas = await _matriculaRepository.ObterTodos();
            return matriculas.Select(m => m.ToDto());
        }

        public async Task<MatriculaDTO> AdicionarAsync(MatriculaDTO dto)
        {
            var matricula = dto.ToEntity();
            var adicionado = await _matriculaRepository.Adicionar(matricula);
            return adicionado.ToDto();
        }

        public async Task<MatriculaDTO> AtualizarAsync(MatriculaDTO dto)
        {
            var matricula = dto.ToEntity();
            var atualizado = await _matriculaRepository.Atualizar(matricula);
            return atualizado.ToDto();
        }

        public async Task<bool> RemoverAsync(int id)
        {
            return await _matriculaRepository.Remover(id);
        }

        public async Task<IEnumerable<MatriculaDTO>> ObterPorAlunoIdAsync(int alunoId)
        {
            var matriculas = await _matriculaRepository.ObterPorAluno(alunoId);
            return matriculas.Select(m => m.ToDto());
        }

        public async Task<IEnumerable<MatriculaDTO>> ObterAtivasAsync(int alunoId = 0)
        {
            var matriculas = await _matriculaRepository.ObterAtivas(alunoId);
            return matriculas.Select(m => m.ToDto());
        }

        public async Task<IEnumerable<MatriculaDTO>> ObterVencendoEmDiasAsync(int dias)
        {
            var matriculas = await _matriculaRepository.ObterVencendoEmDias(dias);
            return matriculas.Select(m => m.ToDto());
        }
    }
}
