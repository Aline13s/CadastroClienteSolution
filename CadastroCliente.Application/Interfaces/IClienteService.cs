using CadastroCliente.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CadastroCliente.Application.Interfaces
{
    public interface IClienteService
    {
        Task<int> CriarAsync(ClienteDto dto);
        Task AtualizarAsync(ClienteDto dto);
        Task ExcluirAsync(int id);
        Task<ClienteListarDto?> ObterPorIdAsync(int id);
        Task<IEnumerable<ClienteListarDto>> ListarAsync(string? nome, int? categoriaId);
    }
}
