using CadastroCliente.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CadastroCliente.Domain.Interfaces
{
    public interface IMenuRepository
    {
        Task<IEnumerable<MenuCompleto>> GetMenuCompletoAsync();
    }
}
