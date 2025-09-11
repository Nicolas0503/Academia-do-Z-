using AcademiaDoZe.Application.DTOs;

public interface IAlunoService
{
    Task<AlunoDTO> ObterPorIdAsync(int id);
    Task<IEnumerable<AlunoDTO>> ObterTodosAsync();
    Task<AlunoDTO> AdicionarAsync(AlunoDTO alunoDto);
    Task<AlunoDTO> AtualizarAsync(AlunoDTO alunoDto);
    Task<bool> RemoverAsync(int id);
    Task<AlunoDTO> ObterPorCpfAsync(string cpf); // <-- Adicione esta linha
}
