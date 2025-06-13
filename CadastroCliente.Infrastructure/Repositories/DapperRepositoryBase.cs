using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CadastroCliente.Infrastructure.Repositories
{
    /// <summary>
    /// Base para repositórios Dapper que precisam de IDbConnection e IDbTransaction.
    /// </summary>
    public abstract class DapperRepositoryBase
    {
        protected readonly IDbConnection Connection;
        public IDbTransaction Transaction { get; set; }

        protected DapperRepositoryBase(IDbConnection connection)
        {
            Connection = connection;
        }
    }
}
