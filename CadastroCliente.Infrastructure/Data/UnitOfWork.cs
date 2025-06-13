using CadastroCliente.Domain.Interfaces;
using CadastroCliente.Infrastructure.Repositories;
using System.Data;

namespace CadastroCliente.Infrastructure.Data
{
    /// <summary>
    /// Coordena repositórios e transação única para múltiplos calls Dapper.
    /// </summary>
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly IDbConnectionFactory _factory;
        private readonly IDbConnection _connection;
        private readonly IDbTransaction _transaction;

        public IClienteRepository Clientes { get; }
        public IMenuRepository Menus { get; }
        public ICategoriaRepository Categorias { get; }

        public UnitOfWork(
            IDbConnectionFactory factory,
            IClienteRepository clienteRepo,
            IMenuRepository menuRepo,
            ICategoriaRepository categoriaRepo)
        {
            _factory = factory;
            _connection = _factory.CreateConnection();
            _transaction = _connection.BeginTransaction();

            Clientes = clienteRepo;
            Menus = menuRepo;
            Categorias = categoriaRepo;

            if (Clientes is DapperRepositoryBase c) c.Transaction = _transaction;
            if (Menus is DapperRepositoryBase m) m.Transaction = _transaction;
            if (Categorias is DapperRepositoryBase cat) cat.Transaction = _transaction;
        }

        public void Commit()
        {
            _transaction.Commit();
            Dispose();
        }

        public void Rollback()
        {
            _transaction.Rollback();
            Dispose();
        }

        public void Dispose()
        {
            _transaction.Dispose();
            _connection.Dispose();
        }
    }
}
