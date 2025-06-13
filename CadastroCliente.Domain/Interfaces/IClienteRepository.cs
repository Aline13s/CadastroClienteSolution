using CadastroCliente.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CadastroCliente.Domain.Interfaces
{
    public interface IClienteRepository
    {
        Task<int> CreateAsync(Cliente cliente);
        Task<ClienteListar?> GetByIdAsync(int id);
        Task<IEnumerable<ClienteListar>> ListAsync(string? nome, int? categoriaId);
        Task UpdateAsync(Cliente cliente);
        Task DeleteAsync(int id);
    }
}
