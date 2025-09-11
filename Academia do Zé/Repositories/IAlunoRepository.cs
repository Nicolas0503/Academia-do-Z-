using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Academia_do_Zé.Entities;

namespace Academia_do_Zé.Repositories
{
    public interface IAlunoRepository : IRepository<Aluno>
    {
        // Métodos específicos do domínio

        Task<Aluno?> ObterPorCpf(string cpf);

        Task<Aluno?> ObterPorEmail(string email);
    }
}
