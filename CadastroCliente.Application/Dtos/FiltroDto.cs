using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CadastroCliente.Application.Dtos
{
    public class FiltroDto
    {
        public int FiltroId { get; set; }
        public int PaginaId { get; set; }
        public string Campo { get; set; } = string.Empty;
        public string Label { get; set; } = string.Empty;
    }
}
