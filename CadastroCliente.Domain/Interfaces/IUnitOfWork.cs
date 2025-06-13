using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CadastroCliente.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        IClienteRepository Clientes { get; }
        IMenuRepository Menus { get; }
        ICategoriaRepository Categorias { get; }
        void Commit();
        void Rollback();
    }
}
