using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Academia_do_Zé.Entities;

namespace Academia_do_Zé.Repositories
{
    public interface IMatriculaRepository : IRepository<Matricula>
    {
        // Métodos específicos do domínio

        Task<IEnumerable<Matricula>> ObterPorAluno(int alunoId);

        Task<IEnumerable<Matricula>> ObterAtivas();
        Task<IEnumerable<Matricula>> ObterVencendoEmDias(int dias);
    }
}
