using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CadastroCliente.Infrastructure.Data
{
    /// Cria conexões (IDbConnection) configuradas para o SQL Server.
    public interface IDbConnectionFactory
    {
        /// Abre e retorna uma conexão ativa.
        System.Data.IDbConnection CreateConnection();
    }
}
