using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Academia_do_Zé.Entities;

namespace Academia_do_Zé.Repositories
{
    public interface ILogradouroRepository : IRepository<Logradouro>
    {
        // Métodos específicos do domínio

        Task<Logradouro?> ObterPorCep(string cep);

        Task<IEnumerable<Logradouro>> ObterPorCidade(string cidade);
    }
}
