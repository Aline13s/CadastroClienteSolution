using CadastroCliente.Application.Dtos;

namespace CadastroCliente.Application.Interfaces
{
    public interface ICategoriaService
    {
        Task<IEnumerable<CategoriaDto>> ListarAsync();
    }
}
