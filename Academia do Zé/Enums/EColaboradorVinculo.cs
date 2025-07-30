using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Nícolas Bastos

namespace Academia_do_Zé.Enums;
public enum EColaboradorVinculo
{
    [Display(Name = "CLT")]
    CLT = 0,
    [Display(Name = "Estagiário")]
    Estagio = 1
}
