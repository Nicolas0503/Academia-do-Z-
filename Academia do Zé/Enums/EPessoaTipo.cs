using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Academia_do_Zé.Enums
{
    public enum EPessoaTipo
    {
        [Display(Name = "Colaborador")]
        Colaborador = 0,

        [Display(Name = "Aluno")]
        Aluno = 1,
    }
}
