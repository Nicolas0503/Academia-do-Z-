using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Nícolas Bastos

namespace Academia_do_Zé.Entities;
public class Acesso : Entity
{
    public Pessoa AlunoColaborador { get; private set; }
    public DateTime DataHora { get; private set; }
    public Acesso(Pessoa pessoa, DateTime dataHora)
    {
        AlunoColaborador = pessoa;
        DataHora = dataHora;
    }
}
