using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


//Nícolas Bastos

namespace Academia_do_Zé.ValueObjects;
public record Arquivo
{
    public byte[] Conteudo { get; }
    public Arquivo(byte[] conteudo)
    {
        Conteudo = conteudo;
    }
}