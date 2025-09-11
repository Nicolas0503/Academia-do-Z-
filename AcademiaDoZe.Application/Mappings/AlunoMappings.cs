using Academia_do_Zé.Entities;
using Academia_do_Zé.ValueObjects;
using AcademiaDoZe.Application.DTOs;

namespace AcademiaDoZe.Application.Mappings
{
    public static class AlunoMappings
    {
        public static AlunoDTO ToDto(this Aluno aluno)
        {
            return new AlunoDTO
            {
                Id = aluno.Id,
                Nome = aluno.Nome,
                Cpf = aluno.Cpf,
                DataNascimento = aluno.DataNascimento,
                Telefone = aluno.Telefone,
                Email = aluno.Email,
                Endereco = aluno.Endereco?.ToDto()!,
                Numero = aluno.Numero,
                Complemento = aluno.Complemento,
                Senha = aluno.Senha,
                Foto = aluno.Foto != null ? new ArquivoDTO
                {
                    Conteudo = aluno.Foto.Conteudo,
                    ContentType = ".jpg" // Ajuste se houver propriedade de tipo em Arquivo
                } : null!
            };
        }

        public static Aluno ToEntity(this AlunoDTO alunoDto)
        {
            return Aluno.Criar(
                alunoDto.Id,
                alunoDto.Nome,
                alunoDto.Cpf,
                alunoDto.DataNascimento,
                alunoDto.Telefone,
                alunoDto.Email ?? string.Empty,
                alunoDto.Endereco.ToEntity(),
                alunoDto.Numero ?? string.Empty,
                alunoDto.Complemento ?? string.Empty,
                alunoDto.Senha ?? string.Empty,
                alunoDto.Foto != null
                    ? Arquivo.Criar(alunoDto.Foto.Conteudo, alunoDto.Foto.ContentType)
                    : null
            );
        }


        public static Aluno UpdateFromDto(this Aluno aluno, AlunoDTO alunoDto)
        {
            return Aluno.Criar(
                aluno.Id,
                alunoDto.Nome ?? aluno.Nome,
                alunoDto.Cpf ?? aluno.Cpf,
                alunoDto.DataNascimento != default ? alunoDto.DataNascimento : aluno.DataNascimento,
                alunoDto.Telefone ?? aluno.Telefone,
                alunoDto.Email ?? aluno.Email,
                alunoDto.Endereco != null ? alunoDto.Endereco.ToEntity() : aluno.Endereco,
                alunoDto.Numero ?? aluno.Numero,
                alunoDto.Complemento ?? aluno.Complemento,
                alunoDto.Senha ?? aluno.Senha,
                alunoDto.Foto != null ? Arquivo.Criar(alunoDto.Foto.Conteudo, alunoDto.Foto.ContentType) : aluno.Foto
            );
        }
    }
}
