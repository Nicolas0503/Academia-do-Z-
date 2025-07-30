using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Academia_do_Zé.Enums;
using Academia_do_Zé.ValueObjects;

//Nícolas Bastos

namespace Academia_do_Zé.Entities;
public class Matricula : Entity
{
    public Aluno AlunoMatricula { get; private set; }
    public EMatriculaPlano Plano { get; private set; }
    public DateOnly DataInicio { get; private set; }
    public DateOnly DataFim { get; private set; }
    public string Objetivo { get; private set; }
    public EMatriculaRestricoes RestricoesMedicas { get; private set; }
    public string ObservacoesRestricoes { get; private set; }
    public Arquivo LaudoMedico { get; private set; }
    public Matricula(Aluno alunoMatricula,
    EMatriculaPlano plano,
    DateOnly dataInicio,
    DateOnly dataFim,
    string objetivo,
    EMatriculaRestricoes restricoesMedicas,
    Arquivo laudoMedico)

    : base()
    {
        AlunoMatricula = alunoMatricula;
        Plano = plano;
        DataInicio = dataInicio;
        DataFim = dataFim;
        Objetivo = objetivo;
        RestricoesMedicas = restricoesMedicas;
        LaudoMedico = laudoMedico;
    }
}
