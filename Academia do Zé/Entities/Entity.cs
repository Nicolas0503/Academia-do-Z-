using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Nícolas Bastos

namespace Academia_do_Zé.Entities
{
    public abstract class Entity
    {
        public int Id { get; protected set; }
        public Entity(int id = 0)
        {
            Id = id;
        }
    }
}
