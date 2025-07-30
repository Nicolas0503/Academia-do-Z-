using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Academia_do_Zé.ValueObjects;

// Nícolas Bastos

namespace Academia_do_Zé.Entities;
public class Aluno : Pessoa
{
    public Aluno(string nomeCompleto,
    string cpf,

    DateOnly dataNascimento,
    string telefone,
    string email,
    Logradouro endereco,
    string numero,
    string complemento,
    string senha,
    Arquivo foto)

    : base(nomeCompleto, cpf, dataNascimento, telefone, email, endereco, numero, complemento, senha, foto)
    {
    }
}
