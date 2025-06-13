using CadastroCliente.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CadastroCliente.Application.Interfaces
{
    public interface IMenuService
    {
        Task<IEnumerable<MenuDto>> ObterMenuCompletoAsync();
    }
}
