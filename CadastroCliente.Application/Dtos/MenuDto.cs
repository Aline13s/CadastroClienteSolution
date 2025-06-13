using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CadastroCliente.Application.Dtos
{
    public class MenuDto
    {
        public int MenuId { get; set; }
        public string Chave { get; set; }
        public string Titulo { get; set; }
        public List<PaginaDto> Paginas { get; set; } = new();
    }
}
