using CadastroCliente.Domain.Entities;
using CadastroCliente.Domain.Interfaces;
using CadastroCliente.Infrastructure.Data;
using Dapper;
using System.Data;

namespace CadastroCliente.Infrastructure.Repositories
{
    public class ClienteRepository : DapperRepositoryBase, IClienteRepository
    {
        public ClienteRepository(IDbConnectionFactory factory)
            : base(factory.CreateConnection())
        {
        }

        public async Task<int> CreateAsync(Cliente cliente)
        {
            var id = await Connection.ExecuteScalarAsync<int>(
                "sp_CriarCliente",
                new
                {
                    Nome = cliente.Nome,
                    Email = cliente.Email,
                    Telefone = cliente.Telefone,
                    CategoriaId = cliente.CategoriaId,
                    Ativo = cliente.Ativo
                },
                commandType: CommandType.StoredProcedure,
                transaction: Transaction);

            return id;
        }

        public async Task<ClienteListar> GetByIdAsync(int id)
            => await Connection.QueryFirstOrDefaultAsync<ClienteListar>(
                "sp_ObterClientePorId",
                new { ClienteId = id },
                commandType: CommandType.StoredProcedure,
                transaction: Transaction);

        public async Task<IEnumerable<ClienteListar>> ListAsync(string? nome, int? categoriaId)
            => await Connection.QueryAsync<ClienteListar>(
                "SELECT * FROM dbo.fn_ListarClientes(@FiltroNome, @FiltroCategoriaId)",
                new { FiltroNome = nome, FiltroCategoriaId = categoriaId },
                transaction: Transaction);

        public async Task UpdateAsync(Cliente cliente)
        {
            await Connection.ExecuteAsync(
                "sp_AtualizarCliente",
                new
                {
                    ClienteId = cliente.ClienteId,
                    Nome = cliente.Nome,
                    Email = cliente.Email,
                    Telefone = cliente.Telefone,
                    CategoriaId = cliente.CategoriaId,
                    Ativo = cliente.Ativo
                },
                commandType: CommandType.StoredProcedure,
                transaction: Transaction);
        }

        public async Task DeleteAsync(int id)
        {
            await Connection.ExecuteAsync(
                "sp_ExcluirCliente",
                new { ClienteId = id },
                commandType: CommandType.StoredProcedure,
                transaction: Transaction);
        }
    }
}
