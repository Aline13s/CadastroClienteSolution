using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CadastroCliente.Application.Dtos
{
    public class ControleDto
    {
        public int ControleId { get; set; }
        public int PaginaId { get; set; }
        public string Chave { get; set; } = string.Empty;
        public string Label { get; set; } = string.Empty;
    }
}
