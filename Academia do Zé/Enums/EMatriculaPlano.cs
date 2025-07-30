using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Nícolas Bastos

namespace Academia_do_Zé.Enums;
public enum EMatriculaPlano
{
    [Display(Name = "Mensal")]
    Mensal = 0,
    [Display(Name = "Trimestral")]
    Trimestral = 1,
    [Display(Name = "Semestral")]
    Semestral = 2,
    [Display(Name = "Anual")]
    Anual = 3
}
