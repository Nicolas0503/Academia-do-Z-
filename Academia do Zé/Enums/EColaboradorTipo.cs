using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Nícolas Bastos

namespace Academia_do_Zé.Enums;
public enum EColaboradorTipo
{
    [Display(Name = "Administrador")]
    Administrador = 0,
    [Display(Name = "Atendente")]
    Atendente = 1,
    [Display(Name = "Instrutor")]
    Instrutor = 2
}

