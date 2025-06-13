using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace CadastroCliente.Infrastructure.Data
{
    /// Lê a connection string de "ConnectionStrings:DefaultConnection" em appsettings.json.
    public class DbConnectionFactory : IDbConnectionFactory
    {
        private readonly IConfiguration _configuration;

        public DbConnectionFactory(IConfiguration configuration)
            => _configuration = configuration;

        public IDbConnection CreateConnection()
        {
            var cs = _configuration.GetConnectionString("DefaultConnection");
            var connection = new SqlConnection(cs);
            connection.Open();
            return connection;
        }
    }
}
