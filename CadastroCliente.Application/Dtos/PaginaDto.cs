using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CadastroCliente.Application.Dtos
{
    public class PaginaDto
    {
        public int PaginaId { get; set; }
        public string Chave { get; set; }
        public string Titulo { get; set; }
        public string Rota { get; set; }
        public List<ControleDto> Controles { get; set; } = new();
        public List<FiltroDto> Filtros { get; set; } = new();
    }
}
