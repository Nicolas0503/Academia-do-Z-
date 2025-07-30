using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Academia_do_Zé.Enums;
using Academia_do_Zé.ValueObjects;

//Nícolas Bastos

namespace Academia_do_Zé.Entities;
public class Colaborador : Pessoa
{
    public DateOnly DataAdmissao { get; private set; }
    public EColaboradorTipo Tipo { get; private set; }
    public EColaboradorVinculo Vinculo { get; private set; }
    public Colaborador(string nomeCompleto,
    string cpf,

    DateOnly dataNascimento,
    string telefone,
    string email,
    Logradouro endereco,
    string numero,
    string complemento,
    string senha,
    Arquivo foto,
    DateOnly dataAdmissao,
    EColaboradorTipo tipo,
    EColaboradorVinculo vinculo)

    : base(nomeCompleto, cpf, dataNascimento, telefone, email, endereco, numero, complemento, senha, foto)
    {
        DataAdmissao = dataAdmissao;
        Tipo = tipo;
        Vinculo = vinculo;
    }
}




