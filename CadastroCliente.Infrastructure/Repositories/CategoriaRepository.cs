using CadastroCliente.Domain.Entities;
using CadastroCliente.Domain.Interfaces;
using CadastroCliente.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CadastroCliente.Infrastructure.Repositories
{
    public class CategoriaRepository : DapperRepository<Categoria>, ICategoriaRepository
    {
        public CategoriaRepository(IDbConnectionFactory factory) : base(factory)
        {
        }
    }
}
