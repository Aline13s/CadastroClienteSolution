using CadastroCliente.Domain.Interfaces;
using CadastroCliente.Infrastructure.Data;
using Dapper;

namespace CadastroCliente.Infrastructure.Repositories
{
    /// <summary>
    /// Implementação genérica de IRepository<T> usando Dapper.
    /// </summary>
    public class DapperRepository<T> : DapperRepositoryBase, IRepository<T> where T : class
    {
        public DapperRepository(IDbConnectionFactory factory)
            : base(factory.CreateConnection())
        {
        }

        public async Task<T> GetByIdAsync(int id)
        {
            var sql = $"SELECT * FROM {typeof(T).Name}s WHERE {typeof(T).Name}Id = @Id";
            return await Connection.QueryFirstOrDefaultAsync<T>(sql, new { Id = id }, Transaction);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            var sql = $"SELECT * FROM {typeof(T).Name}s";
            return await Connection.QueryAsync<T>(sql, transaction: Transaction);
        }

        public async Task AddAsync(T entity)
        {
            // Placeholder: idealmente chamar SP ou gerar INSERT dinâmico
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(T entity)
        {
            // Placeholder: idealmente chamar SP ou gerar UPDATE dinâmico
            throw new NotImplementedException();
        }

        public async Task DeleteAsync(int id)
        {
            var sql = $"DELETE FROM {typeof(T).Name}s WHERE {typeof(T).Name}Id = @Id";
            await Connection.ExecuteAsync(sql, new { Id = id }, Transaction);
        }
    }
}
