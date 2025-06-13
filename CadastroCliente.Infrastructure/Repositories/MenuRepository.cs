using CadastroCliente.Domain.Entities;
using CadastroCliente.Domain.Interfaces;
using CadastroCliente.Infrastructure.Data;
using Dapper;

namespace CadastroCliente.Infrastructure.Repositories
{
    /// <summary>
    /// Repositório de Menu, consumindo a view vw_MenuCompleto.
    /// </summary>
    public class MenuRepository : DapperRepositoryBase, IMenuRepository
    {
        public MenuRepository(IDbConnectionFactory factory)
            : base(factory.CreateConnection())
        {
        }

        public async Task<IEnumerable<MenuCompleto>> GetMenuCompletoAsync()
        {
            var sql = @"SELECT * FROM vw_MenuCompleto";
            return await Connection.QueryAsync<MenuCompleto>(sql, transaction: Transaction);
        }
    }
}
